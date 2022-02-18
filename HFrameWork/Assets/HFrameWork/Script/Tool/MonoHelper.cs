using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoHelper : SingletonScriptCreated<MonoHelper>
{
    #region 公共字段
    #endregion

    #region 私有字段
    Action _onUpdate;
    Action _onLateUpdate;
    Action _onFixedUpdate;
    #endregion

    #region 默认回调
    /// <summary>
    /// 每帧回调
    /// </summary>
    void Update()
    {
        _onUpdate?.Invoke();
    }

    /// <summary>
    /// 每帧后回调
    /// </summary>
    void LateUpdate()
    {
        _onLateUpdate?.Invoke();
    }

    /// <summary>
    /// 固定帧率回调
    /// </summary>
    void FixedUpdate()
    {
        _onFixedUpdate?.Invoke();
    }
    #endregion

    #region 公共方法
    /// <summary>
    /// 注册更新回调
    /// </summary>
    /// <param name="callback"></param>
    public void RegUpdate(Action callback)
    {
        _onUpdate += callback;
    }

    /// <summary>
    /// 注销更新回调
    /// </summary>
    /// <param name="callback"></param>
    public void UnRegUpdate(Action callback)
    {
        _onUpdate -= callback;
    }

    /// <summary>
    /// 清除更新回调
    /// </summary>
    public void ClearUpdate()
    {
        _onUpdate = null;
    }

    /// <summary>
    /// 注册更新后回调
    /// </summary>
    /// <param name="callback"></param>
    public void RegLateUpdate(Action callback)
    {
        _onLateUpdate += callback;
    }

    /// <summary>
    /// 注销更新后回调
    /// </summary>
    /// <param name="callback"></param>
    public void UnRegLateUpdate(Action callback)
    {
        _onLateUpdate -= callback;
    }

    /// <summary>
    /// 清除更新后回调
    /// </summary>
    public void ClearLateUpdate()
    {
        _onLateUpdate = null;
    }

    /// <summary>
    /// 注册固定帧率更新回调
    /// </summary>
    /// <param name="callback"></param>
    public void RegFixedUpdate(Action callback)
    {
        _onFixedUpdate += callback;
    }

    /// <summary>
    /// 注销固定帧率更新回调
    /// </summary>
    /// <param name="callback"></param>
    public void UnRegFixedUpdate(Action callback)
    {
        _onFixedUpdate -= callback;
    }

    /// <summary>
    /// 清除固定帧率更新回调
    /// </summary>
    public void ClearFixedUpdate()
    {
        _onFixedUpdate = null;
    }

    /// <summary>
    /// 执行Invoke
    /// </summary>
    /// <param name="funcName"></param>
    /// <param name="time"></param>
    public void DoInvoke(string funcName, float time)
    {
        Invoke(funcName, time);
    }

    /// <summary>
    /// 执行Invoke
    /// </summary>
    /// <param name="funcName"></param>
    /// <param name="time"></param>
    /// <param name="repeatRate"></param>
    public void DoInvokeRepeating(string funcName, float time, float repeatRate)
    {
        InvokeRepeating(funcName, time, repeatRate);
    }

    /// <summary>
    /// 执行CancelInvoke
    /// </summary>
    /// <param name="funcName"></param>
    public void DoCancelInvoke(string funcName = null)
    {
        CancelInvoke(funcName);
    }

    /// <summary>
    /// 执行循环方法的协程
    /// </summary>
    /// <param name="checkHandler"></param>
    /// <param name="handler"></param>
    public void DoLoopCoroutine(Func<bool> checkHandler, Func<bool> handler)
    {
        StartCoroutine(Coroutine(checkHandler, handler));
    }

    /// <summary>
    /// 打日志
    /// </summary>
    /// <param name="msg"></param>
    public void Log(string msg)
    {
        Debug.Log(msg);
    }
    #endregion

    #region 其他方法
    /// <summary>
    /// 协程
    /// </summary>
    /// <param name="checkHandler"></param>
    /// <param name="handler"></param>
    /// <returns></returns>
    IEnumerator Coroutine(Func<bool> checkHandler, Func<bool> handler)
    {
        while (checkHandler.Invoke())
        {
            if (handler.Invoke())
                yield return 0;
        }
    }
    #endregion
}
