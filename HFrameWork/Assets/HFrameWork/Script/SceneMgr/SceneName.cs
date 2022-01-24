using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HFrameWork.Script.SceneMgr
{
    [Serializable]
    public class SceneConfigItem
    {
        public SceneName sceneName;
        public string name;
    }

    [Serializable]
    public class SceneConfig
    {
        Dictionary<int, SceneConfigItem> Configs;
    }

    public enum SceneName
    {
        DEFAULT,
    }
}
