---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by LeeroyLin.
--- DateTime: 2021/8/25 7:38
---

local clsPtcBase = require("mgrs/net/class_ptc_base")
local clsHttpPtc = gClass.declare("clsHttpPtc", clsPtcBase)

--- ==================== 构造方法 ====================

--- 构造方法
---@param url: Url地址 可带%s
---@param isPost: 是则是Post 否则是Get
---@param isCache: 是否缓存服务器返回数据(默认不缓存)
---@param handler: 基础回调(可空)
function clsHttpPtc:Ctor(url, isPost, isCache, handler)
    self.isPost = isPost
    self.base.Ctor(self, url, isCache, handler)

    self.ptcType = "http"
end

--- ==================== 私有方法 ====================

--- 获得数据和参数
---@param {...}: 总参数
---@return: post传递的数据，url参数
local function get_data_and_args(...)
    local data
    local args

    --- 遍历所有参数
    for i, v in pairs({ ... }) do
        if i == 1 then
            data = v
        else
            table.insert(args, v)
        end
    end

    return data, args
end

--- ==================== 公共方法 ====================

--- 异步请求
---@param {...}: 参数
function clsHttpPtc:Send(...)
    -- 是否是Post请求
    if self.isPost then
        -- 获得数据和参数
        local data, args = get_data_and_args(...)

        -- 拼接参数到url
        local url = string.format(self.protocol, table.unpack(args))

        -- 请求
        gMgrs.net.http_post_async(url, data,
        function (data)
            self:OnRec(data)
        end,
        function (data)
            self:OnError(data)
        end)

    else
        -- 拼接参数到url
        local url = string.format(self.protocol, ...)

        gMgrs.net.http_get_async(url,
        function (data)
            self:OnRec(data)
        end,
        function (data)
            self:OnError(data)
        end)
    end
end

--- 同步请求
---@param {...}: 参数
function clsHttpPtc:SendSync(...)
    local resData

    -- 是否是Post请求
    if self.isPost then
        -- 获得数据和参数
        local data, args = get_data_and_args(...)

        -- 拼接参数到url
        local url = string.format(self.protocol, table.unpack(args))

        -- 请求
        resData = gMgrs.net.http_post(url, data, function (data)
            self:OnError(data)
        end)

    else
        -- 拼接参数到url
        local url = string.format(self.protocol, ...)

        -- 请求
        resData = gMgrs.net.http_get(url, function (data)
            self:OnError(data)
        end)
    end

    -- 回调
    self:OnRec(resData)
end

return clsHttpPtc