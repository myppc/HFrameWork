local event_mgr = {}

---初始化方法
function event_mgr:init()
    self.reg_list = {}
    self.tag_list = {}
end


--[[]
    reg_lsit = 
    {
        [event_key] =
        {
            [call_key] = 
            {
                event_key,
                call_back,
                tag
            }
        }
    }
]]



--- 根据event_key 和call_back移除监听
---@param event_key any
---@param call_back any
function event_mgr:remove_listener_by_call(event_key,call_back)
    if not self.reg_list[event_key] then
        return
    end
    local event_list = self.reg_list[event_key]
    local info = event_list[tostring(call_back)]
    if info == nil then
        return
    end
    local tag = info.tag
    self:_remove_tag_info(tag,event_key)
    event_list[tostring(call_back)] = nil
end

---comment 将指定tag下注册event_key的所有回调都移除掉
---@param event_key any
---@param tag any
function event_mgr:remove_listener_by_tag(event_key,tag)
    if not self.reg_list[event_key] then
        return
    end
    local event_list = self.reg_list[event_key]
    for k,v in pairs(event_list) do
        if v.tag == tag then
            local info = v
            local tag = info.tag
            self:_remove_tag_info(tag,event_key)
            event_list[k] = nil
        end
    end
end

---移除tag下所有的监听事件
---@param tag any
function event_mgr:remove_tag_all_listeners(tag)
    local tag_list = self.tag_list[tag] or {}
    for k,event_key in pairs(tag_list) do
        self:remove_listener_by_tag(event_key,tag)
    end
    self.tag_list[tag] = nil
end

--- 监听事件
function event_mgr:add_listener(event_key,call_back,tag)
    tag = tag or "DEFAULT"
    self.reg_list[event_key] = self.reg_list[event_key] or {}
    local event_list = self.reg_list[event_key]
    local key = tostring(call_back)
    if event_list[key] then
        return
    end
    event_list[key] = {event_key = event_key,call_back = call_back,tag = tag}
    self.tag_list[tag] = self.tag_list[tag] or {}
    local tag_list = self.tag_list[tag]
    tag_list[#tag_list+1] = event_key
end

---发送事件
---@param event_key any
---@param param any
function event_mgr:send(event_key,param)
    local event_list = self.reg_list[event_key] or {}
    for k,v in pairs(event_list) do
        v.call_back(param)
    end
end

function event_mgr:remove_all_events()
    for tag,event_key in pairs(self.tag_list) do
        self:remove_tag_all_listeners(tag)
    end
end

function event_mgr:clear()
    self:remove_all_events()
    self.reg_list = {}
    self.tag_list = {}
end
--#region 私有方法

---移除指定tag下的指定事件
---@param tag any
---@param event_key any
function event_mgr:_remove_tag_info(tag,event_key)
    local tag_list = self.tag_list[tag] or {}
    for i = #tag_list,1,-1 do
        local cur_key = tag_list[i]
        if cur_key == event_key then
            table.remove(tag_list,i)
        end
    end
end


return event_mgr
--#endregion


