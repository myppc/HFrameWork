local base_ui = require("base_ui")

local ui1 = gClass.declare("ui1",base_ui)

function ui1:Ctor()

end

---设置ui加载的prefab
function base_ui:get_resource_config()
    return {
        mode = "mode1",
        asset = "ui1.prefab",
        ui_key = gUIKey.UI_TEST1,
    }
end

function ui1:on_loaded()
    gLog("-----------load ui1 param " .. tostring(self.param.msg))
end

return ui1