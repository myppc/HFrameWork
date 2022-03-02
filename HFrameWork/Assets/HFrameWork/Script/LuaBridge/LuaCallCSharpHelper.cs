
using Assets.HFrameWork.Script.Res;
using Assets.HFrameWork.Script.SBP;
using Assets.HFrameWork.Script.Tool;
using HFrameWork;
using HFrameWork.Script.Pool;
using HFrameWork.Script.SceneMgr;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
//using XLua;

public class LuaCallCSharpHelper
{
    #region 公共字段
    #endregion

    #region 私有字段
    #endregion

    #region 网络

    #endregion

    #region 资源
    /// <summary>
    /// 获取主配置
    /// </summary>
    /// <returns></returns>
    public static Manifest GetManifest()
    {
        return AppConfig.manifest;
    }

    /// <summary>
    /// 获得版本字符串
    /// </summary>
    /// <returns></returns>
    public static string GetVersionStr()
    {
        return AppConfig.versionStr;
    }

    /// <summary>
    /// 加载资源
    /// </summary>
    /// <param name="module">模块名</param>
    /// <param name="assetName">资源名</param>
    /// <param name="eRes">资源类型</param>
    /// <returns>加载完毕后的资源</returns>
    public static UnityEngine.Object Load(int eRes,string module, string assetName ) 
    {
        return ResMgr.Ins.Load((ERes)eRes,module, assetName);
    }

    /// <summary>
    /// 异步加载资源
    /// </summary>
    /// <param name="module">模块名</param>
    /// <param name="assetName"></param>
    /// <param name="eRes">资源类型</param>
    /// <param name="callback"></param>
    /// <param name="progressCallback"></param>
    /// <returns>加载句柄</returns>
    public static void LoadAsync(int eRes,string module, string assetName, Action<UnityEngine.Object> callback)
    {
        ResMgr.Ins.LoadAsync((ERes)eRes,module, assetName, callback);
    }

    public static void LoadManifest()
    {
        ResMgr.Ins.LoadManifest();
    }

    public static void ClearPreGoCache()
    {
        ResMgr.Ins.ClearPreGoCache();
    }

    #endregion

    #region AB资源包

    public static AssetBundle GetBundle(string abName)
    {
        return AssetsBundleMgr.Ins.GetBundle(abName);
    }

    public static void SetBundle(string abName, AssetBundle ab)
    {
        AssetsBundleMgr.Ins.SetBundle(abName, ab);
    }

    public static AssetBundle LoadAssetBundle(string abName)
    {
        return AssetsBundleMgr.Ins.LoadAssetBundle(abName);
    }


    public static void LoadAssetBundleAsync(string abName, Action<string, AssetBundle> finishCallback)
    {
        AssetsBundleMgr.Ins.LoadAssetBundle(abName, finishCallback);
    }


    public static ABLoader GetLoader(string abName)
    {
        return AssetsBundleMgr.Ins.GetLoader(abName);
    }

    public static void SetLoader(string abName, ABLoader loader)
    {
        AssetsBundleMgr.Ins.SetLoader(abName, loader);
    }
    #endregion

    #region 场景


    /// <summary>
    /// 卸载场景
    /// </summary>
    /// <param name="name"></param>
    /// <param name="callback"></param>
    /// <param name="progressCallback"></param>
    /// <returns></returns>
    public static void UnloadScene( string sceneName ,Action finish = null, Action<float> progressCallback = null)
    {
        SceneMgr.Ins.UnloadScene(sceneName,finish, progressCallback);
    }

    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="name"></param>
    /// <param name="callback"></param>
    /// <param name="progressCallback"></param>
    /// <returns></returns>
    public static void LoadScene(string sceneName, Action finish = null, Action<float> progressCallback = null)
    {
        SceneMgr.Ins.LoadScene(sceneName, finish, progressCallback);
    }

    #endregion

    #region 公共
    /// <summary>
    /// 获得运行模式
    /// </summary>
    /// <returns></returns>
    public static int GetRunMode()
    {
        return (int)AppConfig.runMode;
    }


    /// <summary>
    /// c#端字符串格式化
    /// </summary>
    /// <param name="str"></param>
    /// <param name="pars"></param>
    /// <returns></returns>
    public static string FormatStr(string str, params string[] pars)
    {
        return string.Format(str, pars);
    }


    /// <summary>
    /// 获得文件的md5值
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static string GetFileMD5(string filePath)
    {
        return FileHelper.GetFileMD5(filePath);
    }

    /// <summary>
    /// 获得文件的crc值
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static string GetFileCRC(string filePath)
    {
        return FileHelper.GetFileCRC(filePath);
    }

    /// <summary>
    /// 确保有目录
    /// </summary>
    public static void MakeSureHasDir(string path)
    {
        FileHelper.MakeSureHasDir(path);
    }

    /// <summary>
    /// 简易数据存储
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    public static void SetSimpleRecord(string name, string value)
    {
        PlayerPrefs.SetString(name, value);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 简易数据提取
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string GetSimpleRecord(string name)
    {
        return PlayerPrefs.GetString(name);
    }

    /// <summary>
    /// 简易数据删除
    /// </summary>
    /// <param name="name"></param>
    public static void RemoveSimpleRecord(string name)
    {
        PlayerPrefs.DeleteKey(name);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 删除所有数据
    /// </summary>
    public static void RemoveAllSimpleRecord()
    {
        PlayerPrefs.DeleteAll();
    }
    #endregion

    #region Pool
    public static void PoolInit()
    {
        GoPoolManager.Ins.Init();
    }

    public static void  RegisterCacheInfo(string path, string name,int poolType = 0, int cacheCount = 10, string sceneName = "DEFAULT")
    {
        GoPoolManager.Ins.RegisterCacheInfo(path,name, (PoolType)poolType, cacheCount, sceneName);

    }

    public static GameObject CreateGoFromCache(string path, string name)
    {
        return GoPoolManager.Ins.CreateGoFromCache(path, name);
    }

    public static void RecoveryGo(GameObject go)
    {
        GoPoolManager.Ins.RecoveryGo(go);
     }

    public static void ClearSceneCache(string sceneName, bool onlyTemp = false)
    {
        GoPoolManager.Ins.ClearSceneCache(sceneName, onlyTemp);
    }

    public static void LoadCacheByScene(string sceneName)
    {
        GoPoolManager.Ins.LoadCacheByScene(sceneName);
    }

    public static void AddTag(GameObject go, string path, string name)
    {
        GoPoolManager.Ins.AddTag(go, path, name);
    }

    #endregion

    #region Mono帮助
    /// <summary>
    /// 注册Update回调方法
    /// </summary>
    /// <param name="callback"></param>
    public static void RegUpdate(Action callback)
    {
        MonoHelper.Ins.RegUpdate(callback);
    }

    /// <summary>
    /// 注销Update回调方法
    /// </summary>
    /// <param name="callback"></param>
    public static void UnRegUpdate(Action callback)
    {
        MonoHelper.Ins.UnRegUpdate(callback);
    }

    /// <summary>
    /// 注销所有Update回调方法
    /// </summary>
    /// <param name="callback"></param>
    public static void ClearUpdate()
    {
        MonoHelper.Ins.ClearUpdate();
    }

    /// <summary>
    /// 注册LateUpdate回调方法
    /// </summary>
    /// <param name="callback"></param>
    public static void RegLateUpdate(Action callback)
    {
        MonoHelper.Ins.RegLateUpdate(callback);
    }

    /// <summary>
    /// 注销LateUpdate回调方法
    /// </summary>
    /// <param name="callback"></param>
    public static void UnRegLateUpdate(Action callback)
    {
        MonoHelper.Ins.UnRegLateUpdate(callback);
    }

    /// <summary>
    /// 注销所有LateUpdate回调方法
    /// </summary>
    /// <param name="callback"></param>
    public static void ClearLateUpdate()
    {
        MonoHelper.Ins.ClearLateUpdate();
    }

    /// <summary>
    /// 注册FixedUpdate回调方法
    /// </summary>
    /// <param name="callback"></param>
    public static void RegFixedUpdate(Action callback)
    {
        MonoHelper.Ins.RegFixedUpdate(callback);
    }

    /// <summary>
    /// 注销FixedUpdate回调方法
    /// </summary>
    /// <param name="callback"></param>
    public static void UnRegFixedUpdate(Action callback)
    {
        MonoHelper.Ins.UnRegFixedUpdate(callback);
    }

    /// <summary>
    /// 注销所有FixedUpdate回调方法
    /// </summary>
    /// <param name="callback"></param>
    public static void ClearFixedUpdate()
    {
        MonoHelper.Ins.ClearFixedUpdate();
    }

    /// <summary>
    /// 执行Invoke
    /// </summary>
    /// <param name="funcName"></param>
    /// <param name="time"></param>
    public static void DoInvoke(string funcName, float time)
    {
        MonoHelper.Ins.DoInvoke(funcName, time);
    }

    /// <summary>
    /// 执行Invoke
    /// </summary>
    /// <param name="funcName"></param>
    /// <param name="time"></param>
    /// <param name="repeatRate"></param>
    public static void DoInvokeRepeating(string funcName, float time, float repeatRate)
    {
        MonoHelper.Ins.DoInvokeRepeating(funcName, time, repeatRate);
    }

    /// <summary>
    /// 执行CancelInvoke
    /// </summary>
    /// <param name="funcName"></param>
    public static void DoCancelInvoke(string funcName = null)
    {
        MonoHelper.Ins.DoCancelInvoke(funcName);
    }

    /// <summary>
    /// 执行协程
    /// </summary>
    /// <param name="checkHandler"></param>
    /// <param name="handler"></param>
    public static void DoLoopCoroutine(Func<bool> checkHandler, Func<bool> handler)
    {
        MonoHelper.Ins.DoLoopCoroutine(checkHandler, handler);
    }
    #endregion

    #region 其他方法
    public static void UnityDestroy(UnityEngine.Object obj)
    {
        if (obj.GetType() == typeof(UnityEngine.GameObject))
        {
            GoPoolManager.Ins.RecoveryGo(obj as GameObject);
        }
        else
        {
            GameObject.Destroy(obj);
        }
    }
    #endregion
}
