---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by LeeroyLin.
--- DateTime: 2021/7/30 10:28
---

local appCfg = require("system/app_cfg")

local resMgr = {
    manifest = {        -- 主配置
        --modules = {
        --    ["moduleName"] = {
        --        abs = {
        --            ["abName"] = {
        --                hash = "",
        --                crc = "",
        --                dependencies = {},
        --            }
        --        },
        --        assets = {
        --            ["assetName"] = {
        --                abName = "",
        --                path = "",
        --            }
        --        },
        --    }
        --},
    },
    isLog = true,       -- 是否打印加载ab的日志
    dicAB = {           -- 记录AB信息的字典
        --["abName"] = {
        --    req = nil,                    -- 异步加载对象
        --    ab = nil,                     -- ab对象
        --    listCallback = {},            -- 回调列表
        --    listProgressCallback = {},    -- 进度回调列表
        --}
    },
    eRes = {            -- 资源类型
        gameObject = 1,
        sprite = 2,
        atlas = 3,
        audio = 4,
        scene = 5,
    },
}

--- ==================== 私有方法 ====================

--- ==================== 公共方法 ====================

--- 初始化
function resMgr.init()
    -- 初始化资源管理器
    gCSharp.InitResMgr(resMgr.isLog)

    -- 设置主配置
    resMgr.manifest = gCSharp.GetManifest()
end

--- 同步加载AB
---@param abName : ab名
---@return : 返回ab
function resMgr.load_ab(abName)
    return gCSharp.LoadAB(abName)
end

--- 异步加载AB
---@param abName : ab名
---@param callback : 回调方法 参数AssetBundle
---@param progressCallback : 进度回调方法 参数float
function resMgr.load_ab_async(abName, callback, progressCallback)
    gCSharp.LoadABAsync(abName, callback, progressCallback)
end

--- 卸载ab包
---@param abName : ab包名
function resMgr.unload_ab(abName)
    gCSharp.UnLoadAB(abName)
end

--- 同步加载资源
---@param module : 模块名
---@param assetName : 资源名
---@param eRes : 资源类型 空则为GameObject
---@return : 返回资源
function resMgr.load_asset(module, assetName, eRes)
    eRes = eRes or gMgrs.res.eRes.gameObject
    return gCSharp.LoadAsset(module, assetName, eRes)
end

--- 同步加载精灵图
---@param module : 模块名
---@param assetName : 精灵图名
---@return : 返回精灵图
function resMgr.load_sprite(module, assetName)
    return gCSharp.LoadAsset(module, assetName, gMgrs.res.eRes.sprite)
end

--- 异步加载资源
---@param module : 模块名
---@param assetName : 资源名
---@param callback : 回调方法 参数object
---@param eRes : 资源类型 空则为GameObject
---@param progressCallback : 进度回调方法 参数float
function resMgr.load_asset_async(module, assetName, callback, eRes, progressCallback)
    eRes = eRes or gMgrs.res.eRes.gameObject
    gCSharp.LoadAssetAsync(module, assetName, callback, eRes, progressCallback)
end

return resMgr