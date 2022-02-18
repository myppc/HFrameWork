using Assets.HFrameWork.Script.Res;
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
            if (AppConfig.runMode == ERunMode.Editor)
            {
                return LoadLuaFromFile(filepath);
            }
            else
            {
                return LoadLuaFromAB(filepath);
            }
            
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

            //处理lua文件中require
            if (!filePath.Contains(".lua"))
            {
                filePath = $"{filePath}.lua";
            }
            var fileName = Path.GetFileName(filePath);

        var module = AppConfig.manifest.GetModule(AppConfig.LUA_MODULE);
            var asset = module.GetAsset(fileName);
            var path = asset.path;
            // 读取文件内容
            string content = File.ReadAllText(path);

            // utf8编码
            bytes = Encoding.UTF8.GetBytes(content);

            // 记录
            _dicLua.Add(filePath, bytes);

            return bytes;
        }

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
            // 获得lua资源

            // 处理后缀
            string extension = Path.GetExtension(filePath);
            if (extension == "")
            {
                filePath = $"{filePath}{AppConfig.LUA_NEW_EXTENSION}";
            }
            else
            {
                filePath = filePath.Replace(extension, AppConfig.LUA_NEW_EXTENSION);
            }
            var fileName = Path.GetFileName(filePath);
            var text = ResMgr.Ins.Load(ERes.TextAsset, AppConfig.LUA_MODULE, fileName) as TextAsset;

            // utf8编码
            bytes = Encoding.UTF8.GetBytes(text.text);

            // 记录
            _dicLua.Add(filePath, bytes);

            return bytes;
        }

        #endregion

        #region 其他方法
        #endregion
    }
}
