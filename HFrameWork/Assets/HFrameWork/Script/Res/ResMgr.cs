using Assets.HFrameWork.Script.SBP;
using Assets.HFrameWork.Script.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.HFrameWork.Script.Res
{
    public class ResMgr :SingletonClass<ResMgr>
    {
        public Manifest CreateLoacalManifest()
        {
            Manifest manifest = null;
#if UNITY_EDITOR
            manifest = new Manifest(new List<int>() { 0, 0, 0 });

            // 遍历所有资源文件
            FileHelper.PairAllAssets(AppConfig.ASSETS_PATH, (module, fullPath, uPath, fileName, isScene) =>
            {
                // 确保有模块
                if (!manifest.modules.TryGetValue(module, out ManifestModule moduleData))
                {
                    moduleData = new ManifestModule(new Dictionary<string, ManifestAB>(), new Dictionary<string, ManifestAsset>());
                    manifest.modules.Add(module, moduleData);
                }

                // 记录资源
                moduleData.AddAsset(fileName, new ManifestAsset("", uPath));
            });
#endif
            return manifest;
        }
    }
}
