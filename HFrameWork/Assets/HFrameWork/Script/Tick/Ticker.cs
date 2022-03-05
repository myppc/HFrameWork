using UnityEngine;
using UnityEditor;

public class Ticker 
{
    private TickerParam _param;
    private int _callCount = 0;
    private bool _isDestroy;
    public Ticker(TickerParam param)
    {
        _param = param;
    }

    public int GetTickerUid()
    {
        return _param.uid;
    }

    public object GetTickerTag()
    {
        return _param.Tag;
    }

    public void OnFrameTick(int curFrame)
    {
        var dis = curFrame - _param.StartFrame;
        if(dis >0 && dis% _param.TickDelay == 0)
        {
            _callCount += 1;
            _param.OnTickCallBack(curFrame);
            if(_param.LoopCount != 0 && _callCount >= _param.LoopCount)
            {
                _isDestroy = true;
            }
        }
    }

    public bool IsDestroy()
    {
        return _isDestroy;
    }

    public void Destroy()
    {
        _param.OnCancelCallBack?.Invoke();
    }
}