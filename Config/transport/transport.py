import os
from posixpath import split
import xlrd
import sys
import path_config as path_config

excel_path = path_config.excel_path
out_path = path_config.out_path


if sys.getdefaultencoding() != 'utf-8':
	reload(sys)
	sys.setdefaultencoding('utf-8')

langue_dict = {}
config_key_dict = {}

#类型解析函数
def get_analysis_func(type_name):
    funcs = {
        'number':to_number,
        'string':to_string,
        'table':to_table,
        'mix':to_mix,
        'langue':to_langue
    }
    if type_name in funcs:
        return funcs[type_name]
    else:
        return None


#扫描xlsx文件
def scan_files(dir):
    path_list = []
    for lists in os.listdir(dir):
        path = os.path.join(dir,lists)
        if "~$" in path:
            continue
        if ".xlsx" in path:
            path_list.append(path)
    return path_list

#对每张表进行导表工作
def load_all_excels(path_list):
    all_datas = {}
    for path in path_list:
        excel_data = load_excel(path)
        all_datas[excel_data["name"]] = excel_data
    return all_datas
        

#读取数据
def load_excel(file_path):
    file_name = file_path.split('\\').pop()
    xl = xlrd.open_workbook(file_path)
    sheets = xl.sheets()
    key_list,desc_list,type_list =read_head(sheets[0])
    sheet_len = len(sheets)
    excel_data = {}
    excel_data['keys'] = key_list
    excel_data['desc'] = desc_list
    excel_data['types'] = type_list
    excel_data['name'] = file_name
    excel_data['values'] = []
    for index in range(sheet_len):
        sheet = sheets[index]
        start_index = 0 
        if index == 0:
            start_index = 3
        max_row = sheet.nrows
        for row_index in range(start_index,max_row):
            values = sheet.row_values(row_index)
            excel_data['values'].append(values)
    return excel_data

#读取头部信息
def read_head(sheet):
    keys = sheet.row_values(0)
    desc = sheet.row_values(1)
    types = sheet.row_values(2)
    for index in range(len(types)-1, -1, -1):
        if types[index] == "":
            keys.pop(index)
            desc.pop(index)

    return keys,desc,types

#将excel数据转换成lua字符串
def excel_data_to_lua_str(data):
    file_name = data["name"]
    file_name = file_name.replace(".xlsx","").lower()
    config_key = "t_" + file_name
    if config_key in config_key_dict:
        raise ValueError("错误：表重复 " + file_name)
    config_key_dict[config_key] = file_name
    content = ""
    lua_head = create_lua_head(data)
    lua_valuse = create_lua_value(data)
    content = content + lua_head + "\n"
    content = content + lua_valuse + "\n"
    content = content + "\nlocal config_item = {}\nconfig_item.key = key\nconfig_item.data = data\n\nreturn config_item"
    return content

#转换成数字类型
def to_number(value):
    if value == "":
        return "nil"
    return str(float(value))

def to_string(value):
    if value == "":
        return "nil"
    return "'" + str(value) + "'"

def to_table(value,child_type):
    if value == "":
        return "nil"
    value_list = split_table(value,[])
    value_list = split_list_to_value(value_list,child_type)
    ret = split_value_to_lua(value_list)
    return ret

def to_mix(value):
    if value == "":
        return "nil"
    if is_number(value):
        return to_number(value)
    else:
        return to_string(value)

def to_langue(value):
    if value in langue_dict:
        return langue_dict[value]
    index = len(langue_dict) + 1
    langue_dict[value] = index
    return to_number(index)

#将数据组装厂lua字符串
def split_value_to_lua(value_list):
    content = "{"
    for item in value_list:
        if type(item) == list :
            content += split_value_to_lua(item) 
        else:
            content += item + ","
    return content + "}"

#将切分好的数据list转换到对应类型的数据
def split_list_to_value(split_list,func_type):
    ret = []
    for item in split_list:
        if type(item) == list:
            new_item = split_list_to_value(item,func_type)
            ret.append(new_item)
        else:
            func = get_analysis_func(func_type)
            if func ==None:
                raise ValueError("错误type:{}".format(func_type))
            ret.append(func(item))
    return ret
    
def is_number(s):
    try:
        float(s)
        return True
    except ValueError:
        pass
 
    try:
        import unicodedata
        unicodedata.numeric(s)
        return True
    except (TypeError, ValueError):
        pass
 
    return False

#将字符串{}解析成列表
def split_table(value,cur_stack):
    if value == '':
        return cur_stack[0]
    
    end_pos = value.find('}')
    start_pos = value.find('{')
    #已经不包含套表情况了
    if  start_pos == -1:
        end = value[:end_pos]
        ret = value[end_pos + 1:]
        if end != ""and end != ',':
            end_list = end.split(',')
            if len(end_list) > 0 and end_list[-1] == "":
                end_list.pop(-1)
            if len(end_list) > 0 and end_list[0] == "":
                end_list.pop(0)   
            cur_stack += end_list
        return (ret,cur_stack)
    else:
        if end_pos < start_pos:
            head = value[:end_pos]
            last_value = value[end_pos + 1:]
            if head != "" and head != ',':
                head_list = head.split(',')
                if len(head_list) > 0 and head_list[-1] == "":
                    head_list.pop(-1)
                if len(head_list) > 0 and head_list[0] == "":
                    head_list.pop(0)   
                cur_stack += head_list
                return (last_value,cur_stack)
            else:
                return (last_value,cur_stack)
        else:
            head = value[:start_pos]
            last_value = value[start_pos + 1:]
            if head != "" and head != ',':
                head_list = head.split(',')
                if len(head_list) > 0 and head_list[-1] == "":
                    head_list.pop(-1)
                if len(head_list) > 0 and head_list[0] == "":
                    head_list.pop(0)   
                cur_stack += head_list
            new_stack = []
            cur_stack.append(new_stack)
            (new_value,stack) = split_table(last_value,new_stack)
            return split_table(new_value,cur_stack)


#转化excel数据
def create_lua_value(data):
    content = "local data = \n{\n"
    types = data['types']
    keys = data['keys']

    for index in range(len(data['values'])):
        msg = "     [{}] = {{".format(str(index+1))
        value_list = data['values'][index]
        for value_index in range(len(value_list)):
            value = value_list[value_index]
            if types[value_index] == '':
                continue
            item_type = types[value_index]
            type_split = item_type.split('|')
            main_type = type_split[0]
            second_type = None
            if len(type_split) > 1:
                second_type = type_split[1]
            func = get_analysis_func(main_type)
            if func ==None:
                raise ValueError("错误type:{} ,定位 {} - {} - {}".format(main_type,data['name'],index,keys[value_index]))
            if second_type == None:
                msg = msg + func(value) + ","
            else:
                msg = msg + func(value,second_type) + ","
        msg = msg + "},\n"
        content = content + msg
    content += "}\n"
    return content




#生成lua头
def create_lua_head(data):
    content = "--"
    descs = data['desc']
    keys = data['keys']
    for index in range(len(descs)):
        content = content + descs[index] + "    "

    content =content + "\nlocal key = \n{\n"

    for index in range(len(keys)):
        content = content +"    --- "+ descs[index]   +"\n"
        content = content + "    " + keys[index] +" = " + str(index + 1) + ",\n"
            
    content = content + "}\n"
    return content


#进行转换,将数据转换到lua
def transport_datas_to_lua(all_datas):
    for file_name in all_datas:
        data = all_datas[file_name]
        lua_str = excel_data_to_lua_str(data)
        lua_name = file_name.replace('xlsx','lua')
        file = open(out_path + lua_name.lower(),'w+',encoding="utf-8")
        file.write(lua_str)
        file.close()

#保存语言表
def save_langue():
    content = "local langue =\n{\n"
    for lang_info in (langue_dict):
        index = langue_dict[lang_info]
        content = content + "   [{}] = '{}' ,\n".format(index,lang_info)
    content += "\n}\nreturn langue"
    file = open(out_path + 'langue.lua','w+',encoding="utf-8")
    file.write(content)
    file.close()

#保存表索引
def save_langue():
    content = "local config_key =\n{\n"
    for config_key in (config_key_dict):
        file_name = config_key_dict[config_key]
        content = content + "   {} = '{}' ,\n".format(config_key,file_name)
    content += "\n}\nreturn config_key"
    file = open(out_path + 'config_key.lua','w+',encoding="utf-8")
    file.write(content)
    file.close()

def main():
    path_list = scan_files(excel_path)
    all_datas = load_all_excels(path_list)
    transport_datas_to_lua(all_datas)
    save_langue()
    print("TRANSPORT TO LUA SUCC!!!")

print("START TRANSPORT")

main()


