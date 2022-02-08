using Assets.HFrameWork.Script.SBP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public enum ERunMode
{
    /// <summary>
    /// 编辑器模式
    /// 直接从资源目录取资源
    /// </summary>
    Editor,

    /// <summary>
    /// 本地模式
    /// 使用本地打的ab包
    /// </summary>
    Local,

    /// <summary>
    /// 包模式
    /// 从服务器获取ab包再缓存后加载
    /// </summary>
    Package,
}

public enum EABMode
{
    /// <summary>
    /// 加载AB使用LoadFromFile 
    /// </summary>
    FromFile,

    /// <summary>
    /// 加载AB使用LoadFromStream
    /// </summary>
    FromStream,

    /// <summary>
    /// 加载AB使用LoadFromMemory
    /// </summary>
    FromMemory,
}

public static class AppConfig
{
    #region 常量


    /// <summary>
    /// 运行模式
    /// </summary>
    public static ERunMode runMode = ERunMode.Package;

    /// <summary>
    /// 加载AB包方式
    /// </summary>
    public static EABMode loadMode = EABMode.FromFile;

    /// <summary>
    /// 资源根路径
    /// </summary>
    public static readonly string ASSETS_PATH = Path.Combine(Application.dataPath, "Modules");

    /// <summary>
    /// 主配置文件名
    /// </summary>
    public static readonly string MANIFEST_NAME = "manifest.json";

    /// <summary>
    /// Assets加载路径
    /// </summary>
    public static readonly string AB_BUILD_PATH = Path.Combine(Application.streamingAssetsPath, "AssetBundle", GetPlatformString());

    /// <summary>
    /// 主配置文件加载路径
    /// </summary>
    public static string MANIFEST_LOAD_PATH
    {
        get
        {
            // 是否是包模式
            if (AppConfig.runMode == ERunMode.Package)
            {
                return Path.Combine(Application.persistentDataPath, GetPlatformString(), MANIFEST_NAME);
            }
            // 是否是本地模式
            else if (AppConfig.runMode == ERunMode.Local)
            {
                return Path.Combine(AB_BUILD_PATH, MANIFEST_NAME);
            }

            Debug.LogError("禁止在非本地和非包模式下读取manifest文件.");
            return "";
        }
    }


    /// <summary>
    /// AB加载路径
    /// </summary>
    public static string AB_LOAD_PATH
    {
        get
        {
            // 本地模式
            if (runMode == ERunMode.Local)
            {
                return AB_BUILD_PATH;
            }
            // 包模式
            else if (runMode == ERunMode.Package)
            {
                return Path.Combine(Application.persistentDataPath, GetPlatformString());
            }

            Debug.LogError("禁止在非本地和非包模式下加载ab.");
            return "";
        }
    }

    #endregion

    #region 变量
    public static Manifest manifest;
    #endregion


    #region 公用方法
    /// <summary>
    /// 获取平台string
    /// </summary>
    /// <returns></returns>
    public static string GetPlatformString()
    {
#if UNITY_IOS
        return "iOS";
#elif UNITY_ANDROID
        return "Android";
#elif UNITY_STANDALONE_WIN
        return "StandaloneWindows64";
#endif
    }



    #endregion
}
