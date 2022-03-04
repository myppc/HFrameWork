local base_scene = require('base_scene')

local scene2 = gClass.declare("scene2",base_scene)

function scene2:on_loaded()
    gLog("---------------- cur on_loaded scene2")

    gMgrs.ui:open_ui(gUIKey.UI_TEST4,{msg = "open UI_TEST"})

end

return scene2