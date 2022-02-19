using HFrameWork.Script.Pool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HFrameWork.Script.Res
{
    /// <summary>
    /// 管理AB资源包
    /// </summary>
    public class AssetsBundleMgr : SingletonClass<AssetsBundleMgr>
    {
        #region 变量
        Dictionary<string, AssetBundle> bundleDict;
        Dictionary<string, ABLoader> loaderDic;
        #endregion

        #region 构造方法
        public AssetsBundleMgr()
        {
            bundleDict = new Dictionary<string, AssetBundle>();
            loaderDic = new Dictionary<string, ABLoader>();
        }
        #endregion


        #region 私有方法

        #endregion


        #region 公用方法

        /// <summary>
        /// 获取AssetBundle
        /// </summary>
        /// <returns></returns>
        public AssetBundle GetBundle(string abName)
        {
            if (bundleDict.TryGetValue(abName, out AssetBundle ab))
            {
                return ab;
            }
            return null;
        }

        /// <summary>
        /// 添加AB包
        /// </summary>
        /// <param name="abName"></param>
        /// <param name="ab"></param>
        /// <returns></returns>
        public void SetBundle(string abName, AssetBundle ab)
        {
            if (bundleDict.ContainsKey(abName))
            {
                return;
            }
            bundleDict.Add(abName, ab);
        }

        /// <summary>
        /// 加载AB包
        /// </summary>
        /// <param name="abName"></param>
        public AssetBundle LoadAssetBundle(string abName)
        {
            var ab = GetBundle(abName);
            if (ab != null)
            {
                return ab;
            }
            var loader = new ABLoader(abName);
            loader.Load();
            return GetBundle(abName);
        }

        /// <summary>
        /// 异步加载AB包
        /// </summary>
        /// <param name="abName"></param>
        /// <param name="finishCallback"></param>
        public void LoadAssetBundle(string abName, Action<string, AssetBundle> finishCallback)
        {
            var ab = GetBundle(abName);
            if (ab != null)
            {
                finishCallback?.Invoke(abName, ab);
                return;
            }
            var loader = new ABLoader(abName); 
            loader.LoadAsync((abName,ab)=> {
                if (loaderDic.ContainsKey(abName))
                {
                    loaderDic.Remove(abName);
                }
                finishCallback?.Invoke(abName, ab);
            });
            SetLoader(abName, loader);
        }

        /// <summary>
        /// 获取loader
        /// </summary>
        /// <param name="abName"></param>
        /// <returns></returns>
        public ABLoader GetLoader(string abName)
        {
            if (loaderDic.TryGetValue(abName, out ABLoader ret))
            {
                return ret;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 设置abloader
        /// </summary>
        /// <param name="abName"></param>
        /// <param name="loader"></param>
        public void SetLoader(string abName, ABLoader loader)
        {
            if (loaderDic.ContainsKey(abName))
            {
                loaderDic[abName] = loader;
                return;
            }
            loaderDic.Add(abName, loader);
        }

        /// <summary>
        /// 通过AB名字卸载指定AB
        /// </summary>
        /// <param name="abName"></param>
        public void UnLoadABCacheByName(string abName)
        {
            if (bundleDict.TryGetValue(abName, out var abData))
            {
                abData.Unload(true);
                bundleDict.Remove(abName);
            }
        }

        /// <summary>
        /// 卸载全部没有用到的AbCache
        /// </summary>
        public void UnLoadAllABCache()
        {
            var cacheABList = GoPoolManager.Ins.FilterCacheAssets();
            var depensList = new List<string>();
            //找出用到的包的所有依赖
            foreach (var abName in cacheABList)
            {
                depensList = ToolFunc.FilterDepends(abName, depensList);
            }
            var removeList = new List<string>(); 
            foreach (var item in bundleDict)
            {
                var bundleName = item.Key;
                if (!depensList.Contains(bundleName))
                {
                    removeList.Add(bundleName);
                }
            }
            //清理已经加载完成的
            foreach(var removeAB in removeList)
            {
                UnLoadABCacheByName(removeAB);
            }

            //清理正在加载的
            var requestList = ABRequestMgr.Ins.GetAllRequestNames();
            foreach (var bundleName in requestList)
            {
                if (!depensList.Contains(bundleName))
                {
                    ABRequestMgr.Ins.StopRequest(bundleName);
                }
            }
        }

        #endregion
    }
}
