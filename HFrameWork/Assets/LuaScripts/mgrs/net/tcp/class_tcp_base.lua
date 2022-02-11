---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by LeeroyLin.
--- DateTime: 2021/9/8 14:50
---

local clsTcpBase = gClass.declare("clsPtcBase")

--- ==================== 构造方法 ====================

--- 构造方法
---@param address: 地址
---@param port: 端口
---@param isEncrypt: 是否加密
---@param isConnect: 是否立马连接
---@param callback: 回调
---@param errorCallback: 错误回调
function clsTcpBase:Ctor(address, port, isEncrypt, isConnect, callback, errorCallback)
    -- 初始化数据
    self.onConnected = nil
    self.onConnectError = nil
    self.onConnecting = nil
    self.onDisconnected = nil
    self.currSerial = nil

    -- 记录
    self.address = address
    self.port = port

    -- 创建实例
    self.socket = gCSharp.NewTcp(address, port, isEncrypt)

    -- 注册事件
    self.socket:AddOnConnected(function()
        self:OnConnected()
    end)
    self.socket:AddOnConnectError(function(msg)
        self:OnConnectError(msg)
    end)
    self.socket:AddOnConnecting(function()
        self:OnConnecting()
    end)
    self.socket:AddOnDisconnected(function()
        self:OnDisconnected()
    end)
    self.socket:AddOnSendMsg(function(serial, ptc, str)
        self:OnSendMsg(serial, ptc, str)
    end)
    self.socket:AddOnReceiveMsg(function(serial, ptc, str)
        self:OnReceiveMsg(serial, ptc, str)
    end)

    -- 是否立马连接
    if isConnect then
        self:Connect(callback, errorCallback)
    end
end

--- ==================== 公共方法 ====================

--- 连接
function clsTcpBase:Connect(onConnected, onConnectError, onConnecting)
    self.onConnected = onConnected
    self.onConnectError = onConnectError
    self.onConnecting = onConnecting

    if self.socket then
        -- 开启遮罩
        gMgrs.cover.show_cover(gMgrs.cover.eCover.tcp)

        -- 连接
        self.socket:Connect()
    end
end

--- 断开连接
function clsTcpBase:Disconnect(onDisconnected)
    self.onDisconnected = onDisconnected

    if self.socket then
        self.socket:Disconnect()
    end

    self.socket = nil
end

--- 发送
function clsTcpBase:Send(protocol, msg)
    if msg == nil then
        return
    end

    if not self.socket then
        return
    end

    if not self.socket.IsConnected then
        gError("[tcp][send][还没连接到tcp服务器]")
        return
    end

    -- 如果是表
    if type(msg) == "table" then
        msg = gJson.encode(msg)
    else
        msg = tostring(msg)
    end

    -- 发送
    local serial = self.socket:Send(protocol, msg)

    if serial >= 0 then
        -- 记录序列号
        self.currSerial = serial

        -- 开启遮罩
        gMgrs.cover.show_cover(gMgrs.cover.eCover.tcp)
    end
end

--- ==================== 其他方法 ====================

--- 连接上回调
function clsTcpBase:OnConnected()
    gLogGroup("[TCP] 连接上: ", self.address, " 端口: ", self.port)

    -- 关闭遮罩
    gMgrs.cover.hide_cover(gMgrs.cover.eCover.tcp)

    -- 回调
    if self.onConnected then
        self.onConnected()
    end
end

--- 连接错误回调
function clsTcpBase:OnConnectError(msg)
    gLogGroup("[TCP] 连接错误: ", self.address, " 端口: ", self.port, " 信息: ", msg)

    -- 关闭遮罩
    gMgrs.cover.hide_cover(gMgrs.cover.eCover.tcp)

    -- 回调
    if self.onConnectError then
        self.onConnectError(msg)
    end
end

--- 连接中回调
function clsTcpBase:OnConnecting()
    -- 回调
    if self.onConnecting then
        self.onConnecting()
    end
end

--- 断开连接回调
function clsTcpBase:OnDisconnected()
    gLogGroup("[TCP] 断开连接: ", self.address, " 端口: ", self.port)

    -- 回调
    if self.onDisconnected then
        self.onDisconnected()
    end
end

--- 发送信息回调
function clsTcpBase:OnSendMsg(serial, ptc, str)
    gLog(string.format("[TCP] 发送数据：协议[%s],序列号[%s],内容[ %s ]", ptc, serial, str))
end

--- 接收信息回调
function clsTcpBase:OnReceiveMsg(serial, ptc, str)
    gLog(string.format("[TCP] 接收到数据：协议[%s],序列号[%s],内容[ %s ]", ptc, serial, str))

    if self.currSerial == serial then
        -- 关闭遮罩
        gMgrs.cover.hide_cover(gMgrs.cover.eCover.tcp)
    end

    -- 分发
    gMgrs.msg.send(gMgrs.msg.eMsg.net, ptc, str)
end

return clsTcpBase