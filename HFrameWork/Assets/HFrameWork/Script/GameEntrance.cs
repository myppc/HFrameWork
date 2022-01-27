using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HFrameWork;
using Assets.HFrameWork.Script.Tool;
using Assets.HFrameWork.Script.Res;
using System.IO;

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
        GoPoolManager.GetIns().Init();
    }

    /// <summary>
    /// �����������
    /// </summary>
    public void StartEntrance()
    {
        
        ////var go = ResMgr.Ins.Load<GameObject>(ERes.GameObject, "mode1", "Cube");
        ////go.transform.SetParent(this.transform,false);

        //var ab = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "AssetBundle/standalone/mode1"));
        //var go1 = ab.LoadAsset<GameObject>("Assets/Modules/mode1/Prefabs/Cube.prefab");
        //GameObject.Instantiate<GameObject>(go1);
    }
}
