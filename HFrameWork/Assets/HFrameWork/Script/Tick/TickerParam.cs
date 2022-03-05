using System;

public struct TickerParam
{
    ///调用次数，默认0，表示一直调用
    public int LoopCount;
    ///ticker 唯一id
    public int uid;
    public Action<int> OnTickCallBack;
    public Action OnCancelCallBack;
    ///计时帧数
    public int TickDelay;
    public int StartFrame;
    public object Tag;
}

