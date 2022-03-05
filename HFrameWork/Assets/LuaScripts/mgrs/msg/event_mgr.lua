local event_mgr = {}

---初始化方法
function event_mgr:init()
    self.reg_list = {}
    self.uid = 0
end


--- 监听事件
function event_mgr:add_listener(event_key,call_back,tag)
    if not self.reg_list[event_key] then
        self.reg_list[event_key] = {}
    end

    
end

