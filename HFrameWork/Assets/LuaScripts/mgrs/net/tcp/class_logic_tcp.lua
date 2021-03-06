---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by LeeroyLin.
--- DateTime: 2021/9/8 15:06
---

local clsTcpBase = require("mgrs/net/tcp/class_tcp_base")
local clsLogicTcp = gClass.declare("clsLogicTcp", clsTcpBase)

--- ==================== 构造方法 ====================

--- 构造方法
---@param address: 地址
---@param port: 端口
---@param isEncrypt: 是否加密
---@param isConnect: 是否立马连接
---@param callback: 回调
---@param errorCallback: 错误回调
function clsLogicTcp:Ctor(address, port, isEncrypt, isConnect, callback, errorCallback)
    self.base.Ctor(self, address, port, isEncrypt, isConnect, callback, errorCallback)
end

--- ==================== 其他方法 ====================

--- 连接上回调
function clsLogicTcp:OnConnected()
    self.base.OnConnected(self)
end

--- 连接错误回调
function clsLogicTcp:OnConnectError(msg)
    self.base.OnConnectError(self, msg)
end

--- 连接中回调
function clsLogicTcp:OnConnecting()
    self.base.OnConnecting(self)
end

--- 断开连接回调
function clsLogicTcp:OnDisconnected()
    self.base.OnDisconnected(self)
end

--- 发送信息回调
function clsLogicTcp:OnSendMsg(serial, ptc, str)
    self.base.OnSendMsg(self, serial, ptc, str)
end

--- 接收信息回调
function clsLogicTcp:OnReceiveMsg(serial, ptc, str)
    self.base.OnReceiveMsg(self, serial, ptc, str)
end

return clsLogicTcp