using Assets.HFrameWork.Script.SBP;
using System.Collections.Generic;
using UnityEngine;


public class ToolFunc
{
    /// <summary>
    /// 获取manifestAssets数据
    /// </summary>
    /// <param name="path"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static ManifestAsset GetManifestAsset(string path, string name)
    {
        if (AppConfig.manifest == null)
        {
            return null;
        }

        var manifest = AppConfig.manifest;
        var module = manifest.GetModule(path);
        ManifestAsset ret = module.GetAsset(name);
        return ret;
    }



    /// <summary>
    /// 整理需要加载的依赖项
    /// </summary>
    /// <returns></returns>
    public static List<string> FilterDepends(string abName, List<string> dependsList = null)
    {
        if (dependsList == null)
        {
            dependsList = new List<string>();
            dependsList.Add(abName);
        }
        var modeName = abName.Split('_')[0];
        var module = AppConfig.manifest.GetModule(modeName);
        if (module == null)
        {
            return dependsList;
        }
        var manifestAb = module.GetAB(abName);
        var depends = manifestAb.dependencies;
        foreach (var depend in depends)
        {
            if (!dependsList.Contains(depend))
            {
                dependsList.Add(depend);
                dependsList = FilterDepends(depend, dependsList);
            }
        }
        dependsList.Reverse();
        return dependsList;
    }

}