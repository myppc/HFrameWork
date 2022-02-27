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

function ui_stack_item:push_info(ui_key,info)
    local ui_type = gUICfg[ui_key]
    local stack = self.ui_stack[ui_type]
    local remove_index = -1
    for k,v in pairs(stack) do
        if v.ui_key == ui_key then
            remove_index = k
            break
        end
    end

    for index = #stack,remove_index,-1 do
        self:pop_by_ui_type(ui_type)
    end

    stack[#stack+1] = info
    gMgrs.ui:_on_stack_top(info.ui_key)
end

---comment 关闭指定ui层的顶层UI
---@param ui_type any
function ui_stack_item:pop_by_ui_type(ui_type)
    local stack = self.ui_stack[ui_type]
    local top_info = stack[#stack]
    gMgrs.ui:close_ui(top_info.ui_key)
    stack[#stack] = nil
end

---comment 返回一个从Top层到BG层的排列顺序
function ui_stack_item:get_ui_sort()
    local ret = {}
    for layer = gEnum.EUIType.Top ,gEnum.EUIType.Bg,-1 do
        local stack = self.ui_stack[layer]
        for i = #stack,1,-1 do
            ret[#ret + 1] = stack[i].ui_key
        end
    end
    return ret
end

---comment 获取指定ui的信息
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

return ui_stack_item