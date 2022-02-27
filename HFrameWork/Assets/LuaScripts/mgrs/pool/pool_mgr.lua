---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by LeeroyLin.
--- DateTime: 2021/9/24 12:28
---

local pool_mgr = {}

---初始化对象缓存池
function pool_mgr:pool_init()
    gCSharp.PoolInit();
end

---注册缓存信息
function pool_mgr:register_cache_info(path,name,poolType,cacheCount,sceneName)
    gCSharp.RegisterCacheInfo(path,name,poolType,cacheCount,sceneName)
end

---通过对象池获取对象
function pool_mgr:create_go_from_cache(path,name)
    gCSharp.CreateGoFromCache(path,name)
end

---回收对象
function pool_mgr:recovery_go(go)
    gCSharp.RecoveryGo(go)
end

---清理当前场景缓存
function pool_mgr:clear_scene_cache(sceneName,onlyTemp)
    gCSharp.ClearSceneCache(sceneName,onlyTemp)
end

---加载指定场景需要缓存的对象
function pool_mgr:load_cache_by_scene(sceneName)
    gCSharp.LoadCacheByScene(sceneName)
end

---为GameObject 打上标签，方便后续回收
function pool_mgr:add_tag(go,path,name)
    gCSharp.AddTag(go,path,name)
end

---遍历当前所有在Cache中缓存的对象用到的AB包
function pool_mgr:filter_cache_assets()
    return gCSharp.FilterCacheAssets()
end

return pool_mgr