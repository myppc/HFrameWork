local base_ui = gClass.declare("base_ui")


function base_ui:Ctor()
    self.param = {} -- 外部传入参数会保存在这里
    self.go = nil; --UI的根节点
    self.child = {}
    self.is_show = false;
end

---设置ui加载的prefab
function base_ui:get_resource_config()
    return {
        mode = "",
        asset = "",
        ui_key = gUIKey.base_ui,
    }
end

---由管理器调用
function base_ui:_init(go,param)
    self.go = go
    self.param = param;
    self.child = gHelper.filter_child(go)
    self:on_loaded()
end

--------
--生成时调用顺序

    ---- 1.管理器调用_init生成prefab，添加到指定位置，获取UI中的对象
    ---- 2.调用on_load prefab实例化完成调用，实例化对象已经存在，但是不可见，这时候不可见  
    ---- 3.调用register_event 方法注册监听事件
    ---- 4.调用on_update 每次打开时，从ui_mgr设置显示时，会调用刷新界面
    ---- 5.on_show 在每次显示时会调用
--------
--销毁时调用顺序
    ---- 1.release_register_event  取消注册监听事件
    ---- 2.on_hide 隐藏ui
    ---- 2.on_destroy 销毁时调用

----on_show 和 on_hide ui_mgr会主动调用


---------------------------------------
--生命周期
function base_ui:on_loaded()
end

function base_ui:on_register_event()
end

function base_ui:on_update()
end

function base_ui:on_show()
end

function base_ui:release_register_event()
end

function base_ui:on_hide()
end

function base_ui:on_destroy()
end
------------------------------------------