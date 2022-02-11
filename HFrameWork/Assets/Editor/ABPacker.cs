using UnityEditor;
using System.IO;
using UnityEngine;
using UnityEditor.Build.Pipeline;
using System.Collections.Generic;
using UnityEditor.Build.Pipeline.Interfaces;
using Newtonsoft.Json;
using Assets.HFrameWork.Script.Tool;
using Assets.HFrameWork.Script.SBP;

public struct AssetsInfo
{
    public string module;
    public string uPath;
    public string fullPath;
    public string fileName;
    public string abName;
    public string resName;
    public bool isScene;
}

public class ABPacker
{
    static BuildTarget packTarget = BuildTarget.StandaloneWindows; //平台target
    static BuildCompression buildCompression = BuildCompression.LZ4; //打包方式


    [MenuItem("AssetsBundle/Build AssetBundles ")]
    static void BuildAllAssetBundles()
    {
        BundleBuildParameters param;
        string dir;
        //生成打包参数
        (param, dir) = CreateHead();
        var moduleDict = new Dictionary<string, Dictionary<string, Dictionary<string, AssetsInfo>>>();
        moduleDict = AddAssetsInfo(moduleDict);
        PreAllLua();
        moduleDict = AddLuaAssetsInfo(moduleDict);

        //生成buildList
        var buildList = LoadBuildList(moduleDict);
        Build(param, buildList, moduleDict, dir);
        // 工程内部目录移出到外部目录
        FileHelper.MergeDirTo(AppConfig.LUA_PRE_PATH, AppConfig.LUA_PRE_BACKUP_PATH, true);

    }


    /// <summary>
    /// 保存manifest文件
    /// </summary>w
    /// <param name="manifest"></param>
    /// <param name="dir"></param>
    static void SaveManifest(Manifest manifest, string dir)
    {
        var jsonInfo = JsonConvert.SerializeObject(manifest);
        File.WriteAllText(dir, jsonInfo);
    }


    /// <summary>
    /// 获取BundleTarget
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    static (BuildTarget, BuildTargetGroup, string) GetBundleBuildParam(BuildTarget target)
    {
        switch (target)
        {
            case BuildTarget.StandaloneWindows:
                return (BuildTarget.StandaloneWindows64, BuildTargetGroup.Standalone, "StandaloneWindows64");
            case BuildTarget.Android:
                return (BuildTarget.Android, BuildTargetGroup.Android, "Android");

        }
        return (BuildTarget.NoTarget, BuildTargetGroup.Unknown, "");
    }
    /// <summary>
    /// 生成打包参数
    /// </summary>
    /// <returns></returns>
    static (BundleBuildParameters, string) CreateHead()
    {
        string platAdd = "standalone";
        BuildTarget buildTarget;
        BuildTargetGroup buildTargetGroup;
        (buildTarget, buildTargetGroup, platAdd) = GetBundleBuildParam(packTarget);
        string dir = Application.streamingAssetsPath + "/AssetBundle/" + platAdd;

        var param = new BundleBuildParameters(buildTarget, buildTargetGroup, dir);
        param.BundleCompression = buildCompression;

        if (Directory.Exists(dir) == false)
        {
            Directory.CreateDirectory(dir);//在工程下创建AssetBundles目录
        }
        return (param, dir);
    }

    /// <summary>
    /// 遍历lua文件
    /// </summary>
    /// <param name="moduleDict"></param>
    /// <returns></returns>
    static Dictionary<string, Dictionary<string, Dictionary<string, AssetsInfo>>> AddLuaAssetsInfo(Dictionary<string, Dictionary<string, Dictionary<string, AssetsInfo>>> moduleDict)
    {
        //扫描lua脚本
        FileHelper.PairLuaScript(AppConfig.LUA_PRE_PATH, (fullPath, uPath, fileName) =>
        {
            var module = AppConfig.LUA_MODULE;
            var res = AppConfig.LUA_RES;
            if (!moduleDict.ContainsKey(module))
            {
                moduleDict.Add(module, new Dictionary<string, Dictionary<string, AssetsInfo>>());
            }
            var resDict = moduleDict[module];
            AssetsInfo assetsInfo;


            assetsInfo.fileName = fileName;
            assetsInfo.abName = $"{module}_{res}";
            assetsInfo.resName = res;
            assetsInfo.fullPath = fullPath;
            assetsInfo.uPath = uPath;
            assetsInfo.isScene = false;
            assetsInfo.module = module;
            if (!resDict.ContainsKey(res))
            {
                resDict.Add(res, new Dictionary<string, AssetsInfo>());
            }
            var assetDict = resDict[res];
            assetDict.Add(fileName, assetsInfo);
        });

        return moduleDict;
    }
    /// <summary>
    /// 生成buildList
    /// </summary>
    /// <param name="moduleDict"></param>
    /// <returns></returns>
    static List<AssetBundleBuild> LoadBuildList(Dictionary<string, Dictionary<string, Dictionary<string, AssetsInfo>>> moduleDict)
    {
        var buildList = new List<AssetBundleBuild>();
        foreach (var moduleItem in moduleDict)
        {
            var resDict = moduleItem.Value;
            foreach (var resItem in resDict)
            {
                var abName = $"{moduleItem.Key}_{resItem.Key}";
                var assetsDict = resItem.Value;
                var abb = new AssetBundleBuild();
                abb.assetBundleName = abName;
                abb.assetNames = new string[assetsDict.Count];
                var count = 0;
                foreach (var assetsItem in assetsDict)
                {
                    var assetsInfo = assetsItem.Value;
                    abb.assetNames[count] = assetsInfo.uPath;
                    count++;
                }
                buildList.Add(abb);
            }
        }
        return buildList;
    }


    static Dictionary<string, Dictionary<string, Dictionary<string, AssetsInfo>>> AddAssetsInfo(Dictionary<string, Dictionary<string, Dictionary<string, AssetsInfo>>> moduleDict)
    {
        //扫描资源
        FileHelper.PairAllAssets(AppConfig.ASSETS_PATH, (module, resName, fullPath, uPath, fileName, isScene) =>
        {
            if (!moduleDict.ContainsKey(module))
            {
                moduleDict.Add(module, new Dictionary<string, Dictionary<string, AssetsInfo>>());
            }
            var resDict = moduleDict[module];
            AssetsInfo assetsInfo;
            assetsInfo.fileName = fileName;
            assetsInfo.abName = $"{module}_{resName}";
            assetsInfo.resName = resName;
            assetsInfo.fullPath = fullPath;
            assetsInfo.uPath = uPath;
            assetsInfo.isScene = isScene;
            assetsInfo.module = module;
            if (!resDict.ContainsKey(resName))
            {
                resDict.Add(resName, new Dictionary<string, AssetsInfo>());
            }
            var assetDict = resDict[resName];
            assetDict.Add(fileName, assetsInfo);
        });

        return moduleDict;
    }

    /// <summary>
    /// 进行打包
    /// </summary>
    /// <param name="param"></param>
    /// <param name="buildList"></param>
    /// <param name="moduleDict"></param>
    /// <param name="dir"></param>
    static void Build(BundleBuildParameters param , List<AssetBundleBuild> buildList, Dictionary<string, Dictionary<string, Dictionary<string, AssetsInfo>>> moduleDict, string dir)
    {
        var content = new BundleBuildContent(buildList);
        ReturnCode exitCode = ContentPipeline.BuildAssetBundles(param, content, out IBundleBuildResults result);
        if (exitCode == ReturnCode.Success)
        {
            var manifestData = new Manifest(new List<int>() { 0, 0, 0 });
            foreach (var item in result.BundleInfos)
            {
                var abName = item.Key;
                var abDetail = item.Value;
                var t = abName.Split('_');
                var module = t[0];
                var resName = t[1];
                var dict = moduleDict[module][resName];
                var assetsList = new List<ManifestAsset>();
                foreach (var assetsItem in dict)
                {
                    var assetsInfo = assetsItem.Value;
                    var asset = new ManifestAsset(assetsInfo.abName, assetsInfo.uPath);
                    assetsList.Add(asset);
                }
                manifestData.Add(module, abName, abDetail.FileName, new List<string>(abDetail.Dependencies), assetsList);
            }
            SaveManifest(manifestData, dir + "/manifest.json");
        }

        Debug.Log($"----------- BUILD FINISH ,RESULT {exitCode}");
    }

    [MenuItem("AssetsBundle/Build LuaScript ")]
    static void BuildLuaScript()
    {
        BundleBuildParameters param;
        string dir;
        //生成打包参数
        (param,dir)  = CreateHead();
        //将lua处理成txt文件
        PreAllLua();
        var moduleDict = new Dictionary<string, Dictionary<string, Dictionary<string, AssetsInfo>>>();
        //添加乱信息
        moduleDict = AddLuaAssetsInfo(moduleDict);
        //生成buildList
        var buildList = LoadBuildList(moduleDict);
        Build(param,buildList,moduleDict,dir);
        // 工程内部目录移出到外部目录
        FileHelper.MergeDirTo(AppConfig.LUA_PRE_PATH, AppConfig.LUA_PRE_BACKUP_PATH, true);
    }


    /// <summary>
    /// lua预处理
    /// </summary>
    static void PreAllLua()
    {
        // 删除工程内部目录
        if (Directory.Exists(AppConfig.LUA_PRE_PATH))
        {
            Directory.Delete(AppConfig.LUA_PRE_PATH, true);
        }
        // 合并外部备份目录到工程内部目录
        FileHelper.MergeDirTo(AppConfig.LUA_PRE_BACKUP_PATH, AppConfig.LUA_PRE_PATH, false);

        // 遍历所有lua资源
        FileHelper.PairLuaScript(AppConfig.LUA_PATH, (fullPath, uPath, fileName) =>
        {


            // 拼接存储路径
            string relativePath = fullPath.Replace(AppConfig.LUA_PATH.Replace(@"\", "/") + "/", "");
            // 是否.开头
            if (relativePath.StartsWith("."))
            {
                return;
            }
            string savePath = Path.Combine(AppConfig.LUA_PRE_PATH, relativePath.Replace(".lua", AppConfig.LUA_NEW_EXTENSION));

            // 删除注释
            FileHelper.ClearLuaCommentary(fullPath, savePath);
        });

        // 刷新
        AssetDatabase.Refresh();
    }


}