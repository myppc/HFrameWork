local system_enum = 
{
    ---资源类型
    ERes = 
    {
        GameObject = 0,
        Sprite = 1,
        Atlas = 2,
        Audio = 3,
        TextAsset = 4,
    },

    EUIType = 
    {
        Bg = 1,      --最底层bg
        UI = 2, --普通UI，会被资产条挡住
        Bar = 3,    --资产条 
        Pop = 4,    --弹出UI，会盖在资产条上方
        Top = 5,    --最顶层UI，会改在所有UI以及场景上方
    }

}
return system_enum