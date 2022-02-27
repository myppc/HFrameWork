#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class LuaCallCSharpHelperWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(LuaCallCSharpHelper);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 44, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "GetManifest", _m_GetManifest_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetVersionStr", _m_GetVersionStr_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Load", _m_Load_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LoadAsync", _m_LoadAsync_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LoadManifest", _m_LoadManifest_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ClearPreGoCache", _m_ClearPreGoCache_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetBundle", _m_GetBundle_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetBundle", _m_SetBundle_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LoadAssetBundle", _m_LoadAssetBundle_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LoadAssetBundleAsync", _m_LoadAssetBundleAsync_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetLoader", _m_GetLoader_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetLoader", _m_SetLoader_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "OpenScene", _m_OpenScene_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetRunMode", _m_GetRunMode_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "FormatStr", _m_FormatStr_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetFileMD5", _m_GetFileMD5_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetFileCRC", _m_GetFileCRC_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "MakeSureHasDir", _m_MakeSureHasDir_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetSimpleRecord", _m_SetSimpleRecord_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetSimpleRecord", _m_GetSimpleRecord_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RemoveSimpleRecord", _m_RemoveSimpleRecord_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RemoveAllSimpleRecord", _m_RemoveAllSimpleRecord_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "PoolInit", _m_PoolInit_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RegisterCacheInfo", _m_RegisterCacheInfo_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CreateGoFromCache", _m_CreateGoFromCache_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RecoveryGo", _m_RecoveryGo_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ClearSceneCache", _m_ClearSceneCache_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LoadCacheByScene", _m_LoadCacheByScene_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "AddTag", _m_AddTag_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RegUpdate", _m_RegUpdate_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "UnRegUpdate", _m_UnRegUpdate_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ClearUpdate", _m_ClearUpdate_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RegLateUpdate", _m_RegLateUpdate_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "UnRegLateUpdate", _m_UnRegLateUpdate_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ClearLateUpdate", _m_ClearLateUpdate_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RegFixedUpdate", _m_RegFixedUpdate_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "UnRegFixedUpdate", _m_UnRegFixedUpdate_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ClearFixedUpdate", _m_ClearFixedUpdate_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DoInvoke", _m_DoInvoke_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DoInvokeRepeating", _m_DoInvokeRepeating_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DoCancelInvoke", _m_DoCancelInvoke_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DoLoopCoroutine", _m_DoLoopCoroutine_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "UnityDestroy", _m_UnityDestroy_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new LuaCallCSharpHelper();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to LuaCallCSharpHelper constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetManifest_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    
                        var gen_ret = LuaCallCSharpHelper.GetManifest(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetVersionStr_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                        var gen_ret = LuaCallCSharpHelper.GetVersionStr(  );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Load_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    int _eRes = LuaAPI.xlua_tointeger(L, 1);
                    string _module = LuaAPI.lua_tostring(L, 2);
                    string _assetName = LuaAPI.lua_tostring(L, 3);
                    
                        var gen_ret = LuaCallCSharpHelper.Load( _eRes, _module, _assetName );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadAsync_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    int _eRes = LuaAPI.xlua_tointeger(L, 1);
                    string _module = LuaAPI.lua_tostring(L, 2);
                    string _assetName = LuaAPI.lua_tostring(L, 3);
                    System.Action<UnityEngine.Object> _callback = translator.GetDelegate<System.Action<UnityEngine.Object>>(L, 4);
                    
                    LuaCallCSharpHelper.LoadAsync( _eRes, _module, _assetName, _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadManifest_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    LuaCallCSharpHelper.LoadManifest(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ClearPreGoCache_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    LuaCallCSharpHelper.ClearPreGoCache(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetBundle_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _abName = LuaAPI.lua_tostring(L, 1);
                    
                        var gen_ret = LuaCallCSharpHelper.GetBundle( _abName );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetBundle_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _abName = LuaAPI.lua_tostring(L, 1);
                    UnityEngine.AssetBundle _ab = (UnityEngine.AssetBundle)translator.GetObject(L, 2, typeof(UnityEngine.AssetBundle));
                    
                    LuaCallCSharpHelper.SetBundle( _abName, _ab );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadAssetBundle_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _abName = LuaAPI.lua_tostring(L, 1);
                    
                        var gen_ret = LuaCallCSharpHelper.LoadAssetBundle( _abName );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadAssetBundleAsync_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _abName = LuaAPI.lua_tostring(L, 1);
                    System.Action<string, UnityEngine.AssetBundle> _finishCallback = translator.GetDelegate<System.Action<string, UnityEngine.AssetBundle>>(L, 2);
                    
                    LuaCallCSharpHelper.LoadAssetBundleAsync( _abName, _finishCallback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetLoader_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _abName = LuaAPI.lua_tostring(L, 1);
                    
                        var gen_ret = LuaCallCSharpHelper.GetLoader( _abName );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetLoader_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _abName = LuaAPI.lua_tostring(L, 1);
                    Assets.HFrameWork.Script.Res.ABLoader _loader = (Assets.HFrameWork.Script.Res.ABLoader)translator.GetObject(L, 2, typeof(Assets.HFrameWork.Script.Res.ABLoader));
                    
                    LuaCallCSharpHelper.SetLoader( _abName, _loader );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OpenScene_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action>(L, 2)&& translator.Assignable<System.Action<float>>(L, 3)) 
                {
                    string _sceneName = LuaAPI.lua_tostring(L, 1);
                    System.Action _finish = translator.GetDelegate<System.Action>(L, 2);
                    System.Action<float> _progressCallback = translator.GetDelegate<System.Action<float>>(L, 3);
                    
                    LuaCallCSharpHelper.OpenScene( _sceneName, _finish, _progressCallback );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action>(L, 2)) 
                {
                    string _sceneName = LuaAPI.lua_tostring(L, 1);
                    System.Action _finish = translator.GetDelegate<System.Action>(L, 2);
                    
                    LuaCallCSharpHelper.OpenScene( _sceneName, _finish );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 1&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)) 
                {
                    string _sceneName = LuaAPI.lua_tostring(L, 1);
                    
                    LuaCallCSharpHelper.OpenScene( _sceneName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to LuaCallCSharpHelper.OpenScene!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetRunMode_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                        var gen_ret = LuaCallCSharpHelper.GetRunMode(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_FormatStr_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _str = LuaAPI.lua_tostring(L, 1);
                    string[] _pars = translator.GetParams<string>(L, 2);
                    
                        var gen_ret = LuaCallCSharpHelper.FormatStr( _str, _pars );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetFileMD5_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _filePath = LuaAPI.lua_tostring(L, 1);
                    
                        var gen_ret = LuaCallCSharpHelper.GetFileMD5( _filePath );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetFileCRC_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _filePath = LuaAPI.lua_tostring(L, 1);
                    
                        var gen_ret = LuaCallCSharpHelper.GetFileCRC( _filePath );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_MakeSureHasDir_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _path = LuaAPI.lua_tostring(L, 1);
                    
                    LuaCallCSharpHelper.MakeSureHasDir( _path );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetSimpleRecord_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _name = LuaAPI.lua_tostring(L, 1);
                    string _value = LuaAPI.lua_tostring(L, 2);
                    
                    LuaCallCSharpHelper.SetSimpleRecord( _name, _value );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSimpleRecord_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _name = LuaAPI.lua_tostring(L, 1);
                    
                        var gen_ret = LuaCallCSharpHelper.GetSimpleRecord( _name );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveSimpleRecord_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _name = LuaAPI.lua_tostring(L, 1);
                    
                    LuaCallCSharpHelper.RemoveSimpleRecord( _name );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveAllSimpleRecord_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    LuaCallCSharpHelper.RemoveAllSimpleRecord(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PoolInit_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    LuaCallCSharpHelper.PoolInit(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RegisterCacheInfo_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 5&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& (LuaAPI.lua_isnil(L, 5) || LuaAPI.lua_type(L, 5) == LuaTypes.LUA_TSTRING)) 
                {
                    string _path = LuaAPI.lua_tostring(L, 1);
                    string _name = LuaAPI.lua_tostring(L, 2);
                    int _poolType = LuaAPI.xlua_tointeger(L, 3);
                    int _cacheCount = LuaAPI.xlua_tointeger(L, 4);
                    string _sceneName = LuaAPI.lua_tostring(L, 5);
                    
                    LuaCallCSharpHelper.RegisterCacheInfo( _path, _name, _poolType, _cacheCount, _sceneName );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    string _path = LuaAPI.lua_tostring(L, 1);
                    string _name = LuaAPI.lua_tostring(L, 2);
                    int _poolType = LuaAPI.xlua_tointeger(L, 3);
                    int _cacheCount = LuaAPI.xlua_tointeger(L, 4);
                    
                    LuaCallCSharpHelper.RegisterCacheInfo( _path, _name, _poolType, _cacheCount );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    string _path = LuaAPI.lua_tostring(L, 1);
                    string _name = LuaAPI.lua_tostring(L, 2);
                    int _poolType = LuaAPI.xlua_tointeger(L, 3);
                    
                    LuaCallCSharpHelper.RegisterCacheInfo( _path, _name, _poolType );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _path = LuaAPI.lua_tostring(L, 1);
                    string _name = LuaAPI.lua_tostring(L, 2);
                    
                    LuaCallCSharpHelper.RegisterCacheInfo( _path, _name );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to LuaCallCSharpHelper.RegisterCacheInfo!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateGoFromCache_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _path = LuaAPI.lua_tostring(L, 1);
                    string _name = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = LuaCallCSharpHelper.CreateGoFromCache( _path, _name );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RecoveryGo_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.GameObject _go = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    
                    LuaCallCSharpHelper.RecoveryGo( _go );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ClearSceneCache_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2)) 
                {
                    string _sceneName = LuaAPI.lua_tostring(L, 1);
                    bool _onlyTemp = LuaAPI.lua_toboolean(L, 2);
                    
                    LuaCallCSharpHelper.ClearSceneCache( _sceneName, _onlyTemp );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 1&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)) 
                {
                    string _sceneName = LuaAPI.lua_tostring(L, 1);
                    
                    LuaCallCSharpHelper.ClearSceneCache( _sceneName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to LuaCallCSharpHelper.ClearSceneCache!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadCacheByScene_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _sceneName = LuaAPI.lua_tostring(L, 1);
                    
                    LuaCallCSharpHelper.LoadCacheByScene( _sceneName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddTag_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.GameObject _go = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    string _path = LuaAPI.lua_tostring(L, 2);
                    string _name = LuaAPI.lua_tostring(L, 3);
                    
                    LuaCallCSharpHelper.AddTag( _go, _path, _name );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RegUpdate_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.Action _callback = translator.GetDelegate<System.Action>(L, 1);
                    
                    LuaCallCSharpHelper.RegUpdate( _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnRegUpdate_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.Action _callback = translator.GetDelegate<System.Action>(L, 1);
                    
                    LuaCallCSharpHelper.UnRegUpdate( _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ClearUpdate_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    LuaCallCSharpHelper.ClearUpdate(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RegLateUpdate_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.Action _callback = translator.GetDelegate<System.Action>(L, 1);
                    
                    LuaCallCSharpHelper.RegLateUpdate( _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnRegLateUpdate_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.Action _callback = translator.GetDelegate<System.Action>(L, 1);
                    
                    LuaCallCSharpHelper.UnRegLateUpdate( _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ClearLateUpdate_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    LuaCallCSharpHelper.ClearLateUpdate(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RegFixedUpdate_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.Action _callback = translator.GetDelegate<System.Action>(L, 1);
                    
                    LuaCallCSharpHelper.RegFixedUpdate( _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnRegFixedUpdate_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.Action _callback = translator.GetDelegate<System.Action>(L, 1);
                    
                    LuaCallCSharpHelper.UnRegFixedUpdate( _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ClearFixedUpdate_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    LuaCallCSharpHelper.ClearFixedUpdate(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DoInvoke_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _funcName = LuaAPI.lua_tostring(L, 1);
                    float _time = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    LuaCallCSharpHelper.DoInvoke( _funcName, _time );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DoInvokeRepeating_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _funcName = LuaAPI.lua_tostring(L, 1);
                    float _time = (float)LuaAPI.lua_tonumber(L, 2);
                    float _repeatRate = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    LuaCallCSharpHelper.DoInvokeRepeating( _funcName, _time, _repeatRate );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DoCancelInvoke_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)) 
                {
                    string _funcName = LuaAPI.lua_tostring(L, 1);
                    
                    LuaCallCSharpHelper.DoCancelInvoke( _funcName );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 0) 
                {
                    
                    LuaCallCSharpHelper.DoCancelInvoke(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to LuaCallCSharpHelper.DoCancelInvoke!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DoLoopCoroutine_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.Func<bool> _checkHandler = translator.GetDelegate<System.Func<bool>>(L, 1);
                    System.Func<bool> _handler = translator.GetDelegate<System.Func<bool>>(L, 2);
                    
                    LuaCallCSharpHelper.DoLoopCoroutine( _checkHandler, _handler );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnityDestroy_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Object _obj = (UnityEngine.Object)translator.GetObject(L, 1, typeof(UnityEngine.Object));
                    
                    LuaCallCSharpHelper.UnityDestroy( _obj );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
