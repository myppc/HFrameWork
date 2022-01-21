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
    /// 基础初始化
    /// </summary>
    /// <returns></returns>
    public bool Init()
    {
        ///初始化内存管理池
        GoPoolManager.GetIns().Init();
        return true;
    }
}
