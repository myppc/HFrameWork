local ui_stack_item = require("ui_stack_item")
local ui_mgr = {}

function ui_mgr:init()
    self.ui_stack = {}
    self.child = {}
    self.cur_scene = nil
    self.reloading = false;
end

--- 打开ui
---@param ui_key any
---@param param any
function ui_mgr:open_ui(ui_key,param)
    if self.reloading  then
        return
    end
    local ui_ins = gUICfg[ui_key].ui_class:new()
    local go_info = ui_ins:get_resource_config()
    gMgrs.res:load_async(gEnum.ERes.GameObject,go_info.mode,go_info.asset,function(go)
        self:_on_ui_loaded(ui_key,go,ui_ins,param)
    end)
end

--- 关闭指定的UI
---@param ui_key any
function ui_mgr:close_ui(ui_key)
    if self.reloading  then
        return
    end
    local ui_info = self:_get_ui_info(ui_key)
    ui_info.ins:_update_visible(false)
    ui_info.ins:on_destroy()
    gCSharp.UnityDestroy(ui_info.ins.go)
    self:_pop_info(ui_key)
    self:_update_ui_visible()
    self:_update_black_ui_pos()

end

--- 弹出顶层ui
function ui_mgr:pop_ui()
    local cur_stack = self.ui_stack[self.cur_scene]
    local keys = cur_stack:get_ui_sort()
    --关闭顶层UI
    for index = 1,#keys do
        local ui_key = keys[index]
        if gUICfg[ui_key].ui_type ~= gEnum.EUIType.Bar then
            self:close_ui(ui_key)
            break
        end
    end
end

--- 关闭全部的ui
function ui_mgr:close_all_ui()
    if gSceneCfg[self.cur_scene].reload_ui then
        self:_release_scene_ui()
    else
        self:_destroy_scene_ui()
    end
end



--- 从新加载当前场景下的所有UI
function ui_mgr:reload_scene_ui(on_complete_call)
    if self.reloading  then
        return
    end
    self.reloading = true;
    self.child["ClickBlock"]:SetActive(true)
    if self.ui_stack[self.cur_scene] == nil then
        self.ui_stack[self.cur_scene] = ui_stack_item:new(self.cur_scene)
    end

    local stack_item = self.ui_stack[self.cur_scene]
    local key_list = stack_item:get_ui_sort()
    self:_reload_next_ui(key_list,1,on_complete_call)
end

--- 设置场景根节点
---@param root any
function ui_mgr:on_load_new_scene(scene_key,root)
    self.cur_scene = scene_key
    self.child = gHelper.get_child(root)
    self.child["ClickBlock"]:SetActive(false)
    self.child["BlockUI"]:SetActive(false)
end


------------------------------------------
--#region 私有方法

--- 将场景下所有UI释放掉，切不保留信息
function ui_mgr:_destroy_scene_ui()
    if self.reloading  then
        return
    end
    local stack_item = self.ui_stack[self.cur_scene]
    local keys = stack_item:get_ui_sort()
    for index = 1,#keys do
        self:close_ui(keys[index])
    end
end

--- 将场景下所有节点释放掉，但是会保留信息，以便于从新加载栈
function ui_mgr:_release_scene_ui()
    if self.reloading  then
        return
    end
    local stack_item = self.ui_stack[self.cur_scene]
    local keys = stack_item:get_ui_sort()
    for index = 1,#keys do
        local ui_key = keys[index]
        local ui_info = self:_get_ui_info(ui_key)
        ui_info.ins:_update_visible(false)
        ui_info.ins:on_destroy()
        gCSharp.UnityDestroy(ui_info.ins.go)
        ui_info.ins = nil
    end
end

--- 当从新加载的UI完成后，进行下一个的加载
---@param key_list any
---@param cur_index any
function ui_mgr:_reload_next_ui(key_list,cur_index,on_complete_call)
    local ui_key = key_list[cur_index]

    if ui_key == nil then
        self:on_reload_finish()
        if on_complete_call then
            on_complete_call()
        end
        return
    end

    local ui_info = self:_get_ui_info(ui_key)
    local param = ui_info.param
    local ui_ins = gUICfg[ui_key].ui_class:new()
    local go_info = ui_ins:get_resource_config()
    gMgrs.res:load_async(gEnum.ERes.GameObject,go_info.mode,go_info.asset,function(go)
        -- 添加屏蔽层
        local block = self:_add_block_for_ui(ui_key,go)
        ui_info.ins = ui_ins
        --将ui节点加入到对应UI节点下
        self:_add_new_ui_to_layer(ui_key,block)
        --进行初始化UI
        ui_ins:_init(block,table.deepcopy(param))
        ui_ins:_update_visible(true)
        self:_reload_next_ui(key_list,cur_index + 1)
    end)
end


--- 在ui从新载入后回调,如果全都回调完成后就会刷新black位子和设置可见
function ui_mgr:on_reload_finish()
    self.child["ClickBlock"]:SetActive(false)
    self.reloading = false;
    --刷新每层UI的可见，参考UI配置的alway_show来设置可见,每层只有top的UI必然可见，其他根据配置
    self:_update_ui_visible()
    --刷新背景UI的位置
    self:_update_black_ui_pos()
end

--- 在UI节点加载好了之后进行回调
---@param ui_key any
---@param go any
function ui_mgr:_on_ui_loaded(ui_key,go,ui_ins,param)
    -- 添加屏蔽层
    local block = self:_add_block_for_ui(ui_key,go)
    ---对ui信息进行压栈，此过程如果出现ui循环，则会自动关闭循环部分，然后进行压栈
    local ui_info = {}
    ui_info.param = param
    ui_info.ui_key = ui_key
    ui_info.ins = ui_ins
    self:_push_info(ui_info)
    --将ui节点加入到对应UI节点下
    self:_add_new_ui_to_layer(ui_key,block)
    --进行初始化UI
    ui_ins:_init(block,param)
    --刷新每层UI的可见，参考UI配置的alway_show来设置可见,每层只有top的UI必然可见，其他根据配置
    self:_update_ui_visible()
    --刷新背景UI的位置
    self:_update_black_ui_pos()
    
end

--- 为新生成的UI节点添加上屏蔽背景
function ui_mgr:_add_block_for_ui(ui_key,go)
    if gUICfg[ui_key].ui_type == gEnum.EUIType.Bar then
        return go
    end
    local block = gUnity.GameObject.Instantiate(self.child["BlockUI"])
    block:SetActive(true)
    block.name = string.format("%s_block",ui_key)
    go.transform:SetParent(block.transform,false)
    block:GetComponent("Button"):AddEvent(function()
        if gUICfg[ui_key].click_close then
            self:close_ui(ui_key)
        end
    end,false)
    return block
end

--- 根据配置将UI添加至对应的layer中
---@param ui_key any
---@param go any
function ui_mgr:_add_new_ui_to_layer(ui_key,go)
    local ui_type = gUICfg[ui_key].ui_type
    local layer = self:get_layer(ui_type)
    go.transform:SetParent(layer.transform,false)
end

--- 根据key返回layer节点
---@param ui_type any
function ui_mgr:get_layer(ui_type)
    local switch = 
    {
        [gEnum.EUIType.Bg] = self.child["BgLayer"],
        [gEnum.EUIType.UI] = self.child["UILayer"],
        [gEnum.EUIType.Bar] = self.child["GuideBarLayer"],
        [gEnum.EUIType.Pop] = self.child["PopLayer"],
        [gEnum.EUIType.Top] = self.child["TopLayer"],
    }
    return switch[ui_type]
end

--- 刷新每层UI的可见，参考UI配置的alway_show来设置可见,每层只有top的UI必然可见，其他根据配置
function ui_mgr:_update_ui_visible()
    local cur_stack = self.ui_stack[self.cur_scene]
    for type_name,ui_type in pairs(gEnum.EUIType) do
        if ui_type ~= gEnum.EUIType.Bar then
            local cur_sort = cur_stack:get_ui_sort(ui_type,ui_type)
            gLog(cur_sort)
            for index,ui_key in pairs(cur_sort) do
                local ui_info = self:_get_ui_info(ui_key)
                ui_info.ins:_update_visible((index == 1) or gUICfg[ui_info.ui_key].alway_show)
            end
        end
    end
end

--- 刷新blackground的位置
function ui_mgr:_update_black_ui_pos()
    local stack_item = self.ui_stack[self.cur_scene]
    local sort = stack_item:get_ui_sort()
    gLog(sort)
    for index = 1 ,#sort do
        local ui_key = sort[index]
        local ui_info = self:_get_ui_info(ui_key)
        if gUICfg[ui_key].ui_type ~= gEnum.EUIType.Bar then
            if gUICfg[ui_key].background and ui_info.ins.is_show then
                self:_add_to_blackground(ui_key)
                return
            end
        end
    end
    ---如果所有UI都不需要，就直接隐藏block
    self:_release_black()
end



--- 将blackground 从层级节点中释放出来，将blackground的子节点设置到对应layer中,并且隐藏掉block
function ui_mgr:_release_black()
    if self.child["BlackGround"].transform.childCount > 0 then
        local last_ui = self.child["BlackGround"].transform:GetChild(0)
        last_ui.transform:SetParent(self.child["BlackGround"].transform.parent,false)
    end
    self.child["BlackGround"]:SetActive(false)
    self.child["BlackGround"].transform:SetParent(self.child["TempLayer"].transform,false)
end

--- 将block设置给指定UI
---@param ui_key any
function ui_mgr:_add_to_blackground(ui_key)
    local ui_info = self:_get_ui_info(ui_key)
    if ui_info == nil then
        return
    end
    self:_release_black()
    self.child["BlackGround"]:SetActive(true)
    local go = ui_info.ins.go
    go.transform:SetParent(self.child["BlackGround"].transform,false)
    local ui_type = gUICfg[ui_key].ui_type
    local layer = self:get_layer(ui_type)
    self.child["BlackGround"].transform:SetParent(layer.transform,false)
end

--- 获取指定ui的信息
function ui_mgr:_get_ui_info(ui_key)
    local stack_item = self.ui_stack[self.cur_scene]
    return stack_item:get_ui_info(ui_key)
end

--- 压入ui 信息
---@param info {ui_key:string, ins: any, param:table}
function ui_mgr:_push_info(info)
    if self.ui_stack[self.cur_scene] == nil then
        self.ui_stack[self.cur_scene] = ui_stack_item:new()
    end
    local stack_item = self.ui_stack[self.cur_scene]
    local is_open = stack_item:ui_is_open(info.ui_key)
    ---如果要压入的ui不是加载到bar层而且已经打开过，那么就要将其之上的ui全部关闭
    ---如果是加载到bar层上的，那么释放之前的ui，从新加载新的ui
    if is_open then
        if gUICfg[info.ui_key].ui_type ~= gEnum.EUIType.Bar then
            local ui_list = stack_item:get_ui_sort()
            for index = 1,#ui_list do
                local cur_key = ui_list[index]
                if gUICfg[cur_key].ui_type ~= gEnum.EUIType.Bar then
                    self:close_ui(cur_key)
                    if cur_key == info.ui_key then
                        break
                    end
                end
            end
        else
            self:close_ui(info.ui_key)
        end
    end

    stack_item:push_info(info.ui_key,info)
end





--- 将指定UI出栈
---@param ui_key any
function ui_mgr:_pop_info(ui_key)
    local stack_item = self.ui_stack[self.cur_scene]
    stack_item:pop_info(ui_key)
end

--- 清理当前的scene_root
function ui_mgr:_clear_scene_root()
    self.child = {}
end

--#endregion

return ui_mgr