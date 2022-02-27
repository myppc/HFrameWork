---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by LeeroyLin.
--- DateTime: 2021/9/10 12:08
---

local scene1 = require("scene1")
local scene2 = require("scene2")


local scene_key = {
    --- 场景枚举
    BASE_SCENE = "BASE_SCENE",
    SCENE1 = "SCENE1",
    SCENE2 = "SCENE2",


}

--- 配置
local scene_config = {
    [scene_key.SCENE1] = {
        module = "mode1", --模块名
        name = "Scene1", --场景名
        scene_class = scene1, --场景对象
        isloading = true, --是否使用loading 场景
        allow_pop = false, -- 是否允许pop该场景
    },
    [scene_key.SCENE2] = {
        module = "mode1",
        name = "Scene2",
        scene_class = scene2,
        isloading = true,
        allow_pop = true,
    },
}

return scene_config,scene_key