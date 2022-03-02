---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by LeeroyLin.
--- DateTime: 2021/8/13 13:10
---

local component_map = 
{
    img = "Image",
    rt = "RectTransform",
    atr = "Animator",
    t = "Text",
    scr = "ScrollView",
    iscr = "InfiniteScroll",
    btn = "Button",
}

local helper = {}

--- 是否是空物体节点
function helper.is_nil(obj)
    return obj == nil or obj:Equals(nil)
end

--- 值转尺寸字符串
function helper.value_2_size_str(value)
    local array = {"B", "KB", "MB", "GB", "TB"}

    local uVal = 1

    -- 反向遍历
    for i = #array, 1, -1 do
        -- 计算阶段值
        uVal = 1024 ^ (i - 1)

        -- 是否大于等于阶段值
        if value >= uVal then
            return string.format("%.2f", value / uVal) .. array[i]
        end
    end

    return value .. array[1]
end



--- 强制刷新UI
---@param rectTrans: 节点
---@param isGetComp: 是否还需要获取一次组件(*)
function helper.refresh_ui(rectTrans, isGetComp)
    local comp = rectTrans

    -- 是否还需要获取一次组件
    if isGetComp then
        comp = rectTrans:GetComponent("RectTransform")
    end

    gUnity.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(comp)
end

--- 遍历子节点
---@param node: 节点
---@param callback: 回调方法 传递子节点(Transform类型) 和 下标(1开始)
---@param isForwardPair: 是否正向遍历(*) 默认正向
function helper.pair_child_node(node, callback, isForwardPair)
    if isForwardPair == nil then
        isForwardPair = true
    end

    local startIdx = 0
    local endIdx = node.childCount - 1
    local dir = 1

    -- 是否反向
    if not isForwardPair then
        startIdx = endIdx
        endIdx = 0
        dir = -1
    end

    -- 遍历
    for i = startIdx, endIdx, dir do
        local t = i + 1
        callback(node:GetChild(i), t)
    end
end

--- 获取node下所有子节点
---@param node any
---@param list any
function helper.get_child(node,list)
    if list == nil then
        list = {}
    end
    local transform = node.transform
    local childCount = transform.childCount
    if childCount > 0 then
        for i=  0,childCount -1 do
            local child = node.transform:GetChild(i).gameObject
            list[child.name] = child
            helper.get_child(child,list)
        end
    end
    return list;
end

--- 遍历直接点，并获取组件
---@param node any
---@param list any
---@return any
function helper.filter_child(node,list)
    if list == nil then
        list = {}
    end

    local childCount = node.transfrom.childCount
    if childCount > 0 then
        for i=  0,childCount -1 do
            local child = node.transfrom:GetChild(i).gameObject
            local namelist = string.split(child.name,"#")
            if #namelist > 1 then
                local add = {}
                add.go = child
                for index = 2,#namelist do
                    local add_str = namelist[index];
                    if component_map[add_str] ~= nil then
                        local component = child.GetComponent(component_map[add_str])
                        add[component_map[add_str]] = component
                    end
                end
                list[namelist[1]] = add
            end
            helper.filter_child(child,list)
        end
    end
    return list;
end


return helper