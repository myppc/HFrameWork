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
    /// ������ʼ��
    /// </summary>
    /// <returns></returns>
    public bool Init()
    {
        ///��ʼ���ڴ�����
        var succ = true;
        succ = succ&&GoPoolManager.GetIns().Init();
        return succ;
    }

    /// <summary>
    /// �����������
    /// </summary>
    public void StartEntrance()
    {
        LoadMainfest();
    }

    /// <summary>
    /// ����Mainfest
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
