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
    },

    EPoolType =
    {
        TEMP = 0,--该场景零时对象池，退出场景时销毁
        STATIC = 1,--静态池，永久不销毁
        DYNAMIC = 2,--动态池，动态添加，永久不销毁
        SCENE = 3,--该场景对象池，每次在推出场景时销毁，在载入时自动加载  
    },

}
return system_enum