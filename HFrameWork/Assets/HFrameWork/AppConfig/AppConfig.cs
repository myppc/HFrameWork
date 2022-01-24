using Assets.HFrameWork.Script.SBP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

public static class AppConfig
{
    #region 常量
    public static ERunMode runMode = ERunMode.Editor;
    /// <summary>
    /// 资源根路径
    /// </summary>
    public static readonly string ASSETS_PATH = Path.Combine(Application.dataPath, "Modules");
    #endregion

    #region 变量
    public static Manifest manifest;
    #endregion

}
