using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEx : MonoBehaviour
{
    #region 公共字段
    /// <summary>
    /// 回调
    /// </summary>
    public Action callback;
    #endregion

    #region 私有字段
    /// <summary>
    /// 动画组件
    /// </summary>
    Animator _anim;

    /// <summary>
    /// 回调字典
    /// </summary>
    Dictionary<int, Bundle> _dicCallback = new Dictionary<int, Bundle>();

    /// <summary>
    /// 计次
    /// </summary>
    int _count;
    #endregion

    #region 默认回调
    private void Awake()
    {
        // 获得组件
        _anim = GetComponent<Animator>();
    }

    /// <summary>
    /// 开始后回调
    /// </summary>
    void Start()
    {
        
    }
    #endregion

    #region 公共方法
    /// <summary>
    /// 添加事件
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="time"></param>
    /// <param name="layer"></param>
    public void AddEvent(Action callback, float regNormalizedTime = -1, int layer = 0)
    {
        if (callback == null)
        {
            return;
        }

        AnimatorClipInfo[] clipInfos = _anim.GetCurrentAnimatorClipInfo(layer);
        if (clipInfos.Length > 0)
        {
            var clip = clipInfos[0].clip;
            _count++;

            // 拼接事件
            AnimationEvent animEvent = new AnimationEvent();
            animEvent.functionName = "OnEvent";
            animEvent.intParameter = _count;
            if (regNormalizedTime < 0)
            {
                animEvent.time = clip.length;
            }
            else
            {
                animEvent.time = regNormalizedTime * clip.length;
            }

            clip.AddEvent(animEvent);

            // 记录
            _dicCallback.Add(_count, new Bundle() {
                callback = callback,
                layer = layer,
                clip = clip
            });
        }
    }
    #endregion

    #region 回调方法
    /// <summary>
    /// 事件回调
    /// </summary>
    /// <param name="key"></param>
    public void OnEvent(int key)
    {
        // 回调
        if (_dicCallback.TryGetValue(key, out Bundle bundle))
        {
            // 回调
            bundle.callback?.Invoke();

            // 移除事件
            RemoveEvent(key, bundle.clip, bundle.layer);

            // 移除数据
            _dicCallback.Remove(key);
        }
    }
    #endregion

    #region 其他方法
    /// <summary>
    /// 移除事件
    /// </summary>
    /// <param name="key"></param>
    /// <param name="clip"></param>
    /// <param name="layer"></param>
    void RemoveEvent(int key, AnimationClip clip, int layer = 0)
    {
        // 获得所有事件
        var events = new List<AnimationEvent>(clip.events);
        for (int i = 0; i < events.Count; i++)
        {
            var e = events[i];

            // 是否该键
            if (e.intParameter == key)
            {
                // 移除
                events.RemoveAt(i);
                break;
            }
        }
        clip.events = events.ToArray();
    }
    #endregion

    #region 内部类
    class Bundle
    {
        public Action callback;
        public int layer;
        public AnimationClip clip;
    }
    #endregion
}
