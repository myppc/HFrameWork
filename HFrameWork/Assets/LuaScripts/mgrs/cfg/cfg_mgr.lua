---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by LeeroyLin.
--- DateTime: 2021/9/17 11:24
---

local cfgMapping = require("mgrs/cfg/cfg_mapping")

local cfgMgr = cfgMapping.cfgs

--- 列表转字典
---@param data: 数据
---@param templete: 模版
---@return :是否更改，新数据
local function list2dic(data, templete)
    -- 已经转换过了
    if data[1] == nil then
        return false, data
    end

    local nData = {}
    -- 遍历模版并转换为字典类型
    for name, idx in pairs(templete) do
        nData[name] = data[idx]
    end

    return true, nData
end

--- 获得键对应的项下标
local function get_key_cell_idx(mapping, key, startIdx, endIdx)
    -- 是否相同
    if startIdx == endIdx then
        return startIdx
    end

    -- 取中间值
    local centerIdx = math.floor((endIdx + startIdx) / 2)
    local centerIdx2 = centerIdx + 1

    -- 开始下标的起始值
    local startVal1 = mapping[startIdx][1]

    -- 中间下标的结束值
    local centerVal2 = mapping[centerIdx][2]
    local center2Val1 = mapping[centerIdx2][1]

    -- 结束下标的结束值
    local endVal2 = mapping[endIdx][2]

    -- 中间值之前
    if key >= startVal1 and key <= centerVal2 then
        return get_key_cell_idx(mapping, key, startIdx, centerIdx)
    elseif key >= center2Val1 and key <= endVal2 then
        return get_key_cell_idx(mapping, key, centerIdx2, endIdx)
    end
end

--- 获得键对应的下标
local function get_key_idx(mapping, name, key)
    -- 获得键对应的项下标
    local cellIdx = get_key_cell_idx(mapping, key, 1, #mapping)

    -- 判空
    if not cellIdx then
        gError(string.format("[配置表] 尝试从表'%s'中获取不存在的键值'%s'.", name, key))
        return
    end

    -- 获得项
    local cell = mapping[cellIdx]

    -- 返回数据下标
    return key - cell[1] + cell[3]
end

--- 获得单个数据
---@param cfg: 表
---@param key: 键
---@return: 表项数据
local function get(cfg, name, key)
    -- 获得数据下标
    local idx = get_key_idx(cfg.mapping, name, key)

    -- 通过下标取得数据
    local data = cfg.data[idx]

    -- 获得字典类型数据
    local isChanged, nData = list2dic(data, cfg.templete)

    -- 是否更改了
    if isChanged then
        -- 修改数据
        cfg.data[idx] = nData
    end

    return nData
end

--- 返回表的值个数
---@param cfg: 表
---@return: 表值个数
local function num(cfg)
    return cfg.num
end

--- 遍历表（键从小到大）
---@param cfg: 表
---@param callback: 回调方法 传递1个参数 值
local function pair(cfg, callback)
    -- 直接遍历表
    for idx, value in pairs(cfg.data) do
        -- 获得字典数据
        local isChanged, nData = list2dic(value, cfg.templete)

        -- 是否更改了
        if isChanged then
            -- 修改数据
            cfg.data[idx] = nData
        end

        -- 回调
        callback(nData)
    end
end

--- 初始化
function cfgMgr.init()
    for k, v in pairs(cfgMgr) do
        if type(v) ~= "function" then
            v.value = function(key)
                key = tonumber(key)
                if key == nil then
                    gError(string.format("[配置表] 尝试从表'%s'中获取空键", k, key))
                    return
                end
                key = tonumber(key)
                local cfg = require(cfgMapping.path[k])
                return get(cfg, k, key)
            end
            v.num = function()
                local cfg = require(cfgMapping.path[k])
                return num(cfg)
            end
            v.pair = function(callback)
                if callback == nil then
                    gError(string.format("[配置表][遍历表'%s'时传递了一个空回调方法]", k))
                    return
                end
                local cfg = require(cfgMapping.path[k])
                pair(cfg, callback)
            end
        end
    end
end

return cfgMgr