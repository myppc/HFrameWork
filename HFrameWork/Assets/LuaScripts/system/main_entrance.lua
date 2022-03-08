---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by LeeroyLin.
--- DateTime: 2021/7/29 16:58
---

local mainEntrance = {}

--- 设置全局
function mainEntrance.set_global()
    gUnity = require("system/code_hints/code_hints_unityengine") -- +
    gUnity = CS.UnityEngine

    gCSharp = require("system/code_hints/code_hints_lua_bridge") -- +
    gCSharp = CS.LuaCallCSharpHelper

    --gPath = CS.System.IO.Path
    gHelper = require('system/helper') -- +

    local log = require('system/log') -- +
    gLog = log.print
    gLogGroup = log.print_group
    gError = log.print_error

    gJson = require("system/json") -- +

    gClass = require("system/class_helper") -- + 
    gMgrs = require('system/mgrs') --
    --gData = require("system/data_center") --
    --gCfg = require("mgrs/cfg/cfg_mgr") --
    gColor = require("system/color") -- +
    gUtf8 = require("system/extension/utf8")
    gEnum = require("system/system_enum")
    gUIKey = require("ui_key")
    gUICfg = require("mgrs/ui/ui_config")
    gSceneKey = require("scene_key")
    gSceneCfg = require("mgrs/scene/scene_config")
end

--- 设置扩展
function mainEntrance.set_extension()
    require("system/extension/table_ex")
    require("system/extension/string_ex")
    require("system/extension/math_ex")
end

--- 屏蔽全局
function mainEntrance.lock_global()
    setmetatable(_G, {
        __index = function()
            gLog(string.format("不可使用全局字段"))
        end,
        __newindex = function(t, k, v)
            gLog(string.format("不可使用全局字段: %s", k))
        end
    })
end

--- 开始游戏
--function mainEntrance.start_game()
    -- 初始化网络管理器
    --gMgrs.net.init()

    -- 初始化unity更新管理器
    --gMgrs.unityUpdate.init()

    -- 初始化计时器管理器
    --gMgrs.timer.init()

    -- 初始化更新管理器
    --gMgrs.update.init()

    -- 初始化资源管理器
    --gMgrs.res.init()

    -- 初始化协议
    --local protocol = require("modules/protocol/protocol")
    --protocol.init()

    -- 初始化数据管理器
    --gData.init()

    -- 初始化ui管理器
    --gMgrs.ui:init()

    -- 初始化配置表
    --gCfg.init()

    -- 进入热更新流程
    --mainEntrance.start_update()

    -- 1. 拉取基础资源 基场景, uiCanvas预制体，更新界面预制体，lua系统级和管理层代码
    -- 2. 打开基场景
    -- 3. 启动lua，初始化网络，更新管理器
    -- 4. 检测版本并更新manifest
    -- 5. 初始化ui，资源管理器
    -- 6. 正常lua流程打开更新界面
    -- 7. 开始热更新
    -- 8. 更新完毕且校验通过后存储版本文件
    -- 9. 进入登录界面

    -- 1. 检测版本并更新manifest
    -- 2. 检测基础资源是否有更新 有则拉取基础资源 基场景，uiCanvas预制体，更新界面预制体，lua系统级和管理层代码
    -- 3. 正常启动lua
    -- 4. 正常lua流程打开更新界面
    -- 5. 热更新
    -- 6. 更新完毕且校验通过后存储版本文件
    -- 7. 进入登录界面
--end

--- 入口方法
function mainEntrance.run()
    -- 设置全局
    mainEntrance.set_global()

    -- 设置扩展
    mainEntrance.set_extension()

    -- 屏蔽全局
    mainEntrance.lock_global()
    
    ---初始化管理器
    mainEntrance.init_mgr()

    -- 开始游戏
    mainEntrance.start_game()

end

---初始化管理器
function mainEntrance.init_mgr()
    gMgrs.scene:init()
    gMgrs.ui:init()
    gMgrs.unityUpdate:init()
    gMgrs.tick:init()
    gMgrs.tick:start_tick()
    gMgrs.timer:init();
    gMgrs.event:init()
end

function mainEntrance.start_game()
    gMgrs.scene:open_scene(gSceneKey.SCENE1,{msg = "open scene1 "})
end

--- 注销回调
function mainEntrance.on_destroy()
    gMgrs.ui:clear()
    gMgrs.tick:clear()
    gMgrs.timer:clear()
    gMgrs.unityUpdate:clear()
    gMgrs.event:clear()

end

--- 执行注销
function mainEntrance.destroy()
    -- -- 销毁回调
    mainEntrance.on_destroy()

    -- -- 销毁所有UI
    -- gMgrs.ui.clear_all()

    -- -- 关闭场景
    -- gMgrs.scene.unload_scene(function ()
    --     -- 重新加载游戏
    --     gCSharp.ReloadEntrance()
    -- end)
end

return mainEntrance
