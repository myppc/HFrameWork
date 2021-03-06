---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by Administrator.
--- DateTime: 2021/8/15 20:24
---

local rapidjson = require('rapidjson')

local json = {}

--- 序列化
---@param obj: 对象
---@return : json字符串
function json.encode(obj)
    return rapidjson.encode(obj)
end

--- 反序列化
---@param jsonStr: json字符串
---@return : 对象
function json.decode(jsonStr)
    return rapidjson.decode(jsonStr)
end

return json