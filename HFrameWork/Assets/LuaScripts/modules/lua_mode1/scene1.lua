local base_scene = require('base_scene')

local scene1 = gClass.declare("scene1",base_scene)

function scene1:on_loaded()
    gLog("---------------- cur on_loaded scene1")
    gMgrs.ui:open_ui(gUIKey.UI_TEST1,{msg = "open UI_TEST"})
end

return scene1