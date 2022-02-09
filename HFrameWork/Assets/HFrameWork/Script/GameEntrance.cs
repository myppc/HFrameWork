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
    /// ������ʼ��
    /// </summary>
    /// <returns></returns>
    public void Init()
    {
        ///����������
        ResMgr.Ins.LoadManifest();

        ///��ʼ���ڴ�����
        GoPoolManager.Ins.Init();
    }

    /// <summary>
    /// �����������
    /// </summary>
    public void StartEntrance()
    {
        // ��ʼ��Lua�����
        env = new LuaEnv();
        // ����RapidJson
        env.AddBuildin("rapidjson", XLua.LuaDLL.Lua.LoadRapidJson);

        // �Զ������·��
        env.AddLoader(LuaABMgr.Ins.LuaLoader);

        env?.DoString(string.Format("local m = require('{0}') m.{1}()", AppConfig.LUA_MAIN_PATH, "run"));
    }
}
