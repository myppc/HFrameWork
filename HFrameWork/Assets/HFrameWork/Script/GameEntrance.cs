using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HFrameWork;
using Assets.HFrameWork.Script.Tool;
using Assets.HFrameWork.Script.Res;
using System.IO;
using UnityEngine.U2D;
using System;
using XLua;
using HFrameWork.Script.Pool;

public class GameEntrance : MonoBehaviour
{
    public LuaEnv env;
    // Start is called before the first frame update
    void Start()
    {
        Init();
        StartEntrance();
    }

    /// <summary>
    /// 基础初始化
    /// </summary>
    /// <returns></returns>
    public void Init()
    {
        ///加载主配置
        ResMgr.Ins.LoadManifest();

        ///初始化内存管理池
        GoPoolManager.Ins.Init();
    }

    /// <summary>
    /// 进入入口流程
    /// </summary>
    public void StartEntrance()
    {

        // 初始化Lua虚拟机
        env = new LuaEnv();
        // 导入RapidJson
        env.AddBuildin("rapidjson", XLua.LuaDLL.Lua.LoadRapidJson);

        // 自定义加载路径
        env.AddLoader(LuaABMgr.Ins.LuaLoader);
        //编辑器模式下直接运行
        if (AppConfig.runMode == ERunMode.Editor)
        {
            CallLuaMain("run");
        }
        else
        {
            //包模式下需要加载luaAB
            AssetsBundleMgr.Ins.LoadAssetBundle(AppConfig.LUA_AB_NAME, (name, ab) =>
            {
                CallLuaMain("run");
            });
        }

    }

    /// <summary>
    /// 启动lua
    /// </summary>
    /// <param name="func"></param>
    public void CallLuaMain(string func)
    {
        env?.DoString(string.Format("local m = require('{0}') m.{1}()", AppConfig.LUA_MAIN, func));
    }
}
