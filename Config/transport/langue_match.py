import re
import path_config as path_config
import os
import langue_config 
import importlib


langue_save_path = path_config.langue_save_path
langue_dis_path = path_config.langue_dis_path
short_list = langue_config.langue_short
langue_transfrom = path_config.langue_transform_path
transform_module = langue_transfrom.split("/")[-2]


main_langue = {}
if os.path.exists(path_config.langue_main_file):
    main_langue = importlib.import_module(transform_module+ ".langue_main").langue

def input_tag():
    tag = input("输入导出翻译tag: ")

    try:
        tag = int(tag)
    except ValueError:
        raise ValueError("版本信息必须为数字")

    return tag

    
def find_file(dest_tag):    
    file1 = None
    file_list = os.listdir(langue_save_path)
    p1 = re.compile(r'[[](.*?)[]]', re.S)
    for file in file_list:
        tag = int(re.findall(p1, file)[0])
        if tag == dest_tag:
            file1 = file
    return file1


def load_langue(file_name):
    file = open(langue_save_path + "/"+ file_name,'r',encoding="utf-8")
    lines = file.readlines()
    file.close()
    langs = []
    for line in lines:
        if '] = ' in line:
            line = line.split('] = ')[1]
            line = line[:-2]
            langs.append(line)
    return langs

def match_different(lang,dest_short):
    dis = []
    for dis_lang in lang:
        if not dis_lang in main_langue:
            dis_dict = {}
            dis_dict["lang"] = dis_lang
            dis_dict["short"] = []
            if dest_short == "all":
                dis_dict["short"] += langue_config.langue_short
            else:
                dis_dict["short"].append(dest_short)
            dis.append(dis_dict)
        else:
            main_item = main_langue[dis_lang]
            dis_dict = {}
            dis_dict["lang"] = dis_lang
            dis_dict["short"] = []
            if dest_short == "all":
                for short in langue_config.langue_short:
                    if not short in main_item:
                        dis_dict["short"].append(short)

    return dis

def create_dis_str(dis_list,dest_short):
    content = ""
    content += "langue_dis = [\n"
    base_langue = 'zh'
    for dis in dis_list:
        content += "    {\n"
        if dest_short == "":
            for short in short_list:
                if short == base_langue:
                    content += "        '{_short}':{_dis},\n".format(_short = short,_dis = dis)
                else:
                    content += "        '{_short}':'',\n".format(_short = short)
        else:
            content += "        '{_short}':{_dis},\n".format(_short = base_langue,_dis = dis)
            content += "       '{_short}':'',\n".format(_short = dest_short)

        content += "\n    },\n"
    content += "]"
    return content

def save_dis_file(dest_tag,dest_short,content):
    if dest_short =="":
        dest_short = "all"
    filename = '{_path}{_short}-[{_dest_tag}]-langue_transfrom.py'.format(_path = path_config.langue_dis_path,_short = dest_short,_dest_tag = dest_tag)
    file = open(filename,'w+',encoding="utf-8")
    file.write(content)
    file.close()
    print("差异文件已导出:",filename)


def main():
    dest_tag = input_tag()
    dest_short = input("指定翻译版本? 默认所有版本! short =")
    if not dest_short in short_list:
        dest_short = ""
        input("指定翻译版本为 [所有语言]")
    else:
        input("指定翻译版本为 [{0}]".format(dest_short))
    file_name = find_file(dest_tag)
    lang = load_langue(file_name)
    dis = match_different(lang,dest_short)
    content = create_dis_str(dis,dest_short)
    save_dis_file(dest_tag,dest_short,content)


main()