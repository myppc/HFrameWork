using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace HFrameWork
{
    public class LuaABMgr : SingletonClass<LuaABMgr>
    {
        #region 公共字段
        #endregion

        #region 私有字段
        /// <summary>
        /// lua相对路径对应lua字节组
        /// </summary>
        Dictionary<string, byte[]> _dicLua = new Dictionary<string, byte[]>();

        /// <summary>
        /// 模块列表
        /// </summary>
        List<string> _listModules = new List<string>();
        #endregion

        #region AB方法
        /// <summary>
        /// 加载所有lua的ab
        /// </summary>
        /// <param name="isOnlyBase"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        /// 

        /*************************************
        public void LoadLuaABAsync(bool isOnlyBase, Action callback)
        {
            // 拼接模块
            _listModules.Clear();
            _listModules.Add(Helper.LUA_BASE_MODULE);
            if (!isOnlyBase)
            {
                _listModules.Add(Helper.LUA_MODULE);
            }

            // 加载lua所有模块数据
            var listAB = ResMgr.Ins.LoadModuleABs(_listModules);
            
            // 回调
            callback?.Invoke();
        }

        /// <summary>
        /// 清除lua缓存
        /// </summary>
        /// <param name="isUnloadAllLoadedObjects"></param>
        public void ClearLuaCache(bool isUnloadAllLoadedObjects)
        {
            // 卸载lua所有模块数据
            //ResMgr.Ins.UnloadModuleABs(_listModules);

            // 清空lua读取的缓存
            _dicLua.Clear();
        }

        /// <summary>
        /// 加载ab
        /// </summary>
        /// <param name="abName"></param>
        AssetBundle LoadAB(string abName)
        {
            return ResMgr.Ins.LoadAB(abName);
        }
        ***************************************************/
        #endregion

        #region LUA加载
        /// <summary>
        /// Lua加载回调方法
        /// </summary>
        /// <param name="filepath">lua文件相对路径</param>
        /// <returns></returns>
        public byte[] LuaLoader(ref string filepath)
        {
#if UNITY_EDITOR
            // 是否是编辑器模式
            return LoadLuaFromFile(filepath);
#elif UNITY_STANDALONE_WIN
                return LoadLuaFromAB(filepath);
#elif UNITY_ANDROID
                return LoadLuaFromAB(filepath);
#elif UNITY_IOS
                return LoadLuaFromAB(filepath);
#endif
        }

        /// <summary>
        /// 从ab加载lua
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        byte[] LoadLuaFromFile(string filePath)
        {
            // 是否已经有了
            if (_dicLua.TryGetValue(filePath, out byte[] bytes))
            {
                return bytes;
            }

            // 拼接路径
            string path = Path.Combine(AppConfig.LUA_PATH, $"{filePath}.lua");

            // 读取文件内容
            string content = File.ReadAllText(path);

            // utf8编码
            bytes = Encoding.UTF8.GetBytes(content);

            // 记录
            _dicLua.Add(filePath, bytes);

            return bytes;
        }
        /**********************************************************
        /// <summary>
        /// 从ab加载lua
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        byte[] LoadLuaFromAB(string filePath)
        {
            // 是否已经有了
            if (_dicLua.TryGetValue(filePath, out byte[] bytes))
            {
                return bytes;
            }

            // 获得对应ab名
            string abName = Helper.GetLuaABName(filePath);

            // 获得ab包
            AssetBundle ab = LoadAB(abName);

            // 拼接路径
            string uPath = Helper.FullPath2UPath(Path.Combine(Helper.LUA_PRE_PATH, filePath));

            // 处理后缀
            string extension = Path.GetExtension(uPath);
            if (extension == "")
            {
                uPath = $"{uPath}{Helper.LUA_NEW_EXTENSION}";
            }
            else
            {
                uPath = uPath.Replace(extension, Helper.LUA_NEW_EXTENSION);
            }

            // 获得lua资源
            string txt = ab.LoadAsset<TextAsset>(uPath).text;

            // utf8编码
            bytes = Encoding.UTF8.GetBytes(txt);

            // 记录
            _dicLua.Add(filePath, bytes);

            return bytes;
        }
        ***************************************/
        #endregion

        #region 其他方法
        #endregion
    }
}
