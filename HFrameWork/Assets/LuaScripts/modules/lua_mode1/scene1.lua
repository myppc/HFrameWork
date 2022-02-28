local base_scene = require('base_scene')

local scene1 = gClass.declare("scene1",base_scene)

function scene1:on_loaded()
    gLog("---------------- cur on_loaded scene1")
end

return scene1