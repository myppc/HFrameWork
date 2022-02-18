using Assets.HFrameWork.Script.SBP;
using Assets.HFrameWork.Script.Tool;
using HFrameWork;
using HFrameWork.Script.Pool;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.U2D;

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
        TextAsset,
    }

    public class ResMgr :SingletonClass<ResMgr>
    {
        #region 私有变量

        #endregion

        #region 构造方法
        public ResMgr()
        { 

        }
        #endregion

        #region 公用方法
        /// <summary>
        /// 同步加载资源
        /// </summary>
        /// <param name="eRes"></param>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public UnityEngine.Object Load(ERes eRes,string path,string name,bool ignoreCache = false)
        {
            switch (eRes)
            {
                case ERes.GameObject:
                    return LoadGameObject(path, name, ignoreCache);
                default:
                    return LoadAssets(eRes,path, name);
            }
            return null;
        }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eRes"></param>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <param name="finishCallback"></param>
        public void LoadAsync(ERes eRes, string path, string name, Action<UnityEngine.Object> finishCallback = null,bool ignoreCache = false)
        {
            switch (eRes)
            {
                case ERes.GameObject:
                    LoadGameObjectAsync(path,name,finishCallback, ignoreCache);
                    break;
                default:
                    LoadAssetsAsync(eRes, path, name, finishCallback);
                    break;
            }
        }
        /// <summary>
        /// 加载manifest
        /// </summary>
        public void LoadManifest()
        {
            switch (AppConfig.runMode)
            {
                case ERunMode.Editor:
                    AppConfig.manifest = CreateLoacalManifest();
                    break;
                case ERunMode.Local:
                case ERunMode.Package:
                    AppConfig.manifest = LoadPackageManifest();
                    break;
            }
        }
        #endregion

        #region 私有方法



        /// <summary>
        /// 用于加载asset，监听加载asset完成
        /// </summary>
        /// <param name="ab"></param>
        /// <param name="assetPath"></param>
        /// <param name="resType"></param>
        /// <param name="onAssetComplete"></param>
        private void LoadAssetsWithAB(ERes eRes ,AssetBundle ab, string assetPath,  Action<UnityEngine.Object> onAssetComplete) 
        {
            AssetBundleRequest req = null;
            switch (eRes)
            {
                case ERes.GameObject:
                    req = ab.LoadAssetAsync<GameObject>(assetPath);
                    break;
                case ERes.Sprite:
                    req = ab.LoadAssetAsync<Sprite>(assetPath);
                    break;
                case ERes.Atlas:
                    req = ab.LoadAssetAsync<UnityEngine.U2D.SpriteAtlas>(assetPath);
                    break;
                case ERes.Audio:
                    req = ab.LoadAssetAsync<AudioClip>(assetPath);
                    break;
                case ERes.TextAsset:
                    req = ab.LoadAssetAsync<TextAsset>(assetPath);
                    break;
                default:
                    req = ab.LoadAssetAsync<GameObject>(assetPath);
                    break;
            }
            req.completed += (operation) => {
                var request = (AssetBundleRequest)operation;
                if (request.isDone)
                {
                    var ret = request.asset;
                    onAssetComplete(ret);
                }
            };
        }

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

            FileHelper.PairLuaScript(AppConfig.LUA_PATH, (fullPath, uPath, fileName) => {
                if (!manifest.modules.TryGetValue(AppConfig.LUA_MODULE, out ManifestModule moduleData))
                {
                    moduleData = new ManifestModule(new Dictionary<string, ManifestAB>(), new Dictionary<string, ManifestAsset>());
                    manifest.modules.Add(AppConfig.LUA_MODULE, moduleData);
                }
                //记录lua资源的全路径，后续可通过文件名寻找
                moduleData.AddAsset(fileName, new ManifestAsset("", fullPath));
            });

#endif
            return manifest;
        }

        /// <summary>
        /// 同步加载非GameObject资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private UnityEngine.Object LoadAssets(ERes eRes, string path, string name)
        {
            var manifestAsset = GetManifestAsset(path, name);
            if (AppConfig.runMode == ERunMode.Editor)
            {
#if UNITY_EDITOR
                //编辑器模式下生成
                UnityEngine.Object ret = null;
                switch (eRes)
                {
                    case ERes.Atlas:
                        ret = UnityEditor.AssetDatabase.LoadAssetAtPath<SpriteAtlas>(manifestAsset.path);
                        break;
                    case ERes.Audio:
                        ret = UnityEditor.AssetDatabase.LoadAssetAtPath<AudioClip>(manifestAsset.path);
                        break;
                    case ERes.Sprite:
                        ret = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(manifestAsset.path);
                        break;
                    case ERes.TextAsset:
                        ret = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(manifestAsset.path);
                        break;
                }
                return ret;
#endif

            }
            else
            {
                var ab = AssetsBundleMgr.Ins.GetBundle(manifestAsset.abName);
                if (ab == null)
                {
                    ab = AssetsBundleMgr.Ins.LoadAssetBundle(manifestAsset.abName);
                }

                UnityEngine.Object ret = null;
                switch (eRes)
                {
                    case ERes.Atlas:
                        ret = ab.LoadAsset<SpriteAtlas>(manifestAsset.path);
                        break;
                    case ERes.Audio:
                        ret = ab.LoadAsset<AudioClip>(manifestAsset.path);
                        break;
                    case ERes.Sprite:
                        ret = ab.LoadAsset<Sprite>(manifestAsset.path);
                        break;
                    case ERes.TextAsset:
                        ret = ab.LoadAsset<TextAsset>(manifestAsset.path);
                        break;
                }
         
                return ret;
            }
            return null;

        }


        /// <summary>
        /// 异步加载非GameObject资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private void LoadAssetsAsync(ERes eRes,string path, string name, Action<UnityEngine.Object> finishCallback = null)
        {
            var manifestAsset = GetManifestAsset(path, name);
            if (AppConfig.runMode == ERunMode.Editor)
            {
#if UNITY_EDITOR
                //编辑器模式下生成,直接调用同步方法生成
                var sprite = LoadAssets(eRes, path, name);
                finishCallback?.Invoke(sprite);
                return;
#endif
            }
            else
            {
                //加载AB完成后的回调
                var callBack = new Action<string, AssetBundle>((abName, abData) =>
                {
                    LoadAssetsWithAB(eRes,abData, manifestAsset.path, new Action<UnityEngine.Object>((asset) =>
                    {
                        finishCallback?.Invoke(asset);
                    }));
                });

                var ab = AssetsBundleMgr.Ins.GetBundle(manifestAsset.abName);
                if (ab == null)
                {
                    //AB包未加载完成，先加载AB包
                    AssetsBundleMgr.Ins.LoadAssetBundle(manifestAsset.abName, callBack);
                }
                else
                {
                    //AB包已经加载完成，直接加载asset
                    callBack(manifestAsset.abName, ab);
                }
            }
        }

        /// <summary>
        /// 加载gameobject
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private UnityEngine.Object LoadGameObject(string path,string name, bool ignoreCache = false)
        {
            UnityEngine.GameObject go = null;
            if (!ignoreCache)
            {
                go = GoPoolManager.Ins.CreateGoFromCache(path, name);
                if (go != null)
                {
                    return go;
                }
            }

            var manifestAsset =  GetManifestAsset(path, name);
            if (AppConfig.runMode == ERunMode.Editor)
            {
#if UNITY_EDITOR
                //编辑器模式下生成
                var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(manifestAsset.path);
                go = UnityEngine.Object.Instantiate(asset as GameObject);
#endif
            }
            else
            {
                var ab = AssetsBundleMgr.Ins.GetBundle(manifestAsset.abName);
                if (ab == null)
                {
                    ab = AssetsBundleMgr.Ins.LoadAssetBundle(manifestAsset.abName);
                }
                var ttt = ab.LoadAllAssets();

                var asset = ab.LoadAsset(manifestAsset.path);
                go = UnityEngine.Object.Instantiate(asset as GameObject);
            }

            GoPoolManager.Ins.AddTag(go, path, name);
            return go;
        }

        /// <summary>
        /// 异步加载gameobject资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <param name="finishCallback"></param>
        private void LoadGameObjectAsync(string path, string name, Action<UnityEngine.Object> finishCallback = null, bool ignoreCache = false)
        {
            UnityEngine.Object go;
            if (!ignoreCache)
            {
                go = GoPoolManager.Ins.CreateGoFromCache(path, name);
                if (go != null)
                {
                    finishCallback?.Invoke(go);
                    return;
                }
            }
            var manifestAsset = GetManifestAsset(path, name);
            if (AppConfig.runMode == ERunMode.Editor)
            {
#if UNITY_EDITOR
                //编辑器模式下生成
                var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(manifestAsset.path);
                go = UnityEngine.Object.Instantiate(asset as GameObject);
                GoPoolManager.Ins.AddTag(go as GameObject, path, name);
                finishCallback?.Invoke(go);
#endif
            }
            else
            {
                //加载AB完成后的回调
                var callBack = new Action<string, AssetBundle>((abName, abData) =>
                {
                    LoadAssetsWithAB(ERes.GameObject,abData, manifestAsset.path,new Action<UnityEngine.Object>((asset) =>
                    {
                        var ret = UnityEngine.GameObject.Instantiate(asset as GameObject);
                        GoPoolManager.Ins.AddTag(ret, path, name);
                        finishCallback?.Invoke(ret);
                    }));
                });

                var ab = AssetsBundleMgr.Ins.GetBundle(manifestAsset.abName);
                if (ab == null)
                {
                    //AB包未加载完成，先加载AB包
                    AssetsBundleMgr.Ins.LoadAssetBundle(manifestAsset.abName, callBack);
                }
                else
                {
                    //AB包已经加载完成，直接加载asset
                    callBack(manifestAsset.abName, ab);
                }
            }



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
