local pool_type = 
{
    TEMP = 0,--该场景零时对象池，退出场景时销毁
    STATIC = 1,--静态池，永久不销毁
    DYNAMIC = 2,--动态池，动态添加，永久不销毁
    SCENE = 3,--该场景对象池，每次在推出场景时销毁，在载入时自动加载
    
}

return pool_type