using Assets.HFrameWork.Script.Tool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HFrameWork.Script.SBP
{
    /// <summary>
    /// 配置主数据
    /// </summary>
    [Serializable]
    public class Manifest
    {
        #region 公共字段
        /// <summary>
        /// 版本号
        /// </summary>
        public List<int> version;

        /// <summary>
        /// 所有的模块信息
        /// </summary>
        public Dictionary<string, ManifestModule> modules;
        #endregion

        #region 构造方法
        public Manifest(List<int> version)
        {
            this.version = version;
            modules = new Dictionary<string, ManifestModule>();
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 获得模块
        /// </summary>
        /// <param name="key"></param>
        public ManifestModule GetModule(string key)
        {
            if (modules.TryGetValue(key, out ManifestModule moduleData))
            {
                return moduleData;
            }
            return null;
        }

        /// <summary>
        /// 添加模块数据
        /// </summary>
        /// <param name="module"></param>
        /// <param name="abName"></param>
        /// <param name="path"></param>
        /// <param name="deps"></param>
        /// <param name="assets"></param>
        public void Add(string module, string abName, string fullpath, List<string> deps, List<ManifestAsset> assets)
        {
            // 确保有该模块数据
            if (!modules.TryGetValue(module, out ManifestModule mData))
            {
                mData = new ManifestModule(new Dictionary<string, ManifestAB>(), new Dictionary<string, ManifestAsset>());
                modules.Add(module, mData);
            }

            // ab包数据
            ManifestAB abData = new ManifestAB(
                FileHelper.GetFileMD5(fullpath),
                File2CRC32.GetFileCRC32(fullpath),
                deps,
                new FileInfo(fullpath).Length);
            mData.AddAB(abName, abData);

            // 遍历所有的资源
            foreach (var asset in assets)
            {
                // 添加资源数据
                string assetName = Path.GetFileName(asset.path);
                mData.AddAsset(assetName, asset);
            }
        }

        /// <summary>
        /// 移除module相关数据
        /// </summary>
        /// <param name="moduleName"></param>
        public void RemoveModule(string moduleName)
        {
            if (modules.TryGetValue(moduleName, out ManifestModule mData))
            {
                modules.Remove(moduleName);
            }
        }

        #endregion

        #region 其他方法
        #endregion
    }

}
