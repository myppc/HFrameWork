using Assets.HFrameWork.Script.SBP;
using Assets.HFrameWork.Script.Tool;
using HFrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HFrameWork.Script.Res
{
    /// <summary>
    /// 资源类型
    /// </summary>
    public enum ERes
    {
        GameObject,
    }

    public class ResMgr :SingletonClass<ResMgr>
    {
        #region 公用方法
        /// <summary>
        /// 生成本地Manifest
        /// </summary>
        /// <returns></returns>
        public Manifest CreateLoacalManifest()
        {
            Manifest manifest = null;
#if UNITY_EDITOR
            manifest = new Manifest(new List<int>() { 0, 0, 0 });

            // 遍历所有资源文件
            FileHelper.PairAllAssets(AppConfig.ASSETS_PATH, (module, fullPath, uPath, fileName, isScene) =>
            {
                // 确保有模块
                if (!manifest.modules.TryGetValue(module, out ManifestModule moduleData))
                {
                    moduleData = new ManifestModule(new Dictionary<string, ManifestAB>(), new Dictionary<string, ManifestAsset>());
                    manifest.modules.Add(module, moduleData);
                }

                // 记录资源
                moduleData.AddAsset(fileName, new ManifestAsset("", uPath));
            });
#endif
            return manifest;
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="eRes"></param>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <returns></returns>

        public T Load<T>(ERes eRes,string path,string name)
        {
            switch (eRes)
            {
                case ERes.GameObject:
                    return (T)LoadGameObject(path, name);
            }
            return null;
        }

        #endregion

        #region 私有方法
        /// <summary>
        /// 加载gameobject
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private UnityEngine.Object LoadGameObject(string path,string name)
        {

            var go = GoPoolManager.GetIns().CreateGoFromCache(path, name);
            if (go != null)
            {
                return go;
            }
            var asset =  GetManifestAsset(path, name);
#if UNITY_EDITOR
            //编辑器模式下生成
            go = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(asset.path);
#else
            
#endif
            GoPoolManager.GetIns().AddTag(go, path, name);
            return go;
        }

        /// <summary>
        /// 获取manifestAssets数据
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private ManifestAsset GetManifestAsset(string path, string name)
        {
            if (AppConfig.manifest == null)
            {
                return null;
            }

            var manifest = AppConfig.manifest;
            var module = manifest.GetModule(path);
            ManifestAsset ret = module.GetAsset(name);
            return ret;
        }
        #endregion
    }
}
