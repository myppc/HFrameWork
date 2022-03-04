local base_ui = require("base_ui")

local ui4 = gClass.declare("ui4",base_ui)
local UI = require("ui2_index")

function ui4:Ctor()

end

---设置ui加载的prefab
function ui4:get_resource_config()
    return {
        mode = "mode2",
        asset = "ui2.prefab",
        ui_key = gUIKey.UI_TEST4,
    }
end

function ui4:on_loaded()
    gLog("============= 444444444444  on_loaded");
    gLog({self.child[UI.BTN1].Button});

    self.child[UI.TEXT].Text.text = "444444444444"
    self.child[UI.BTN1].Button:AddEvent(function()
        gLog("============= click btn");
        gMgrs.ui:open_ui(gUIKey.UI_TEST5)
    end)
end

return ui4