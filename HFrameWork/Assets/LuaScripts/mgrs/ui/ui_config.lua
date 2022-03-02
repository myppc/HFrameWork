local base_ui = require("mgrs/ui/base_ui")
local ui1 = require("ui1")

local ui_config =
{
    [gUIKey.BASE_UI] = 
    {
        background = true,  --是否有背景遮罩
        click_close = true, --点击背景遮罩是否关闭ui
        ui_class = base_ui, --生成的UI对象
        ui_type = gEnum.EUIType.UI, --ui类型，管理器会根据不同类型设置ui层级和父节点
        reload = true, --如果该UI在栈内，重新打开场景时是否需要加载该UI
        alway_show = true, --在栈内位置不是top时，会检查该字段，若为true，该ui不会隐藏 
    },
    [gUIKey.UI_TEST1] = 
    {
        background = true,  --是否有背景遮罩
        click_close = true, --点击背景遮罩是否关闭ui
        ui_class = ui1, --生成的UI对象
        ui_type = gEnum.EUIType.UI, --ui类型，管理器会根据不同类型设置ui层级和父节点
        reload = true, --如果该UI在栈内，重新打开场景时是否需要加载该UI
        alway_show = true, --在栈内位置不是top时，会检查该字段，若为true，该ui不会隐藏 
    }
}

return ui_config
