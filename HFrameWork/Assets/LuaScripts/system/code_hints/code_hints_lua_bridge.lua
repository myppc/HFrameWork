local code_hints_lua_bridge = {
     GetManifest = function() end ,
     GetVersionStr = function() end ,
     Load = function( eRes, module, assetName ) end ,
     LoadAsync = function( eRes, module, assetName, callback) end ,
     LoadManifest = function() end ,
     ClearPreGoCache = function() end ,
     GetBundle = function( abName) end ,
     SetBundle = function( abName, ab) end ,
     LoadAssetBundle = function( abName) end ,
     LoadAssetBundleAsync = function( abName, finishCallback) end ,
     GetLoader = function( abName) end ,
     SetLoader = function( abName, loader) end ,
     UnLoadAllABCache = function() end ,
     UnloadScene = function( sceneName , finish , progressCallback ) end ,
     LoadScene = function( sceneName, finish , progressCallback ) end ,
     GetRunMode = function() end ,
     FormatStr = function( str, pars) end ,
     GetFileMD5 = function( filePath) end ,
     GetFileCRC = function( filePath) end ,
     MakeSureHasDir = function( path) end ,
     SetSimpleRecord = function( name, value) end ,
     GetSimpleRecord = function( name) end ,
     RemoveSimpleRecord = function( name) end ,
     RemoveAllSimpleRecord = function() end ,
     PoolInit = function() end ,
      RegisterCacheInfo = function( path, name, poolType , cacheCount , sceneName ) end ,
     CreateGoFromCache = function( path, name) end ,
     RecoveryGo = function( go) end ,
     ClearSceneCache = function( sceneName, onlyTemp ) end ,
     LoadCacheByScene = function( sceneName) end ,
     AddTag = function( go, path, name) end ,
     RegUpdate = function( callback) end ,
     UnRegUpdate = function( callback) end ,
     ClearUpdate = function() end ,
     RegLateUpdate = function( callback) end ,
     UnRegLateUpdate = function( callback) end ,
     ClearLateUpdate = function() end ,
     RegFixedUpdate = function( callback) end ,
     UnRegFixedUpdate = function( callback) end ,
     ClearFixedUpdate = function() end ,
     DoInvoke = function( funcName, time) end ,
     DoInvokeRepeating = function( funcName, time, repeatRate) end ,
     DoCancelInvoke = function( funcName ) end ,
     DoLoopCoroutine = function( checkHandler, handler) end ,
     UnityDestroy = function( obj) end ,
}
return code_hints_lua_bridge