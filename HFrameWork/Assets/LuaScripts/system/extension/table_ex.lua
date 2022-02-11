---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by LeeroyLin.
--- DateTime: 2021/9/26 9:25
---

--- 表内容转字符串
---@param tbl: 表
---@param level: 子级 可空 外部使用不用传递该值
---@return : 表字符串
function table.tostring(tbl, level)
    level = level or 0

    -- 传入的类型
    local tblType = type(tbl)

    -- 是否是字符串
    if tblType == "string" then
        local array = {
            "\"",
            tbl,
            "\"",
        }
        return table.concat(array)
    elseif tblType == "table" then  -- 是否是表

    else
        return tostring(tbl)
    end

    local str_array = {}

    -- 根据子级决定前面的空格
    local spacing = "    "
    for i = 1, level do
        table.insert(str_array, spacing)
    end
    local pre = table.concat(str_array)
    table.insert(str_array, spacing)
    local valuePre = table.concat(str_array)

    -- 重置字符串表
    str_array = {}

    -- 前缀
    if level == 0 then
        table.insert(str_array, "\n")
    end

    -- 前括号
    table.insert(str_array, "{")

    local keyType

    -- 遍历
    local count = 0
    for i, v in pairs(tbl) do
        -- 计次
        count = count + 1

        -- 间隔
        if count > 1 then
            table.insert(str_array, ",")
        end

        -- 获得键类型
        keyType = type(i)

        -- 值前括号
        table.insert(str_array, "\n")
        table.insert(str_array, valuePre)
        table.insert(str_array, "[ ")

        -- 是否键是数值类型
        if keyType == "number" then
            table.insert(str_array, tostring(i))

        elseif keyType == "string" then -- 字符串类型
            table.insert(str_array, "\"")
            table.insert(str_array, tostring(i))
            table.insert(str_array, "\"")
        else    -- 其他类型
            table.insert(str_array, tostring(i))
        end

        table.insert(str_array, " ] = ")

        -- 递归值
        table.insert(str_array, table.tostring(v, level + 1))

    end

    -- 后括号
    table.insert(str_array, "\n")
    table.insert(str_array, pre)
    table.insert(str_array, "}")

    return table.concat(str_array)
end

--- 返回表数量
---@param tbl: 表
function table.num(tbl)
    local count = 0
    for i, v in pairs(tbl) do
        count = count + 1
    end

    return count
end

--- 按值排序
---@param tbl: 原表
---@param value: 值
---@param isAsce: 是则升序，否则降序
function table.sort_by_value(tbl, value, isAsce)
    -- 判空
    if tbl == nil then
        return
    end

    local newTable = {}
    for k, v in pairs(tbl) do
        table.insert(newTable, v)
    end

    table.sort(newTable, function(a, b)
        if isAsce then
            if a[value] < b[value] then
                return true
            end
        else
            if a[value] > b[value] then
                return true
            end
        end
    end)
    return newTable
end

--- 查找某个元素
---@param tbl: 表
---@param val: 元素
---@return: 是否找到 bool类型
function table.find(tbl, val)
    for i, v in pairs(tbl) do
        if v == val then
            return true
        end
    end

    return false
end