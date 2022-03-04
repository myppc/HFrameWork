local base_ui = require("base_ui")

local ui3 = gClass.declare("ui3",base_ui)
local name_index = require("ui2_index")

function ui3:Ctor()

end

---设置ui加载的prefab
function ui3:get_resource_config()
    return {
        mode = "mode2", 
        asset = "ui2.prefab",
        ui_key = gUIKey.UI_TEST3,
    }
end

function ui3:on_loaded()
    self.child[name_index.TEXT].Text.text = "33333333"
    self.child[name_index.BTN1].Button:AddEvent(function()
        gMgrs.ui:open_ui(gUIKey.UI_TEST4)
    end)

    self.child[name_index.BTN4].Button:AddEvent(function()
        gMgrs.ui:pop_ui()
    end)
end

return ui3