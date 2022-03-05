local base_ui = require("base_ui")

local ui2 = gClass.declare("ui2",base_ui)
local UI = require("ui2_index")

function ui2:Ctor()

end

---设置ui加载的prefab
function ui2:get_resource_config()
    return {
        mode = "mode2",
        asset = "ui2.prefab",
        ui_key = gUIKey.UI_TEST2,
    }
end

function ui2:on_loaded()
    self.child[UI.TEXT].Text.text = "22222222222222"
    
    self.child[UI.BTN1].Button:AddEvent(function()
        gMgrs.ui:open_ui(gUIKey.UI_TEST3)
    end)

end

return ui2