using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class TickManager 
{
    private static TickManager _Instance;

    private int _uid;
    private int _frame;
    private bool _isStart;
    private bool _isPause;
    private Dictionary<int, Ticker> _tickerDict;
    private Dictionary<object, List<int>> _tagDict;
    private List<int> _uidList;
    private List<int> _dropList;
    private List<Ticker> _addList;
    private bool _isDirty = false;
    

    public static TickManager GetInstance()
    {
        if (_Instance == null)
        {
            _Instance = new TickManager();
        }
        return _Instance;
    }

    #region 私有方法
    private void ClearAddList()
    {
        foreach (var ticker in _addList)
        {
            var uid = ticker.GetTickerUid();
            _uidList.Add(uid);
            _tickerDict.Add(uid, ticker);
            RecordTickToTag(ticker);
        }
        _addList.Clear();
    }

    /// <summary>
    /// 记录tag
    /// </summary>
    /// <param name="ticker"></param>
    private void RecordTickToTag(Ticker ticker)
    {
        var tag = ticker.GetTickerTag();
        if (!_tagDict.ContainsKey(tag))
        {
            _tagDict.Add(tag, new List<int>());
        }
        var list = _tagDict[tag];
        list.Add(ticker.GetTickerUid());
    }

    /// <summary>
    /// 删除tag的记录
    /// </summary>
    /// <param name="ticker"></param>
    private void RemoveTagRecord(Ticker ticker)
    {
        var tag = ticker.GetTickerTag();
        if (_tagDict.ContainsKey(tag))
        {
            var list = _tagDict[tag];
            list.Remove(ticker.GetTickerUid());
        }
    }

    /// <summary>
    /// 异步清理丢弃的计时器
    /// </summary>
    private void ClearDropList()
    {
        if (!_isDirty)
        {
            return;
        }
        foreach (var uid in _dropList)
        {
            var ticker = _tickerDict[uid];
            _uidList.Remove(uid);
            _tickerDict.Remove(uid);
            RemoveTagRecord(ticker);
            ticker.Destroy();
        }
        _dropList.Clear();
        _isDirty = false;
    }

    #endregion

    /// <summary>
    /// 初始化计时器
    /// </summary>
    public void Init()
    {
        _uid = 0;
        _frame = 0;
        _isStart = false;
        _isPause = false;
        _tickerDict = new Dictionary<int, Ticker>();
        _tagDict = new Dictionary<object, List<int>>();
        _uidList = new List<int>();
        _dropList = new List<int>();
        _addList = new List<Ticker>();
    }

    /// <summary>
    /// 开始计时
    /// </summary>
    public void StartTick()
    {
        _isStart = true;
    }

    /// <summary>
    /// 添加一个计时器
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public int AddTick(TickerParam param)
    {
        _uid += 1;
        param.uid = _uid;
        param.StartFrame = _frame;
        var ticker = new Ticker(param);
        _addList.Add(ticker);
        return _uid;
    }

    /// <summary>
    /// 通过唯一id取消计时器
    /// </summary>
    /// <param name="uid"></param>
    public void CancelByUid(int uid)
    {
        if (_dropList.IndexOf(uid) != -1)
        {
            return;
        }
        if (_tickerDict.ContainsKey(uid) && _uidList.IndexOf(uid) != -1)
        {
            _dropList.Add(uid);
            _isDirty = true;
        }
    }

    /// <summary>
    /// 通过tag取消该tag下所有计时器
    /// </summary>
    /// <param name="tag"></param>
    public void CancelTickersByTag(object tag)
    {
        if (tag.GetType() == typeof(string) && (string)tag == "DEFAULT")
        {
            Debug.LogWarning("Can not cancel tag : DEFAULT");
            return;
        }
        if (!_tagDict.ContainsKey(tag))
        {
            return;
        }

        var list = _tagDict[tag];
        foreach (var id in list)
        {
            CancelByUid(id);
        }
        list.Clear();
    }


    /// <summary>
    /// 暂停计时器
    /// </summary>
    /// <param name="isPause"></param>
    public void SetPause(bool isPause)
    {
        _isPause = isPause;
    }

    /// <summary>
    /// frameupdate回调
    /// </summary>
    public void OnFrameUpdateTick()
    {
        if(!_isStart || _isPause)
        {
            return;
        }
        ClearDropList();
        _frame += 1;
        foreach(var uid in _uidList)
        {
            var ticker = _tickerDict[uid];
            ticker.OnFrameTick(_frame);
            if (ticker.IsDestroy())
            {
                CancelByUid(uid);
            }
        }
        ClearAddList();
    }

    /// <summary>
    /// 销毁时调用
    /// </summary>
    public void Destroy()
    {
        _tickerDict.Clear();
        _uidList.Clear();
        _dropList.Clear();
    }

    /// <summary>
    /// 生成一个tickerparam参数
    /// </summary>
    /// <returns></returns>
    public static TickerParam GetTickerParam()
    {
        TickerParam param;
        param.LoopCount = 0;
        param.uid = 0;
        param.StartFrame = 0;
        param.OnCancelCallBack = null;
        param.OnTickCallBack = null;
        param.TickDelay = 1;
        param.Tag = "DEFAULT";
        return param;
    }
    
    /// <summary>
    /// 获取当前帧数
    /// </summary>
    /// <returns></returns>
    public int GetFrame()
    {
        return _frame;
    }
}