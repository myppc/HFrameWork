using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var succ = Init();
    }

    /// <summary>
    /// ������ʼ��
    /// </summary>
    /// <returns></returns>
    public bool Init()
    {
        ///��ʼ���ڴ�����
        GoPoolManager.GetIns().Init();
        return true;
    }
}
