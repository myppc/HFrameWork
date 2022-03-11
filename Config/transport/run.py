import transport
import os
import shutil
import datetime
import path_config as path_config

out_path = path_config.out_path
langue_save_path = path_config.langue_save_path
unity_project_path = path_config.unity_project_path
config_path = path_config.config_path

is_copy_config_to_unity = input("是否拷贝配置到unity工程? <Y/N>[default = Y]")

if is_copy_config_to_unity.upper() != "N":
    if os.path.exists(unity_project_path + config_path):
        shutil.rmtree(unity_project_path + config_path)
    shutil.copytree(out_path,unity_project_path + config_path)

is_save = input("是否保存当前langue?  <Y/N>[default = N] ")

if is_save.upper() == "Y":
    if not os.path.exists(langue_save_path):
        os.mkdir(langue_save_path)
    # 返回当前文件夹下文件的个数
    len = len(os.listdir(langue_save_path)) + 1


    time = datetime.datetime.now().strftime("%Y-%m-%d-%H-%M-%S")
    copy_name = 'langue-' + time +'[' + str(len) +'].lua'
    shutil.copyfile(out_path + 'langue.lua',langue_save_path + copy_name)
    print("COPY SUCC ,file name :",format(copy_name))


input('FINISH  !!!!')