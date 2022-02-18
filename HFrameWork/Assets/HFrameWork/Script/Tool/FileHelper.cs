using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HFrameWork.Script.Tool
{
    public static class FileHelper
    {
        /// <summary>
        /// 遍历资源文件夹下内容
        /// </summary>
        /// <param name="path"></param>
        /// <param name="handler"></param>
        public static void PairAllAssets(string path, Action<string, string, string, string, string, bool> handler)
        {
            var info = new DirectoryInfo(path);
            DirectoryInfo dirInfo;

            var dirs = info.GetDirectories("*", SearchOption.TopDirectoryOnly);
            foreach (var dir in dirs) ///模块列表
            {
                dirInfo = new DirectoryInfo(dir.FullName);
                string moduleName = dir.Name.ToLower();

                var resDirs = dirInfo.GetDirectories("*", SearchOption.TopDirectoryOnly); //资源文件夹目录列表
                foreach (var resDir in resDirs)
                {
                    string resName = resDir.Name.ToLower();
                    var files = resDir.GetFiles("*", SearchOption.AllDirectories);
                    foreach (var file in files)
                    {
                        if (file.Extension == ".meta")
                        {
                            continue;
                        }

                        // 获得路径
                        string fullPath = file.FullName.Replace(@"\", "/");
                        string uPath = fullPath.Replace(Application.dataPath, "Assets");

                        // 获得文件名
                        string fileName = Path.GetFileName(uPath);
                        //fileName = fileName.Replace(file.Extension, "");

                        // 是否是场景
                        bool isScene = file.Extension == ".unity";
                        handler.Invoke(moduleName, resName, fullPath, uPath, fileName, isScene);
                    }
                }
            }
        }

        /// <summary>
        /// 遍历lua脚本文件夹
        /// </summary>
        /// <param name="path"></param>
        /// <param name="handler"></param>
        public static void PairLuaScript(string path, Action<string, string, string> handler)
        {
            //fullPath, uPath, fileName

            var info = new DirectoryInfo(path);
            DirectoryInfo dirInfo;

            var files = info.GetFiles("*", SearchOption.AllDirectories);
            foreach (var file in files) ///模块列表
            {
                if (file.Extension == ".meta")
                {
                    continue;
                }
                // 获得路径
                string fullPath = file.FullName.Replace(@"\", "/");
                string uPath = fullPath.Replace(Application.dataPath, "Assets");
                // 获得文件名
                string fileName = Path.GetFileName(uPath);
                handler.Invoke(fullPath, uPath, fileName);
            }
        }


        /// <summary>
        /// 获得文件的md5值
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileMD5(string filePath)
        {
            try
            {
                FileStream fs = new FileStream(filePath, FileMode.Open);
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(fs);
                fs.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2")); //x2:以16进制转换
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                Debug.LogError("[GetFileMD5] failed. Error:" + ex.Message);
                return "";
            }
        }

        /// <summary>
        /// 获得文件的crc值
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileCRC(string filePath)
        {
            return File2CRC32.GetFileCRC32(filePath);
        }

        /// <summary>
        /// 合并目录
        /// </summary>
        /// <param name="sourceDir"></param>
        /// <param name="targetDir"></param>
        /// <param name="isDeleteSourceDir">是否清除原目录</param>
        public static void MergeDirTo(string sourceDir, string targetDir, bool isDeleteSourceDir)
        {
            // 打开原目录
            MakeSureHasDir(sourceDir);
            DirectoryInfo infoSource = new DirectoryInfo(sourceDir);

            // 获得原目录所有文件
            var files = infoSource.GetFiles("*", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                // 获取文件信息
                FileInfo info = new FileInfo(file.FullName);

                // 拼接新路径
                string newPath = file.FullName.Replace(sourceDir.Replace("/", "\\"), targetDir);

                // 确保有新目录
                string dir = Path.GetDirectoryName(newPath);
                MakeSureHasDir(dir);

                // 移动文件
                info.MoveTo(newPath);
            }
        }

        /// <summary>
        /// 确保有目录
        /// </summary>
        public static void MakeSureHasDir(string path)
        {
            // 是否没有该目录
            if (!Directory.Exists(path))
            {
                // 创建目录
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// 清除Lua注释
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="savePath"></param>
        public static void ClearLuaCommentary(string fullPath, string savePath)
        {
            // 以行读取
            var lines = File.ReadAllLines(fullPath);

            // 是否没内容
            if (lines.Length == 0)
            {
                return;
            }

            // 新的每一行
            List<string> newLines = new List<string>();

            // 标记是否在引号内
            char c = default;
            char lastC = default;
            char last2C = default;
            char last3C = default;
            char lastQuotation = default;
            bool isInQuo = false;
            bool isInBigQuo = false;
            bool isHasAvailableChar = false;        // 是否有有效字符 非空
            int startIdx;
            int endIdx;
            bool isNeedChange = false;      // 是否需要转义

            // 遍历每一行
            string line;
            for (int i = 0; i < lines.Length; i++)
            {
                // 获得该行数据
                line = lines[i];

                isHasAvailableChar = false;
                startIdx = -1;
                endIdx = line.Length - 1;
                lastC = default;
                last2C = default;
                last3C = default;
                isNeedChange = false;

                // 遍历每个字符
                for (int t = 0; t < line.Length; t++)
                {
                    // 获得字符
                    c = line[t];

                    // 是否在大注释中
                    if (isInBigQuo)
                    {
                        // 是否结束大注释
                        if (t >= 1)
                        {
                            if (c == ']' && lastC == ']')
                            {
                                startIdx = t;
                                isInBigQuo = false;
                            }
                        }
                    }
                    else
                    {
                        // 是否没在引号中
                        if (!isInQuo)
                        {
                            // 是否是引号
                            if (c == '\"' || c == '\'')
                            {
                                // 记录且标记
                                lastQuotation = c;
                                isInQuo = true;
                            }
                        }
                        // 在引号中
                        else
                        {
                            // 是否与之前引号相同
                            if (c == lastQuotation)
                            {
                                // 之前不是转义
                                if (!isNeedChange)
                                {
                                    // 记录且标记
                                    lastQuotation = default;
                                    isInQuo = false;
                                }
                            }

                            // 是否转义
                            if (isNeedChange)
                            {
                                isNeedChange = false;
                            }

                            // 是否是转义符
                            if (c == '\\')
                            {
                                isNeedChange = true;
                            }
                        }

                        // 是否没在引号中
                        if (!isInQuo)
                        {
                            // 是否是注释
                            if (c == '-' && lastC == '-')
                            {
                                // 记录结束位置
                                endIdx = t - 2;

                                // 是否是大注释
                                if (t < line.Length - 2)
                                {
                                    if (line[t + 1] == '[' && line[t + 2] == '[')
                                    {
                                        // 标记
                                        isInBigQuo = true;
                                    }
                                }

                                break;
                            }
                        }

                        // 是否还没有非空字符
                        if (!isHasAvailableChar)
                        {
                            // 是否字符非空
                            if (c != ' ' && c != '\t' && c != '\n')
                            {
                                // 标记
                                isHasAvailableChar = true;

                                // 记录非空下标
                                startIdx = t;
                            }
                            // 或在引号中
                            else if (isInQuo)
                            {
                                // 标记
                                isHasAvailableChar = true;

                                // 记录非空下标
                                startIdx = t;
                            }
                        }
                    }

                    // 记录
                    last3C = last2C;
                    last2C = lastC;
                    lastC = c;
                }

                // 是否有效
                if (endIdx >= startIdx && startIdx >= 0)
                {
                    // 添加新行
                    newLines.Add(line.Substring(startIdx, endIdx - startIdx + 1));
                }
                else
                {
                    // 添加空行 确保报错提示里显示的行号是正确的
                    newLines.Add("");
                }
            }

            // 确保有目录
            string dir = Path.GetDirectoryName(savePath);
            FileHelper.MakeSureHasDir(dir);

            // 存储新lua
            File.WriteAllLines(savePath, newLines);
        }


        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ReadFile(string filePath)
        {
            string content = "";

            // 是否没有文件
            if (!File.Exists(filePath))
            {
                return content;
            }

            try
            {
                // 读取内容
                content = File.ReadAllText(filePath);
            }
            catch (Exception e)
            {
                Debug.LogError($"[C#][UpdateMgr][ReadFile][Error to read '{filePath}' : {e.Message}]");
            }

            return content;
        }
    }
}
