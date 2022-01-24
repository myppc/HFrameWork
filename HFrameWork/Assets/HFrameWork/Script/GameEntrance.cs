using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HFrameWork;
using Assets.HFrameWork.Script.Tool;
using Assets.HFrameWork.Script.Res;

public class GameEntrance : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var succ = Init();
        if (succ)
        {
            StartEntrance();
        }
    }

    /// <summary>
    /// 基础初始化
    /// </summary>
    /// <returns></returns>
    public bool Init()
    {
        ///初始化内存管理池
        var succ = true;
        succ = succ&&GoPoolManager.GetIns().Init();
        return succ;
    }

    /// <summary>
    /// 进入入口流程
    /// </summary>
    public void StartEntrance()
    {
        LoadMainfest();
    }

    /// <summary>
    /// 加载Mainfest
    /// </summary>
    public void LoadMainfest()
    {
        if (AppConfig.runMode == ERunMode.Editor)
        {
            AppConfig.manifest = ResMgr.Ins.CreateLoacalManifest();
            return;
        }
    }
}
