using UnityEditor;
using System.IO;
using UnityEngine;
using UnityEditor.Build.Pipeline;
using System.Collections.Generic;
using UnityEditor.Build.Pipeline.Interfaces;
using Assets.HFrameWork.Script.Tool;
using Assets.HFrameWork.Script.Res;
using Newtonsoft.Json;
using Assets.HFrameWork.Script.SBP;
using System.Linq;

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
        //扫描资源
        var moduleDict = new Dictionary<string, Dictionary<string,Dictionary<string,AssetsInfo>>>();
        FileHelper.PairAllAssets(AppConfig.ASSETS_PATH, (module,resName, fullPath, uPath, fileName, isScene) =>
        {
            if (!moduleDict.ContainsKey(module))
            {
                moduleDict.Add(module, new Dictionary<string, Dictionary<string,AssetsInfo>>());
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
                foreach(var assetsItem in assetsDict)
                {
                    var assetsInfo = assetsItem.Value;
                    abb.assetNames[count] = assetsInfo.uPath;
                    count++;
                }
                buildList.Add(abb);
            }
        }
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
                foreach(var assetsItem in dict)
                {
                    var assetsInfo = assetsItem.Value;
                    var asset = new ManifestAsset(assetsInfo.abName, assetsInfo.uPath);
                    assetsList.Add(asset);
                }
                manifestData.Add(module, abName, abDetail.FileName, new List<string>(abDetail.Dependencies), assetsList);
            }
            SaveManifest(manifestData, dir + "/manifest.json");
        }
    }


    /// <summary>
    /// 保存manifest文件
    /// </summary>w
    /// <param name="manifest"></param>
    /// <param name="dir"></param>
    static void SaveManifest(Manifest manifest,string dir)
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
                return (BuildTarget.StandaloneWindows64,BuildTargetGroup.Standalone, "StandaloneWindows64");
            case BuildTarget.Android:
                return (BuildTarget.Android, BuildTargetGroup.Android, "Android");
            
        }
        return (BuildTarget.NoTarget, BuildTargetGroup.Unknown, "");
    }
}