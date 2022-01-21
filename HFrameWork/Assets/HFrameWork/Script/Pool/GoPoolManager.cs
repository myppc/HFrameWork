using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对象
/// </summary>

namespace HFrameWork
{

    /// <summary>
    /// 对象池类型
    /// </summary>
    public enum PoolType
    { 
        STATIC,//静态池，永久不销毁
        DYNAMIC,//动态池，动态添加，永久不销毁
        SCENE,//该场景对象池，每次在推出场景时销毁，在载入时自动加载
        TEMP,//该场景零时对象池，退出场景时销毁
    }

    /// <summary>
    /// 缓存信息
    /// </summary>
    public struct CacheInfo
    {
        public string key;
        public PoolType poolType;
        public int cacheCount;
        public string sceneName;
        public string path;
        public string name;
    }

    public class GoPoolManager
    {
        public static GoPoolManager instance;
        private GameObject poolRoot;
        private Dictionary<PoolType ,Transform> poolDict;
        public Dictionary<string, CacheInfo> cacheInfoDict;
        public Dictionary<string, List<GameObject>> GoDict;


        public static GoPoolManager GetIns()
        { 
            if(instance == null)
            {
                instance = new GoPoolManager();
            }

            return instance;
        }

        /// <summary>
        /// 池子基础初始化
        /// </summary>
        public void Init()
        {
            cacheInfoDict = new Dictionary<string, CacheInfo>();
            GoDict = new Dictionary<string, List<GameObject>>();
            poolDict = new Dictionary<PoolType, Transform>();
            InitPoolGo();
        }

        /// <summary>
        /// 初始化对象池
        /// </summary>
        public void InitPoolGo()
        {
            poolRoot = new GameObject();
            poolRoot.name = "RootPool";
            GameObject.DontDestroyOnLoad(poolRoot);
            var staticPool = new GameObject();
            staticPool.name = "Static";
            staticPool.transform.SetParent(poolRoot.transform, false);
            var dynamicPool = new GameObject();
            dynamicPool.name = "Dynamic";
            dynamicPool.transform.SetParent(poolRoot.transform, false);

            var scenePool = new GameObject();
            scenePool.name = "Scene";
            scenePool.transform.SetParent(poolRoot.transform, false);

            var tempPool = new GameObject();
            tempPool.name = "Temp";
            tempPool.transform.SetParent(poolRoot.transform, false);


            poolDict.Add(PoolType.STATIC ,staticPool.transform);
            poolDict.Add(PoolType.DYNAMIC, dynamicPool.transform);
            poolDict.Add(PoolType.SCENE, scenePool.transform);
            poolDict.Add(PoolType.TEMP, tempPool.transform);

        }

        /// <summary>
        /// 注册对象池信息
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="name">prefabName</param>
        /// <param name="poolType">缓存类型</param>
        /// <param name="cacheCount">缓存个数</param>
        public void RegisterCacheInfo(string path,string name,PoolType poolType  = PoolType.TEMP,int cacheCount  = 20,string sceneName = "")
        {
            poolType = poolType == PoolType.STATIC ? PoolType.DYNAMIC : poolType;
            var key = string.Format("{0}/{1}", path, name);
            CacheInfo cacheInfo;
            cacheInfo.key = key;
            cacheInfo.poolType = poolType;
            cacheInfo.cacheCount = cacheCount;
            cacheInfo.sceneName = sceneName;
            cacheInfo.name = name;
            cacheInfo.path = path;
            if (cacheInfoDict.ContainsKey(key))
            {
                cacheInfoDict.Remove(key);
            }
            cacheInfoDict.Add(key,cacheInfo);
        }

        /// <summary>
        /// 根据key来清理对象池
        /// </summary>
        /// <param name="key"></param>
        public void ClearCacheByKey(string key)
        {
            if (!GoDict.ContainsKey(key))
            {
                return;
            }
            var list = GoDict[key];
            foreach (var item in list)
            {
                GameObject.Destroy(item);
            }
            list.Clear();
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
                RegisterCacheInfo(path, name);
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
            else
            {
                var go =  CreateGo(path , name);
                return go;
            }
        }

        /// <summary>
        /// 直接new出一个对象，并添加信息
        /// </summary>
        public GameObject CreateGo(string path, string name)
        {
            var key = string.Format("{0}/{1}", path, name);
            GameObject ret = GameObject.Instantiate(Resources.Load<GameObject>(key));
            var objCacheInfo = ret.AddComponent<ObjectCacheInfo>();
            objCacheInfo.cacheInfo = cacheInfoDict[key];
            return ret;
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
            var key = string.Format("{0}/{1}", info.cacheInfo.path, info.cacheInfo.name);
            if (!GoDict.ContainsKey(key))
            {
                GoDict.Add(key,new List<GameObject>());
            }
            var list = GoDict[key];
            //判断缓存是否饱和，如果没有饱和则添加对象到对象池，否则销毁对象
            if (list.Count < info.cacheInfo.cacheCount)
            {
                list.Add(go);
                Transform parent = poolDict[info.cacheInfo.poolType];
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
        /// <param name="sceneName"></param>
        public void ClearSceneCache(string sceneName)
        {
            foreach(var item in cacheInfoDict)
            {
                var key = item.Key;
                var cacheInfo = item.Value;
                if (cacheInfo.sceneName == sceneName && cacheInfo.poolType == PoolType.TEMP)
                {
                    ClearCacheByKey(key);
                }
            }
        }

        /// <summary>
        /// 加载指定场景需要缓存的对象
        /// </summary>
        /// <param name="sceneName"></param>
        public void LoadCacheByScene(string sceneName)
        {
            foreach (var item in cacheInfoDict)
            {
                var cacheInfo = item.Value;
                if (cacheInfo.sceneName == sceneName)
                {
                    if (!GoDict.ContainsKey(cacheInfo.key))
                    {
                        GoDict.Add(cacheInfo.key, new List<GameObject>());
                    }
                    var list = GoDict[cacheInfo.key];
                    var count = Mathf.Max(cacheInfo.cacheCount - list.Count, 0);
                    for (var i = 0; i < count; i++)
                    { 
                        var go = CreateGo(cacheInfo.path,cacheInfo.name);
                        RecoveryGo(go);
                    }
                }
            }
        }
    }
}