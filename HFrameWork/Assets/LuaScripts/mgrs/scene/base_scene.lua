local base_scene = gClass.declare("base_scene")

--- ==================== 生命周期 ====================

--- 构造方法
function base_scene:Ctor()
    self.scene_key = nil             -- 场景枚举
    self.param = {}             -- 参数

end

---comment 由管理器调用的初始化
---@param scene_key any
---@param param any
function base_scene:_init(scene_key,param)
    self.scene_key = scene_key
    self.param = param
    self.scene_root = gMgrs.res:load(gEnum.ERes.GameObject,"System","SceneRoot.prefab")
end

---comment 由管理器调用，使用者不要复写
function base_scene:_close()
    
end

--- ==================== 生命周期 ====================
--- 加载完毕回调
function base_scene:on_loaded()

end

--- 关闭前回调
function base_scene:on_close()

end

return base_scene