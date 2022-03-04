---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by LeeroyLin.
--- DateTime: 2021/9/10 12:07
---
local loading_scene = require("loading_scene")

local LOADING_SCENE = "LOADING_SCENE";
local ENTRACE_SCENE = "EntraceScene";

local scene_mgr = {}
-- function scene_mgr:open_scene(sceneName,args,finish, progressCallback )
--     gCSharp.OpenScene(sceneName,args,finish, progressCallback)
-- end

function scene_mgr:init()
    self.cur_scene_name = ENTRACE_SCENE
    self.scene_stack = {}
end


--#region 公有方法

--- 打开场景，默认loadingscene
function scene_mgr:open_scene(scene_key,param,on_finish,progressCallback)
    ---先关闭当前场景
    local isloading = gSceneCfg[scene_key].isloading or true
    if isloading then
        ---进入loading 场景
        gCSharp.LoadScene(LOADING_SCENE,function()
            self:_last_scene_close()
            self:_clear_scene_resource(scene_key)
            gCSharp.UnloadScene(self.cur_scene_name)
            self.cur_scene_name = LOADING_SCENE
            self:_loading_to_new_scene(scene_key,param,on_finish,progressCallback)
        end)
    else
       self:_just_load_new_scene(scene_key,param,on_finish,progressCallback)
    end
end

--- 弹出顶层的场景
function scene_mgr:pop_scene()
    if #self.scene_stack <= 1 then
        return;
    end
    local last_scene_info = self.scene_stack[#self.scene_stack - 1]
    if last_scene_info then
        self:open_scene(last_scene_info.scene_key,last_scene_info.param)
    end
end

--- 重新加载当前场景
function scene_mgr:reload_cur_scene()
    if #self.scene_stack <= 0 then
        return;
    end
    local last_scene_info = self:_get_last_scene()
    if last_scene_info then
        self:open_scene(last_scene_info.scene_key,last_scene_info.param)
    end
end

--#endregion

--#region 私有方法
---在场景退出时清理资源
---@param new_scene_key any
function scene_mgr:_clear_scene_resource(new_scene_key)
    local last_scene_info = self:_get_last_scene()
    if last_scene_info == nil then
        return
    end
    local last_scene_name = gSceneCfg[last_scene_info.scene_key].name
    if last_scene_info.scene_key == new_scene_key then
        --只清理临时缓存
        gMgrs.pool:clear_scene_cache(last_scene_name,true);
    else
        --清理预加载Prefab
        gMgrs.res:clear_pre_go_cache();
        --清理对象池中的
        gMgrs.pool:clear_scene_cache(last_scene_name);
        --清理没有用到的AB包
        gCSharp.UnLoadAllABCache();
    end
end


--- 找当前的栈信息，将其剔除掉
---@param scene_key string key
---@return any
---@return any
function scene_mgr:_find_stack(scene_key)
    local stack_scene_info = nil
    local scene_index = nil
    for index = #self.scene_stack,1,-1 do
        local scene_info = self.scene_stack[index]
        if scene_info.scene_key ==scene_key then
            stack_scene_info = scene_info
            scene_index = index;
            break
        end
    end
    return stack_scene_info,scene_index
end

--- 如果该场景在栈中，则弹出栈
---@param scene_key any
function scene_mgr:_pop_stack(scene_key)
    local scene_info ,scene_index = self:_find_stack()
    if scene_index == nil then
        return
    end
    for index = #self.scene_stack,scene_index,-1 do
        self.scene_stack[#self.scene_stack] = nil
    end
end    

--- 向栈内压入参数和key
---@param scene_key any
---@param param any
function scene_mgr:_push_stack(scene_key,param,new_scene)
    self.scene_stack[#self.scene_stack+1] = {scene_key = scene_key,param = param,scene = new_scene}
end

--- 获取当前scene
function scene_mgr:_get_last_scene()
    return self.scene_stack[#self.scene_stack]
end

--- 在进入loading后，准备进入用户指定的场景
function scene_mgr:_loading_to_new_scene(scene_key,param,on_finish,progressCallback)

    ---生成loading scene
    local loading = loading_scene:new()
    ---初始化Loading场景
    loading:_init(LOADING_SCENE)
    ---回调loading的on_load
    loading:on_loaded()


    ---加载新的场景
    local scene_info = gSceneCfg[scene_key]
    local scene_name = scene_info.name
    gCSharp.LoadScene(scene_name,
        --- 新场景加载完成回调
        function()
            if on_finish then
                on_finish()
            end
            
            gCSharp.UnloadScene(LOADING_SCENE)
            self.cur_scene_name = scene_name;
            self:_init_new_scene(scene_key,param)
        end
        --- 更新进度函数
        ,function(value)
            loading:update_prog_value(value)
            if progressCallback then
                progressCallback(value)
            end
        end
    )
end

--- 直接进入新场景，不使用loading
---@param scene_key any
---@param param any
---@param on_finish any
---@param progressCallback any
function scene_mgr:_just_load_new_scene(scene_key,param,on_finish,progressCallback)
    local scene_info = gSceneCfg[scene_key]
    local scene_name = scene_info.name
    
    gCSharp.LoadScene(scene_name,
        --- 新场景加载完成回调
        function()
            if on_finish then
                on_finish()
            end
            self:_last_scene_close()
            self:_clear_scene_resource(scene_key)
            gCSharp.UnloadScene(self.cur_scene_name)
            ---初始化场景
            self.cur_scene_name = scene_name;
            self:_init_new_scene(scene_key,param)
        end
        --- 更新进度函数
        ,function(value)
            if progressCallback then
                progressCallback(value)
            end
        end
    )
end

--- 销毁老场景前调用生命周期
function scene_mgr:_last_scene_close()
    local old_scene_info = self:_get_last_scene()
    if old_scene_info then
        --释放栈内UI
        gMgrs.ui:close_all_ui()
        --由base scene来释放相关资源
        old_scene_info.scene:_close()
        --由使用者使用的 关闭回调
        old_scene_info.scene:on_close()
        self:_pop_stack(old_scene_info.scene_key)

    end
end

--- 为加载好的场景生成实例并且调用生命周期
function scene_mgr:_init_new_scene(scene_key,param)
    local scene_info = gSceneCfg[scene_key]
    ---缓存池加载场景需要的缓存对象
    gMgrs.pool:load_cache_by_scene(scene_key)
    ---生成新场景实例
    local new_scene = scene_info.scene_class:new()
    ---将新场景入栈
    self:_push_stack(scene_key,param,new_scene)
    ---为新场景生成UI根节点
    new_scene.scene_root = gMgrs.res:load(gEnum.ERes.GameObject,"system","SceneRoot.prefab")
    ---将生成好的UI根节点设置到ui管理器中
    gMgrs.ui:on_load_new_scene(scene_key,new_scene.scene_root)
    ---初始化新场景，传递参数
    new_scene:_init(scene_key,table.deepcopy(param) )
    ---根据配置来从新加载场景UI，完成后回调场景on_load
    if scene_info.reload_ui then
        gMgrs.ui:reload_scene_ui(function()
            new_scene:on_loaded()
        end)
    else
        new_scene:on_loaded()
    end
end

--#endregion

return scene_mgr