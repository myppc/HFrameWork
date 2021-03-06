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
    public static ERunMode runMode = ERunMode.Editor;

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
    /// lua 下标文件路径
    /// </summary>
    public static readonly string LUA_GO_INDEX = Path.Combine(Application.dataPath, "LuaScripts/name_indexs");

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

    /// <summary>
    /// lua代码根路径
    /// </summary>
    public static readonly string LUA_PATH = Path.Combine(Application.dataPath, "LuaScripts");

    /// <summary>
    /// Lua主文件路径
    /// </summary>
    public static readonly string LUA_MAIN = "main_entrance.lua";

    /// <summary>
    /// Lua AB模块名
    /// </summary>
    public static readonly string LUA_MODULE = "lua";

    /// <summary>
    /// Lua AB包 资源名
    /// </summary>
    public static readonly string LUA_RES = "script";

    /// <summary>
    /// Lua AB包名字
    /// </summary>
    public static readonly string LUA_AB_NAME = $"{LUA_MODULE}_{LUA_RES}";


    /// <summary>
    /// lua文件新后缀
    /// </summary>
    public static readonly string LUA_NEW_EXTENSION = ".txt";

    /// <summary>
    /// lua代码预处理路径
    /// </summary>
    //public static readonly string LUA_PRE_PATH = Path.Combine(AB_PRE_OUT_PATH, "luas");
    public static readonly string LUA_PRE_PATH = Path.Combine(Application.dataPath, "Resources", "luas");

    /// <summary>
    /// lua代码预处理备份路径
    /// </summary>
    public static readonly string LUA_PRE_BACKUP_PATH = Application.dataPath.Replace("Assets", "luas");

    //版本号
    public static string versionStr;

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
