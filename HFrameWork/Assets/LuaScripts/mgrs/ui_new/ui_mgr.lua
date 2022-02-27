local ui_mgr = {}

function ui_mgr:init()
    self.ui_stack = {}
    self.child = {}
    self.cur_scene = nil
end

---comment 打开ui
---@param ui_key any
---@param param any
function ui_mgr:open_ui(ui_key,param)
    local ui_ins = gUICfg[ui_key].ui_class:new()
    local go_info = ui_ins:get_resource_config()
    gMgrs.res:load_async(gEnum.ERes.GameObject,go_info.mode,go_info.asset,function(go)
        self:_on_ui_loaded(ui_key,go,ui_ins,param)
    end)
end

function ui_mgr:close_ui(ui_key)
    
end

---comment 弹出顶层ui
function ui_mgr:pop_ui()
end

------------------------------------------
---私有方法 

---comment 在UI节点加载好了之后进行回调
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
end

---comment 为新生成的UI节点添加上屏蔽背景
function ui_mgr:_add_block_for_ui(ui_key,go)
    local block = gCSharp.GameObject.Instance(self.child["BlockUI"])
    block:SetActive(true)
    block.name = string.format("%s_block",ui_key)
    go.transform:SetParent(block.transform,false)
    block:GetComponent("Button"):AddEvent(function()
        if gUICfg[ui_key].click_close then
            self:close_ui(ui_key)
        end
    end)
    return block
end

---comment 根据配置将UI添加至对应的layer中
---@param ui_key any
---@param go any
function ui_mgr:_add_new_ui_to_layer(ui_key,go)
    local ui_type = gUICfg[ui_key].ui_type
    local layer = self:get_layer(ui_type)
    go.transform:SetParent(layer.transform,false)
end

----HZYND 打开UI的时候要隐藏后面的UI，需要调用栈内UI的on_hide
---comment 设置该UI到达栈的Top

function ui_mgr:_on_stack_top(ui_key)

end

---comment 根据key返回layer节点
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

function ui_mgr:_update_ui_visible()
    
end

---comment 刷新blackground的位置
function ui_mgr:_update_black_ui_pos()
    local stack_item = self.ui_stack[self.cur_scene]
    local sort = stack_item:get_ui_sort()
    for index = #sort ,1,-1 do
        local ui_key = sort[index]
        local ui_info = self:_get_ui_info(ui_key)
        if gUICfg[ui_key].background and ui_info.ins.is_show then
            self:_add_to_blackground(ui_key)
            return
        end
    end
    ---如果所有UI都不需要，就直接隐藏block
    self:_release_black()
end

---comment 设置场景根节点
---@param root any
function ui_mgr:_set_scene_root(root)
    self.child = gHelper.get_child(root)
    self.child["ClickBlock"]:SetActive(false)
    self.child["BlockUI"]:SetActive(false)
end

---comment 将blackground 从层级节点中释放出来，将blackground的子节点设置到对应layer中,并且隐藏掉block
function ui_mgr:_release_black()
    if self.child["BlackGround"].transform.childCount > 0 then
        local last_ui = self.child["BlackGround"].transform.GetChild(0)
        last_ui.transform:SetParent(self.child["BlackGround"].transform.parent,false)
    end
    self.child["BlackGround"].SetActive(false)
end

---comment 将block设置给指定UI
---@param ui_key any
function ui_mgr:_add_to_blackground(ui_key)
    local ui_info = self:_get_ui_info(ui_key)
    if ui_info == nil then
        return
    end
    self:_release_black()
    self.child["BlackGround"].SetActive(true)
    local go = ui_info.ui_ins.go
    go.transform:SetParent(self.child["BlackGround"].transform,false)
    local ui_type = gUICfg[ui_key].ui_type
    local layer = self:get_layer(ui_type)
    self.child["BlackGround"].transform:SetParent(layer.transform,false)
end

---comment 获取指定ui的信息
function ui_mgr:_get_ui_info(ui_key)
    local stack_item = self.ui_stack[self.cur_scene]
    return stack_item:get_ui_info(ui_key)
end

---comment 压入ui 信息
---@param info {ui_key:string, ins: any, param:table}
function ui_mgr:_push_info(info)
    local stack_item = self.ui_stack[self.cur_scene]
    stack_item:push_info(info.ui_key,info)
end


---comment 清理当前的scene_root
function ui_mgr:_clear_scene_root()
    self.child = {}
end

return ui_mgr