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
    }
}
