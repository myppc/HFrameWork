using Assets.HFrameWork.Script.Res;
using HFrameWork.Script.SceneMgr;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 对象
/// </summary>

namespace HFrameWork.Script.Pool
{

    /// <summary>
    /// 对象池类型
    /// </summary>
    public enum PoolType
    {
        TEMP,//该场景零时对象池，退出场景时销毁
        STATIC,//静态池，永久不销毁
        DYNAMIC,//动态池，动态添加，永久不销毁
        SCENE,//该场景对象池，每次在推出场景时销毁，在载入时自动加载
        
    }

    /// <summary>
    /// 缓存信息
    /// </summary>
    public struct CacheInfo
    {
        public string key;
        public List<SaveInfo> saveInfos;
        public string path;
        public string assetName;
    }

    public struct SaveInfo
    {
        public PoolType poolType;
        public int cacheCount;
        public string sceneKey;
    }


    public class GoPoolManager:SingletonClass<GoPoolManager>
    {
        private string curSceneKey = "DEFAULT";
        private GameObject poolRoot;
        private Dictionary<string, CacheInfo> cacheInfoDict;
        private Dictionary<string, List<GameObject>> GoDict;

        /// <summary>
        /// 池子基础初始化
        /// </summary>
        public bool Init()
        {
            cacheInfoDict = new Dictionary<string, CacheInfo>();
            GoDict = new Dictionary<string, List<GameObject>>();
            InitPoolGo();
            return true;
        }

        /// <summary>
        /// 注册对象池信息
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="name">prefabName</param>
        /// <param name="poolType">缓存类型</param>
        /// <param name="cacheCount">缓存个数</param>
        public void RegisterCacheInfo(string path,string name,PoolType poolType  = PoolType.TEMP,int cacheCount  = 20,string sceneKey = "")
        {
            sceneKey = sceneKey != "" ? sceneKey : curSceneKey;
            poolType = poolType == PoolType.STATIC ? PoolType.DYNAMIC : poolType;
            var key = string.Format("{0}/{1}", path, name);
            if (!cacheInfoDict.ContainsKey(key))
            {
                CacheInfo addCache;
                addCache.saveInfos = new List<SaveInfo>();
                addCache.key = key;
                addCache.assetName = name;
                addCache.path = path;
                cacheInfoDict.Add(key, addCache);
            }
            //如果缓存信息中有这个场景的信息了，那么替换成新的
            var cacheInfo = cacheInfoDict[key];
            var index = cacheInfo.saveInfos.FindIndex((item) => {
                return item.sceneKey == sceneKey;
            });
            if (index != -1)
            {
                cacheInfo.saveInfos.RemoveAt(index);
            }

            SaveInfo saveInfo;
            saveInfo.poolType = poolType;
            saveInfo.cacheCount = cacheCount;
            saveInfo.sceneKey = sceneKey;
            cacheInfo.saveInfos.Add(saveInfo);
        }

        /// <summary>
        /// 通过对象池获取对象
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <returns>返回池子中的对象，如果池子空了则返回NULL</returns>
        public GameObject CreateGoFromCache(string path, string name)
        {
            var key = string.Format("{0}/{1}", path, name);
            if (!cacheInfoDict.ContainsKey(key))
            {
                return null;
            }
            if (!GoDict.ContainsKey(key))
            {
                return null;
            }
            var list = GoDict[key];
            if (list.Count > 0)
            {
                var ret = list[0];
                list.RemoveAt(0);
                ret.SetActive(true);
                ret.transform.SetParent(null, false);
                return ret;
            }
            return null;
        }

        /// <summary>
        /// 回收对象
        /// </summary>
        /// <param name="go"></param>
        public void RecoveryGo(GameObject go)
        {
            var info = go.GetComponent<ObjectCacheInfo>();
            //如果没有缓存信息，则直接销毁对象
            if (info == null)
            {
                GameObject.Destroy(go);
                return;
            }
            var key = string.Format("{0}/{1}", info.path, info.assetName);
            if (!GoDict.ContainsKey(key))
            {
                GoDict.Add(key,new List<GameObject>());
            }
            var list = GoDict[key];
            //判断当前场景是否需要缓存这个对象
            var index = cacheInfoDict[key].saveInfos.FindIndex((item)=> {
                return curSceneKey == item.sceneKey;
            });
            //若当前场景不需要缓存该对象，就直接删除
            if (index == -1)
            {
                GameObject.Destroy(go);
                return;
            }
            var maxCount = cacheInfoDict[key].saveInfos[index].cacheCount;

            //判断缓存是否饱和，如果没有饱和则添加对象到对象池，否则销毁对象
            if (list.Count < maxCount)
            {
                list.Add(go);
                Transform parent = poolRoot.transform;
                go.transform.SetParent(parent, false);
                go.SetActive(false);
            }
            else
            {
                GameObject.Destroy(go);
            }
        }

        /// <summary>
        /// 清理当前场景缓存
        /// </summary>
        /// <param name="sceneKey"></param>
        public void ClearSceneCache(string sceneKey = "",bool onlyTemp = false)
        {
            sceneKey = sceneKey != "" ? sceneKey : curSceneKey;
            foreach (var item in cacheInfoDict)
            {
                var key = item.Key;
                var cacheInfo = item.Value;
                var maxCacheCount = 0;
                var isRemove = cacheInfo.saveInfos.FindIndex((item) => {
                    return (item.sceneKey == sceneKey && (item.poolType == PoolType.TEMP || (item.poolType == PoolType.SCENE && !onlyTemp) )); 
                });
                //这一类资源没有缓存在该场景下
                if (isRemove == -1)
                {
                    continue;
                }
                foreach (var saveInfo in cacheInfo.saveInfos)
                {
                    //找出其他场景下常驻的最大个数
                    if (saveInfo.sceneKey != sceneKey)
                    {
                        if ((saveInfo.poolType == PoolType.DYNAMIC || saveInfo.poolType == PoolType.STATIC) || (!onlyTemp && saveInfo.poolType == PoolType.SCENE))
                        {
                            maxCacheCount = Math.Max(saveInfo.cacheCount, maxCacheCount);
                        }
                    }

                }

                ClearCacheByKey(key, maxCacheCount);
            }
        }

        /// <summary>
        /// 加载指定场景需要缓存的对象
        /// </summary>
        /// <param name="sceneKey"></param>
        public void LoadCacheByScene(string sceneKey)
        {
            curSceneKey = sceneKey;
            foreach (var item in cacheInfoDict)
            {
                var cacheInfo = item.Value;
                var loadItemIndex = cacheInfo.saveInfos.FindIndex((item) => {
                    return (item.sceneKey == sceneKey && item.poolType == PoolType.SCENE);
                });
                if (loadItemIndex != -1)
                {
                    if (!GoDict.ContainsKey(cacheInfo.key))
                    {
                        GoDict.Add(cacheInfo.key, new List<GameObject>());
                    }
                    var list = GoDict[cacheInfo.key];
                    var count = Mathf.Max(cacheInfo.saveInfos[loadItemIndex].cacheCount - list.Count, 0);
                    var recoverylist = new List<GameObject>();
                    for (var i = 0; i < count; i++)
                    {
                        ResMgr.Ins.LoadAsync(ERes.GameObject, cacheInfo.path, cacheInfo.assetName,(go)=> {
                            AddTag(go as GameObject, cacheInfo.path, cacheInfo.assetName);
                            RecoveryGo(go as GameObject);
                        },true);
                    }
                }

            }
        }

        /// <summary>
        /// 为GameObject 打上标签，方便后续回收
        /// </summary>
        /// <param name="go"></param>
        /// <param name="path"></param>
        /// <param name=""></param>
        public void AddTag(GameObject go, string path,string name)
        {
            var key = string.Format("{0}/{1}", path, name);
            if(cacheInfoDict.ContainsKey(key))
            {
                var objCacheInfo = go.AddComponent<ObjectCacheInfo>();
                objCacheInfo.key = key;
                objCacheInfo.assetName = name;
                objCacheInfo.path = path;
            }
        }

        /// <summary>
        /// 遍历当前所有在Cache中缓存的对象用到的AB包
        /// </summary>
        /// <returns></returns>
        public List<string> FilterCacheAssets()
        {
            var abDict = new Dictionary<string,bool>();
            foreach (var item in GoDict)
            {
                var goList = item.Value;
                if (goList.Count <= 0)
                {
                    continue;
                }
                var go = goList[0];
                var cacheInfo = go.GetComponent<ObjectCacheInfo>();
                //cacheInfo.path
                var maniestAsset = ToolFunc.GetManifestAsset(cacheInfo.path, cacheInfo.assetName);
                if (!abDict.ContainsKey(maniestAsset.abName))
                {
                    abDict.Add(maniestAsset.abName, true);
                }
            }
            var keys = abDict.Keys.ToList<string>();
            return keys;
        }

        #region 私有方法
        /// <summary>
        /// 初始化对象池
        /// </summary>
        private void InitPoolGo()
        {
            poolRoot = new GameObject();
            poolRoot.name = "RootPool";
            GameObject.DontDestroyOnLoad(poolRoot);
        }

        /// <summary>
        /// 根据key来清理对象池
        /// </summary>
        /// <param name="key"></param>
        private void ClearCacheByKey(string key, int holdCount = 0)
        {
            if (!GoDict.ContainsKey(key))
            {
                return;
            }
            var list = GoDict[key];
            var removeCount = Math.Max(list.Count - holdCount, 0);
            for (var i = 0; i < removeCount; i++)
            {
                var item = list[0];
                list.RemoveAt(0);
                GameObject.Destroy(item);
            }

        }
        #endregion
    }
}