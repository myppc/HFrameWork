local base_ui = require("base_ui")

local ui5 = gClass.declare("ui5",base_ui)
local UI = require("ui2_index")

function ui5:Ctor()

end

---设置ui加载的prefab
function ui5:get_resource_config()
    return {
        mode = "mode2",
        asset = "ui2.prefab",
        ui_key = gUIKey.UI_TEST5,
    }
end

function ui5:on_loaded()
    self.child["Text"].Text.text = "5555555"

    self.child[UI.BTN1].Button:AddEvent(function()
        gMgrs.ui:open_ui(gUIKey.UI_TEST3)
    end)
end

return ui5