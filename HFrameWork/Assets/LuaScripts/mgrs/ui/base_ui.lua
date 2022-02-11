---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by LeeroyLin.
--- DateTime: 2021/8/23 10:26
---

local baseUI = gClass.declare("baseUI")

--- 构造方法
function baseUI:Ctor()
    self.uiKey = nil             -- ui键
    self.isUILoading = false     -- 标记ui是否在加载中
    self.isUIClose = false       -- 标记是否UI关闭了
    self.mainNode = nil          -- ui主节点
    self.uiArgs = {}             -- ui参数 默认空表
    self.uiGroup = {}            -- ui组件组 记录该ui下的ui组件
    self.uiRes = {}              -- 记录加载的资源
    self.subViews = {}           -- 记录子界面实例
    self.subViewData = nil       -- 子界面数据
    self.subViewIdx = -1         -- 子界面下标

    self._netHandlers = {}       -- 记录注册的网络事件字典 键为协议号 值为{ins, handler, errorHandler}
    self._netCallbackCache = {}  -- 缓存ui加载时网络的回调 {handler, args}
end

--- ==================== 生命周期 ====================

--- 初始化
function baseUI:OnInit()
    -- 注册网络事件
    self:RegNet()
end

--- 通过该方法设置该ui所需的资源 第一个必须为主资源
function baseUI:OnGetUIResources()
    local arrayRes = {
        --{module = "", name = ""}
    }

    return arrayRes
end

--- 设置网络处理
function baseUI:OnSetNet()
    return {}
end

--- 加载完毕回调
function baseUI:OnLoaded()
    -- 是否有缓存网络回调
    self:DispatchNet()
end

--- 获得UI组件
function baseUI:OnGetUIs()

end

--- 重新显示回调
function baseUI:OnShow()
end

--- 关闭前回调
function baseUI:OnClose()
    -- 注销网络事件
    self:UnRegNet()

    -- 关闭子节点
    self:ClearSubView()
end

--- 设置子界面数据 有子界面需求才设置
function baseUI:OnSetSubView()
    return nil
    --[[
        {
            -- 子界面实例获取器
            subViewInsGetter = function(idx)
            end,
            -- 子界面的根节点
            parentNode = nil,
            -- 是否改变界面时，销毁之前的界面，显示时重新加载新的。否则只是隐藏，显示后回调OnShow方法
            isChangeDestroy = false,
            -- 是否改变界面时，清除之前的界面的实例，显示时创建新的实例。
            isChangeClearIns = false,
        }
    ]]
end

--- ==================== 子界面方法 ====================

--- 显示子界面
---@param idx: 子界面下标
---@param args: 传参
function baseUI:ShowSubView(idx, args)
    if not self.subViewData then
        self.subViewData = self:OnSetSubView()
        if not self.subViewData then
            return
        end
    end

    -- 是否改变时销毁之前的
    if self.subViewData.isChangeDestroy then
        -- 删除所有节点
        for i, v in pairs(self.subViews) do
            gHelper.destroy_node(v.mainNode)
        end
    else
        -- 隐藏所有节点
        for i, v in pairs(self.subViews) do
            v.mainNode.gameObject:SetActive(false)
        end
    end

    -- 是否改变时清除旧实例
    if self.subViewData.isChangeClearIns then
        -- 删除数据
        self.subViews = {}
    end

    -- 是否没有实例
    self.subViewIdx = idx
    local ins = self.subViews[self.subViewIdx]
    if ins == nil then
        -- 新建实例
        ins = self.subViewData.subViewInsGetter(self.subViewIdx)

        -- 加载节点
        self:LoadSubView(ins, self.subViewData.parentNode)

        -- 回调
        ins.uiArgs = args
        ins:OnInit()
        ins:OnGetUIs()
        ins:OnLoaded()

        -- 记录
        self.subViews[self.subViewIdx] = ins
    else
        -- 显示节点
        ins.mainNode.gameObject:SetActive(true)

        -- 回调
        ins:OnShow()
    end
end

--- 加载子界面
function baseUI:LoadSubView(ins, parentNode)
    -- 获得资源信息
    local listRes = ins:OnGetUIResources()
    local res = {}
    for i, v in pairs(listRes) do
        -- 同步加载
        res[i] = gMgrs.res.load_asset(v.module, v.name).transform
    end

    -- 记录资源
    ins.mainNode = res[1]
    ins.uiRes = res

    -- 设置父节点
    ins.mainNode:SetParent(parentNode, false)
end

--- 清除子界面
function baseUI:ClearSubView()
    for i, v in pairs(self.subViews) do
        -- 回调
        v:OnClose()

        -- 删除节点
        gHelper.destroy_node(v.mainNode)
    end

    self.subViews = {}
end

--- ==================== 公共方法 ====================

--- 缓存或直接回调
function baseUI:CacheOrCallback(serviceData, handler)
    -- 是否ui在加载中
    if self.isUILoading then
        -- 缓存回调
        table.insert(self._netCallbackCache, {
            handler = handler,
            args = serviceData,
        })
        return
    end

    -- 直接回调
    handler(serviceData)
end

--- 注册网络事件
function baseUI:RegNet()
    -- 获得协议配置
    local ptcs = self:OnSetNet()

    -- 记录初始化请求信息 {ins=nil, args=nil}
    local initNets = {}

    -- 遍历
    for i, data in pairs(ptcs) do
        -- 是否有注册事件
        if data.callback then

            -- 回调方法
            local handler = function(serviceData)
                self:CacheOrCallback(serviceData, data.callback)
            end

            -- 错误回调方法
            local errorHandler = function(serviceData)
                self:CacheOrCallback(serviceData, data.errorCallback)
            end

            -- 记录
            self._netHandlers[data.ptcIns.protocol] = {
                ins = data.ptcIns,
                handler = handler,
                errorHandler = errorHandler,
            }

            -- 注册事件
            data.ptcIns:Reg(tostring(self), handler, errorHandler)

            local lastHandler = "none"
            if i > 1 then
                local dic = ptcs[i - 1].ptcIns.handlers
                for key, v in pairs(dic) do
                    lastHandler = v.handler
                end
            end

            -- 是否初始化就请求
            if data.isInitReq then
                -- 记录
                table.insert(initNets, {
                    ins = data.ptcIns,
                    args = data.args or {},
                })
            end
        end
    end

    -- 发送请求
    for i, v in pairs(initNets) do
        if v.ins.ptcType == "http" then
            v.ins:Send(table.unpack(v.args))
        else
            v.ins:Send(v.args)
        end
    end
end

--- 注销网络事件
function baseUI:UnRegNet()
    -- 遍历所有记录
    for ptc, v in pairs(self._netHandlers) do
        -- 注销网络事件
        v.ins:UnReg(tostring(self))
    end

    -- 置空列表
    self._netHandlers = {}
end

--- 关闭自身
function baseUI:CloseSelf()
    gMgrs.ui.close_ui(self.uiKey)
end

--- 检测自身是否是顶层
function baseUI:CheckIsTop()
    return gMgrs.ui.check_is_top(self.uiKey)
end

--- 分发网络回调
function baseUI:DispatchNet()
    for i, v in pairs(self._netCallbackCache) do
        v.handler(v.args)
    end
    self._netCallbackCache = {}
end

--- 获取组件
---@param path: 相对根节点路径
---@param compName: 组件名
function baseUI:GetComp(path, compName)
    return self.mainNode:Find(path):GetComponent(compName)
end

--- 获得变换对象
---@param path: 相对根节点路径
function baseUI:GetTrans(path)
    return self.mainNode:Find(path)
end

--- 获得游戏物体
---@param path: 相对根节点路径
function baseUI:GetObj(path)
    return self.mainNode:Find(path).gameObject
end

--- 获得按钮并注册事件
---@param path: 相对根节点路径
---@param handler: 点击处理方法(*)
---@param isAnim: 是否有抖动动画 默认有(*)
function baseUI:GetBtn(path, handler, isAnim)
    if isAnim == nil then
        isAnim = true
    end

    local btn = self.mainNode:Find(path):GetComponent("Button")
    btn:AddEvent(handler, isAnim)

    return btn
end

--- 获得文本
---@param path: 相对根节点路径
function baseUI:GetText(path)
    return self.mainNode:Find(path):GetComponent("Text")
end

--- 获得MeshPro文本
---@param path: 相对根节点路径
function baseUI:GetTextPro(path)
    return self.mainNode:Find(path):GetComponent("TextMeshProUGUI")
end

--- 获得图片
---@param path: 相对根节点路径
function baseUI:GetImage(path)
    return self.mainNode:Find(path):GetComponent("Image")
end

--- 获得矩形变换组件
---@param path: 相对根节点路径
function baseUI:GetRectTrans(path)
    return self.mainNode:Find(path):GetComponent("RectTransform")
end

--- 获得输入框并注册事件
---@param path: 相对根节点路径
---@param handlerChanged: 值改变处理方法
---@param handlerEnd: 结束编辑处理方法
function baseUI:GetInput(path, handlerChanged, handlerEnd)
    local input = self.mainNode:Find(path):GetComponent("InputField")
    input:AddEvent(handlerChanged, handlerEnd)

    return input
end

--- 获得开关并注册事件
---@param path: 相对根节点路径
---@param handlerChanged: 按钮处理方法
function baseUI:GetToggle(path, handlerChanged)
    local toggle = self.mainNode:Find(path):GetComponent("Toggle")
    toggle:AddEvent(handlerChanged)

    return toggle
end

--- 获得拖动条并注册事件
---@param path: 相对根节点路径
---@param handlerChanged: 值改变处理方法
function baseUI:GetSlider(path, handlerChanged)
    local slider = self.mainNode:Find(path):GetComponent("Slider")
    slider:AddEvent(handlerChanged)

    return slider
end

--- 获得滚动栏并注册事件
---@param path: 相对根节点路径
---@param handlerChanged: 值改变处理方法
function baseUI:GetScroll(path, handlerChanged)
    local scroll = self.mainNode:Find(path):GetComponent("ScrollRect")
    scroll:AddEvent(handlerChanged)

    return scroll
end

--- 获得有限滚动栏并注册事件
---@param path: 相对根节点路径
---@param handlerChanged: 值改变处理方法
function baseUI:GetInfiniteScroll(path, handlerChanged)
    local scroll = self.mainNode:Find(path):GetComponent("InfiniteScroll")
    scroll:AddEvent(handlerChanged)

    return scroll
end

return baseUI