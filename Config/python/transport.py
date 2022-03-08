import os
import xlrd

excel_path = "../excel"
out_path = "../out"

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
        start_index = 0 if index == 0 else 3
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
    return keys,desc,types

#进行转换,将数据转换到lua
def transport_datas_to_lua(all_datas):
    pass

def main():
    path_list = scan_files(excel_path)
    all_datas = load_all_excels(path_list)
    transport_datas_to_lua(all_datas)


main()