local ui_stack_item = gClass.declare("ui_stack_item")

function ui_stack_item:Ctor(scene_name)
    self.scene_name = scene_name
    self.ui_stack = {
        [gEnum.EUIType.Bg] = {},
        [gEnum.EUIType.UI] = {},
        [gEnum.EUIType.Bar] = {},
        [gEnum.EUIType.Pop] = {},
        [gEnum.EUIType.Top] = {},
    }
end

--- 向栈内压入信息
---@param ui_key any
---@param info any
function ui_stack_item:push_info(ui_key,info)
    local ui_type = gUICfg[ui_key].ui_type
    local stack = self.ui_stack[ui_type]
    stack[#stack+1] = info
end

--- 关闭指定ui
---@param ui_type any
function ui_stack_item:pop_info(ui_key)
    local ui_type = gUICfg[ui_key].ui_type
    local stack = self.ui_stack[ui_type]
    for k,v in pairs(stack) do
        if v.ui_key == ui_key then
            table.remove(stack,k)
            break
        end
    end
end

--- 返回一个从Top层到BG层的排列顺序,index越小，越靠前
function ui_stack_item:get_ui_sort(start_type,end_type)
    start_type = start_type or gEnum.EUIType.Bg
    end_type = end_type or gEnum.EUIType.Top
    if end_type < start_type then
        return {}
    end

    local ret = {}
    for layer = end_type ,start_type,-1 do
        local stack = self.ui_stack[layer]
        for i = #stack,1,-1 do
            ret[#ret + 1] = stack[i].ui_key
        end
    end
    return ret
end

--- 获取指定ui的信息
---@param ui_key any
---@return any
function ui_stack_item:get_ui_info(ui_key)
    local ui_type = gUICfg[ui_key].ui_type
    local stack = self.ui_stack[ui_type]
    for k,v in pairs(stack) do
        if v.ui_key == ui_key then
            return v
        end
    end
    return nil
end

--- 查找指定UI的位置
---@param ui_key any
---@return any
function ui_stack_item:ui_is_open(ui_key)
    local ui_type = gUICfg[ui_key].ui_type
    local stack = self.ui_stack[ui_type]
    for k,v in pairs(stack) do
        if v.ui_key == ui_key then
            return true
        end
    end
    return false
end

return ui_stack_item