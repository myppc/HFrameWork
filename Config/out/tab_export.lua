--字段名    数字测试1    字符串测试1    表格测试1    表格测试2    表格测试3    语言表测试    
local key = 
{
    --- 字段名
    id = 1,
    --- 数字测试1
    num1 = 2,
    --- 字符串测试1
    str1 = 3,
    --- 表格测试1
    tab1 = 4,
    --- 表格测试2
    tab2 = 5,
    --- 表格测试3
    tab3 = 6,
    --- 语言表测试
    lang1 = 7,
}

local data = 
{
     [1] = {1000.0,4.0,'safasdf1234',{1.0,2.0,3.0,{5.0,5.0,1.0,}},{'1','2','sdff',{'666','ff',}},{4.0,'4f',12.0,{'fff','g1',}},1.0,},
     [2] = {1001.0,nil,'ffffffff',nil,{'1','2','sdff',{'666','ff',}},{4.0,'4f',12.0,{'fff','g1',}},2.0,},
}


local config_item = {}
config_item.key = key
config_item.data = data

return config_item