using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ExtensionHelper
{
    #region 公共字段
    #endregion

    #region 私有字段
    #endregion

    #region 公共方法
    /// <summary>
    /// 按钮添加事件
    /// </summary>
    /// <param name="toggle"></param>
    /// <param name="callback"></param>
    /// <param name="isAnim"></param>
    public static void AddEvent(this Button btn, Action callback, bool isAnim = true)
    {
        btn.onClick.RemoveAllListeners();
        if (callback != null)
        {
            // 是否有动画
            ButtonEx ex = null;
            if (isAnim)
            {
                ex = btn.GetComponent<ButtonEx>();
                if (ex == null)
                {
                    ex = btn.gameObject.AddComponent<ButtonEx>();
                }
            }

            // 注册事件
            btn.onClick.AddListener(()=> {
                // 播放动画
                if (ex != null)
                {
                    ex.StartAnim(-1);
                }

                // 回调
                callback.Invoke();
            });
        }
    }

    /// <summary>
    /// 按钮清除事件
    /// </summary>
    /// <param name="btn"></param>
    public static void ClearEvent(this Button btn)
    {
        btn.onClick.RemoveAllListeners();
    }

    /// <summary>
    /// 输入框添加事件
    /// </summary>
    /// <param name="input"></param>
    /// <param name="changedCallback"></param>
    /// <param name="endCallback"></param>
    public static void AddEvent(this InputField input, Action<string> changedCallback, Action<string> endCallback)
    {
        if (changedCallback != null)
        {
            input.onValueChanged.RemoveAllListeners();
            input.onValueChanged.AddListener((v) => {
                changedCallback.Invoke(v);
            });
        }
        if (endCallback != null)
        {
            input.onEndEdit.RemoveAllListeners();
            input.onEndEdit.AddListener((v) => {
                endCallback.Invoke(v);
            });
        }
    }

    /// <summary>
    /// 输入框清除事件
    /// </summary>
    /// <param name="input"></param>
    public static void ClearEvent(this InputField input)
    {
        input.onValueChanged.RemoveAllListeners();
        input.onEndEdit.RemoveAllListeners();
    }

    /// <summary>
    /// 开关添加事件
    /// </summary>
    /// <param name="toggle"></param>
    /// <param name="callback"></param>
    public static void AddEvent(this Toggle toggle, Action<bool> callback)
    {
        toggle.onValueChanged.RemoveAllListeners();
        if (callback != null)
        {
            toggle.onValueChanged.AddListener((isOn) => {
                callback.Invoke(isOn);
            });
        }
    }

    /// <summary>
    /// 开关清除事件
    /// </summary>
    /// <param name="toggle"></param>
    public static void ClearEvent(this Toggle toggle)
    {
        toggle.onValueChanged.RemoveAllListeners();
    }

    /// <summary>
    /// 滑动条添加事件
    /// </summary>
    /// <param name="slider"></param>
    /// <param name="callback"></param>
    public static void AddEvent(this Slider slider, Action<float> callback)
    {
        slider.onValueChanged.RemoveAllListeners();
        if (callback != null)
        {
            slider.onValueChanged.AddListener((v) => {
                callback.Invoke(v);
            });
        }
    }

    /// <summary>
    /// 滑动条清除事件
    /// </summary>
    /// <param name="scroll"></param>
    public static void ClearEvent(this Slider slider)
    {
        slider.onValueChanged.RemoveAllListeners();
    }

    /// <summary>
    /// 滚动栏添加事件
    /// </summary>
    /// <param name="scroll"></param>
    /// <param name="callback"></param>
    public static void AddEvent(this ScrollRect scroll, Action<Vector2> callback)
    {
        scroll.onValueChanged.RemoveAllListeners();
        if (callback != null)
        {
            scroll.onValueChanged.AddListener((v) => {
                callback.Invoke(v);
            });
        }
    }

    /// <summary>
    /// 滚动栏清除事件
    /// </summary>
    /// <param name="scroll"></param>
    public static void ClearEvent(this ScrollRect scroll)
    {
        scroll.onValueChanged.RemoveAllListeners();
    }

    /// <summary>
    /// 播放动画并添加回调
    /// </summary>
    /// <param name="anim"></param>
    /// <param name="clipName"></param>
    /// <param name="normalizedTime"></param>
    /// <param name="callback"></param>
    /// <param name="layer"></param>
    public static void PlayWithCallback(this Animator anim, string clipName, Action callback, float normalizedTime = 0, int layer = 0)
    {
        // 播放动画
        if (!string.IsNullOrEmpty(clipName))
        {
            anim.Play(clipName, layer, normalizedTime);
        }

        // 添加播放完毕回调
        AddCallback(anim, callback);
    }

    /// <summary>
    /// 添加回调
    /// </summary>
    /// <param name="anim"></param>
    /// <param name="callback"></param>
    /// <param name="regNormalizedTime"></param>
    /// <param name="layer"></param>
    public static void AddCallback(this Animator anim, Action callback, float regNormalizedTime = 1, int layer = 0)
    {
        AnimatorEx ex = null;
        if (callback != null)
        {
            // 添加组件
            ex = anim.GetComponent<AnimatorEx>();
            if (ex == null)
            {
                ex = anim.gameObject.AddComponent<AnimatorEx>();
            }

            // 注册事件
            ex.AddEvent(callback, regNormalizedTime, layer);
        }
    }
    #endregion

    #region 其他方法
    #endregion
}
