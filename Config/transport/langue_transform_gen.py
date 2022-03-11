import re
import path_config as path_config
import os
import langue_config 
import importlib
import sys
sys.path.append("..")



langue_short = langue_config.langue_short
langue_transfrom = path_config.langue_transform_path
transform_module = langue_transfrom.split("/")[-2]
main_langue = {}
if os.path.exists(path_config.langue_main_file):
    main_langue = importlib.import_module(transform_module+ ".langue_main").langue

def create_main_langue_instance():
    if os.path.exists(path_config.langue_main_file):
        return
    content = ""
    content = "langue = {}"
    file = open(path_config.langue_main_file,'w+',encoding="utf-8")
    file.write(content)
    file.close()

def get_input():
    dest_tag = input("输入差异文件翻译后的tag: ")
    try:
        dest_tag = int(dest_tag)
    except ValueError:
        raise ValueError("版本信息必须为数字")
    short = input("输入差异文件语言类型： 默认为 all ")
    if not short in langue_short:
        short = "all"
        input("指定翻译版本为 [所有语言]")
    else:
        input("指定翻译版本为 [{0}]".format(short))

    return dest_tag,short


def read_file(dest_tag,short):
    filename = '{_short}-[{_dest_tag}]-langue_transfrom.py'.format(_short = short,_dest_tag = dest_tag)
    if not os.path.exists(path_config.langue_transform_path + filename):
        input("没有找到该文件 " + filename)
        exit()
    module_name = filename.replace(".py","")

    content=importlib.import_module(transform_module+ "."+ module_name)
    langue_dis = content.langue_dis
    for dis in langue_dis:
        key = dis["zh"]
        if key in main_langue:
            pass
        else:
            main_langue[key] = {}
        main_item = main_langue[key]
        for dis_key in dis:
            main_item[dis_key] = dis[dis_key]

def record_main_langue():
    content = ""
    content += "langue = {\n"
    for main_key in main_langue:
        main_item = main_langue[main_key]
        content += "    '{_main_key}':{{\n".format(_main_key = main_key)
        for short in main_item:
            lang = main_item[short]
            if lang == "" or lang == '':
                continue
            content += "        '{_short}':'{_lang}',\n".format(_short = short,_lang = lang)
        content += "    },\n"
    content += "}"

    file = open(path_config.langue_main_file,'w+',encoding="utf-8")
    file.write(content)
    file.close()


def main():
    create_main_langue_instance()
    dest_tag,short = get_input()
    read_file(dest_tag,short)
    record_main_langue()

main()