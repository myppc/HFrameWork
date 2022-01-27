using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HFrameWork.Script.Res
{
    /// <summary>
    /// 管理AB资源包
    /// </summary>
    public class AssetsBundleMgr: SingletonClass<AssetsBundleMgr>
    {
        #region 变量
        Dictionary<string, AssetBundle> bundleDict;
        #endregion

        #region 构造方法
        public AssetsBundleMgr()
        {
            bundleDict = new Dictionary<string, AssetBundle>();
        }
        #endregion

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
    }
}
