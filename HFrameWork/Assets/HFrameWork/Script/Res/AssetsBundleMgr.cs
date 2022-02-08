using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HFrameWork.Script.Res
{
    public struct ABAsync
    {
        public string abName;
        public List<ABLoader> loaderList;
        public Action<string, AssetBundle> finishCallBack;
    }

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

        #endregion
    }
}
