local tick_mgr = {}

--初始化
function tick_mgr:init()
    gCSharp.InitTickMgr()
end

--开始计时
function tick_mgr:start_tick()
    gCSharp.StartTick()
    if self._on_frame then
        gMgrs.unityUpdate:unreg_fixedupdate(self._on_frame)
        self._on_frame = nil
    end
    self._on_frame = function()
        self:on_frame_update_tick()
    end
    gMgrs.unityUpdate:reg_fixedupdate(self._on_frame)
end

--添加计时回调
function tick_mgr:add_tick(tick_param)
    return gCSharp.AddTick(tick_param)
end

---通过唯一id移除计时器
---@param uid integer 唯一ID
function tick_mgr:cancel_by_uid(uid)
    gCSharp.CancelByUid(uid)
end

---通过tag移除对应的所有计时器
---@param tag any
function tick_mgr:cancel_by_tag(tag)
    gCSharp.CancelTickersByTag(tag)
end

---设置暂停状态
---@param is_pause any
function tick_mgr:set_pause(is_pause)
    gCSharp.SetPause(is_pause)
end

---每帧驱动函数
function tick_mgr:on_frame_update_tick()
    gCSharp.OnFrameUpdateTick()
end

---清理计时器
function tick_mgr:clear()
    if self._on_frame then
        gMgrs.unityUpdate:unreg_fixedupdate(self._on_frame)
    end
    gCSharp.Destroy()
end

---comment 生成注册参数 设置 OnCancelCallBack 在取消计时器时回调， 设置 OnTickCallBack在到达每个计时单位时回调
---@return table {LoopCount:integer 调用次数,默认0，表示一直调用 ,TickDelay:integer 计时帧数,默认1帧,OnTickCallBack:function,OnCancelCallBack:function }
function tick_mgr:get_ticker_param()
    return gCSharp.GetTickerParam()
end

---获取当前帧数
function tick_mgr:get_frame()
    return gCSharp.GetFrame()
end

---计时器进行计算帧数进行回调
---@param loop integer  调用次数,默认0,表示一直调用
---@param delay integer 计时间隔帧数,默认为1次调用
---@param callback function 回调方法
---@param on_cancel function 取消时回调
---@param tag any 标记
function tick_mgr:tick_frame(loop,delay,callback,on_cancel,tag)
    local param = self:get_ticker_param()
    param.LoopCount = loop or param.LoopCount
    param.TickDelay = delay or param.TickDelay
    param.OnTickCallBack = callback
    param.OnCancelCallBack = on_cancel
    param.Tag = tag or param.Tag
    return self:add_tick(param)
end

---延迟一帧调用
---@param callback function 回调方法
function tick_mgr:next_frame(callback)
    local param = self:get_ticker_param()
    param.LoopCount = 1
    param.OnTickCallBack = callback
    return self:add_tick(param)
end


return tick_mgr