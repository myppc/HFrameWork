using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonEx : MonoBehaviour
{
    #region 常量
    /// <summary>
    /// 缩放比例
    /// </summary>
    static readonly float ZOOM_RATE = 0.9f;

    /// <summary>
    /// 缩放时间
    /// </summary>
    static readonly float ZOOM_TIME = 0.05f;
    #endregion

    #region 公共字段
    #endregion

    #region 私有字段
    /// <summary>
    /// 按钮
    /// </summary>
    Button _btn;

    /// <summary>
    /// 矩形变换组件
    /// </summary>
    RectTransform _rectTrans;

    /// <summary>
    /// 尺寸
    /// </summary>
    Vector3 _scale;

    /// <summary>
    /// 缩放尺寸
    /// </summary>
    Vector3 _zoomScale;

    /// <summary>
    /// 每帧改变值
    /// </summary>
    Vector3 _delta;

    /// <summary>
    /// 方向
    /// </summary>
    int _dir;
    #endregion

    #region 默认回调
    /// <summary>
    /// 唤醒后回调
    /// </summary>
    private void Awake()
    {
        // 获得组件
        _btn = GetComponent<Button>();
        _rectTrans = GetComponent<RectTransform>();

        // 记录尺寸
        _scale = _rectTrans.localScale;
        _zoomScale.x = _scale.x * ZOOM_RATE;
        _zoomScale.y = _scale.y * ZOOM_RATE;
        _zoomScale.z = _scale.z;

        // 计算每帧改变值
        _delta.x = (Time.fixedDeltaTime / ZOOM_TIME) * (_scale.x - _zoomScale.x);
        _delta.y = (Time.fixedDeltaTime / ZOOM_TIME) * (_scale.y - _zoomScale.y);
    }

    /// <summary>
    /// 开始后回调
    /// </summary>
    void Start()
    {
        
    }

    /// <summary>
    /// 固定帧率回调
    /// </summary>
    void FixedUpdate()
    {
        if (_dir == 0)
        {
            return;
        }

        // 缩放
        _rectTrans.localScale = new Vector3(
            Mathf.Max(_zoomScale.x, _rectTrans.localScale.x + Mathf.Sign(_dir) * _delta.x),
            Mathf.Max(_zoomScale.y, _rectTrans.localScale.y + Mathf.Sign(_dir) * _delta.y),
            _rectTrans.localScale.z
        );

        if (_dir < 0)   // 是否想缩小
        {
            // 是否已经足够小了
            if (_rectTrans.localScale.x <= _zoomScale.x)
            {
                // 设置缩放
                _rectTrans.localScale = _zoomScale;

                // 反向
                _dir = -_dir;
            }
        }
        else    // 放大
        {
            // 是否已经足够大了
            if (_rectTrans.localScale.x >= _scale.x)
            {
                // 设置缩放
                _rectTrans.localScale = _scale;

                // 停止
                _dir = 0;
            }
        }
    }
    #endregion

    #region 公共方法
    /// <summary>
    /// 开始动画
    /// </summary>
    /// <param name="dir">方向 -1按下 1弹起</param>
    public void StartAnim(int dir)
    {
        if (dir == 0)
        {
            return;
        }

        // 设置方向
        _dir = dir;
    }
    #endregion

    #region 其他方法
    #endregion
}
