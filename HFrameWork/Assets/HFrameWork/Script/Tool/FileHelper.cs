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
        /// 遍历文件夹下内容
        /// </summary>
        /// <param name="path"></param>
        /// <param name="handler"></param>
        public static void PairAllAssets(string path, Action<string, string, string, string, bool> handler)
        {
            var info = new DirectoryInfo(path);
            DirectoryInfo dirInfo;

            var dirs = info.GetDirectories("*", SearchOption.TopDirectoryOnly);
            foreach (var dir in dirs)
            {
                dirInfo = new DirectoryInfo(dir.FullName);
                string moduleName = dir.Name.ToLower();

                var files = dir.GetFiles("*", SearchOption.AllDirectories);
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

                    // 是否是场景
                    bool isScene = file.Extension == ".unity";
                    handler.Invoke(moduleName, fullPath, uPath, fileName, isScene);
                }
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
    }
}
