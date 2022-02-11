---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by LeeroyLin.
--- DateTime: 2021/7/29 18:12
---

local uiEnum = {}

--- UI枚举
uiEnum.eUI = {
    update = "update",
    sysDialog = "sysDialog",
    login = "login",
    chooseServer = "chooseServer",
    tips = "tips",
    mainCity = "mainCity",
    propertyBar = "propertyBar",
    playerInfo = "playerInfo",
    mainFixed = "mainFixed",
    world = "world",
    bag = "bag",
    hero = "hero",
    heroDetails = "heroDetails",
}

--- 节点枚举
uiEnum.eNodes = {
    module = "Module",  -- 模块层  对应节点名
    fixed = "Fixed",    -- 固定层
    top = "Top",        -- 顶层
    system = "System",  -- 系统层
}

return uiEnum