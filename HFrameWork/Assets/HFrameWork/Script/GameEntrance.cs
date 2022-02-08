using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HFrameWork;
using Assets.HFrameWork.Script.Tool;
using Assets.HFrameWork.Script.Res;
using System.IO;
using UnityEngine.U2D;
using System;

public class GameEntrance : MonoBehaviour
{
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
    }
}
