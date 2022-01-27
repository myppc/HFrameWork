using Assets.HFrameWork.Script.SBP;
using Assets.HFrameWork.Script.Tool;
using HFrameWork;
using Newtonsoft.Json;
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
    /// 资源类型
    /// </summary>
    public enum ERes
    {
        GameObject,
        Sprite,
        Atlas,
        Audio,
    }

    public class ResMgr :SingletonClass<ResMgr>
    {
        #region 私有变量
        private Dictionary<string, AssetBundle> abDataDict;
        #endregion

        #region 构造方法
        public ResMgr()
        {
            abDataDict = new Dictionary<string, AssetBundle>();
        }
        #endregion

        #region 公用方法

        

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="eRes"></param>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public T Load<T>(ERes eRes,string path,string name) where T:UnityEngine.Object
        {
            switch (eRes)
            {
                case ERes.GameObject:
                    return (T)LoadGameObject(path, name);
                default:
                    return LoadAssets<T>(path, name);

            }
            return null;
        }


        public void LoadManifest()
        {
            switch (AppConfig.runMode)
            {
                case ERunMode.Local:
                    AppConfig.manifest = CreateLoacalManifest();
                    break;
                case ERunMode.Package:
                    AppConfig.manifest = LoadPackageManifest();
                    break;
            }
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 加载streamingAssets下的manifest
        /// </summary>
        /// <returns></returns>
        private Manifest LoadPackageManifest()
        {
            var jsonStr = File.ReadAllText(AppConfig.MANIFEST_LOAD_PATH);
            return JsonConvert.DeserializeObject<Manifest>(jsonStr);
        }

        /// <summary>
        /// 生成本地Manifest
        /// </summary>
        /// <returns></returns>
        private Manifest CreateLoacalManifest()
        {
            Manifest manifest = null;
#if UNITY_EDITOR
            manifest = new Manifest(new List<int>() { 0, 0, 0 });

            // 遍历所有资源文件
            FileHelper.PairAllAssets(AppConfig.ASSETS_PATH, (module, resName, fullPath, uPath, fileName, isScene) =>
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
        /// 加载非GameObject资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private T LoadAssets<T>(string path, string name) where T: UnityEngine.Object
        {
            var asset = GetManifestAsset(path, name);
#if UNITY_EDITOR
            //编辑器模式下生成
            var sprite = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(asset.path);
            return sprite;
#else
            return null;
#endif
        }

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
            var manifestAsset =  GetManifestAsset(path, name);
#if UNITY_EDITOR
            //编辑器模式下生成
            var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(manifestAsset.path);
            go = UnityEngine.Object.Instantiate(asset as GameObject);
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

        /// <summary>
        /// 同步加载AB包并保存
        /// </summary>
        /// <param name="abName"></param>
        /// <returns></returns>
        private AssetBundle LoadAB(string abName)
        {
            var succ = abDataDict.TryGetValue(abName, out AssetBundle abData);
            if (succ)
            {
                return abData;
            }
            AssetBundle ab = null;
            return null;
            //TODO 加载过程中需要考虑依赖加载
            //if (AppConfig.runMode == ERunMode.Local)
            //{
            //    ab = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, $"{AppConfig.ASSETS_PLAT}/{abName}"));
            //}
            //abDataDict.Add(abName, ab);
            //return ab;
        }

        /// <summary>
        /// 异步加载AB包并保存
        /// </summary>
        /// <param name="abName"></param>
        /// <param name="loadFinish"></param>
        private void LoadAbSync(string abName, Action<AssetBundle> loadFinish)
        {
            var succ = abDataDict.TryGetValue(abName, out AssetBundle abData);
            if (succ)
            {
                loadFinish?.Invoke(abData);
            }
        }

        #endregion
    }
}
