local base_ui = require("base_ui")

local ui1 = gClass.declare("ui1",base_ui)

local name_index = require("ui1_index")

function ui1:Ctor()

end

---设置ui加载的prefab
function ui1:get_resource_config()
    return {
        mode = "mode1",
        asset = "ui1.prefab",
        ui_key = gUIKey.UI_TEST1,
    }
end

function ui1:on_loaded()
    self.child[name_index.BTN1].Button:AddEvent(function()
        gMgrs.ui:open_ui(gUIKey.UI_TEST2)
    end)

end

return ui1