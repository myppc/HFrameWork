using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.HFrameWork.Script.SBP
{
    /// <summary>
    /// 模块配置数据
    /// </summary>
    [Serializable]
    public class ManifestModule
    {
        #region 不序列化
        /// <summary>
        /// 该模块下所有的AB信息
        /// </summary>
        public Dictionary<string, ManifestAB> abs;

        /// <summary>
        /// 该模块下所有的资源信息
        /// </summary>
        public Dictionary<string, ManifestAsset> assets;
        #endregion

        #region 构造方法
        public ManifestModule() : base()
        {
        }
        public ManifestModule(Dictionary<string, ManifestAB> abs, Dictionary<string, ManifestAsset> assets) : base()
        {
            this.abs = abs;
            this.assets = assets;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 添加AB包信息
        /// </summary>
        /// <param name="abName"></param>
        /// <param name="abData"></param>
        public void AddAB(string abName, ManifestAB abData)
        {
            abs.Add(abName, abData);
        }

        /// <summary>
        /// 添加资源信息
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="assetData"></param>
        public void AddAsset(string assetName, ManifestAsset assetData)
        {
            assets.Add(assetName, assetData);
        }

        /// <summary>
        /// 获得ab
        /// </summary>
        /// <param name="key"></param>
        public ManifestAB GetAB(string key)
        {
            if (abs.TryGetValue(key, out ManifestAB abData))
            {
                return abData;
            }
            return null;
        }

        /// <summary>
        /// 获得资源
        /// </summary>
        /// <param name="key"></param>
        public ManifestAsset GetAsset(string key)
        {
            if (assets.TryGetValue(key, out ManifestAsset assetData))
            {
                return assetData;
            }
            return null;
        }
        #endregion
    }
}
