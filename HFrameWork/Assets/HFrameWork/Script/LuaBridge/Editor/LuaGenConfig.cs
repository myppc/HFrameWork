using System.Collections.Generic;
using System;
using UnityEngine;
using XLua;
using System.IO;
using Assets.HFrameWork.Script.SBP;
using Assets.HFrameWork.Script.Res;
using Assets.HFrameWork.Script.Tool;
using HFrameWork.Script.SceneMgr;
using HFrameWork;

public static class LuaGenConfig
{
    //lua中要使用到C#库的配置，比如C#标准库，或者Unity API，第三方库等。
    [LuaCallCSharp]
    public static List<Type> LuaCallCSharp = new List<Type>() {
                typeof(object),
                typeof(UnityEngine.Object),
                typeof(Vector2),
                typeof(Vector3),
                typeof(Vector4),
                typeof(Quaternion),
                typeof(Color),
                typeof(Ray),
                typeof(Bounds),
                typeof(Ray2D),
                typeof(Time),
                typeof(GameObject),
                typeof(Component),
                typeof(Behaviour),
                typeof(Transform),
                typeof(Resources),
                typeof(TextAsset),
                typeof(Keyframe),
                typeof(AnimationCurve),
                typeof(AnimationClip),
                typeof(MonoBehaviour),
                typeof(ParticleSystem),
                typeof(SkinnedMeshRenderer),
                typeof(Renderer),
                typeof(WWW),
                typeof(Light),
                typeof(Mathf),
                typeof(List<int>),
                typeof(Action<int>),
                typeof(Action<string>),
                typeof(Action<float>),
                typeof(Action<bool>),
                typeof(Action<Vector2>),
                typeof(Action<Vector3>),
                typeof(Action<Transform>),
                typeof(Debug),
                typeof(ExtensionHelper),
                typeof(LuaCallCSharpHelper),
                typeof(TickerParam),
                //typeof(AssetsBundleMgr),
                //typeof(Manifest),
                //typeof(ManifestModule),
                //typeof(ManifestAB),
                //typeof(ManifestAsset),
                //typeof(AStar),
                //typeof(FileHelper),
                //typeof(MonoHelper),
                //typeof(SceneMgr),
                //typeof(AppConfig),
                //typeof(GoPoolManager),
                //typeof(ResMgr),
                //typeof(ERes)
            };

    //C#静态调用Lua的配置（包括事件的原型），仅可以配delegate，interface
    [CSharpCallLua]
    public static List<Type> CSharpCallLua = new List<Type>() {
                typeof(Action),
                typeof(Func<double, double, double>),
                typeof(Func<bool>),
                typeof(Func<int>),
                typeof(Func<float>),
                typeof(Func<string>),
                typeof(Action<int>),
                typeof(Action<string>),
                typeof(Action<float>),
                typeof(Action<bool>),
                typeof(Action<int, bool>),
                typeof(Action<Vector2>),
                typeof(Action<Vector3>),
                typeof(Action<Transform, int>),
                typeof(Action<int, int, string>),
                typeof(Func<Transform>),
                typeof(Action<Bounds>),
                typeof(UnityEngine.Events.UnityAction),
                typeof(System.Collections.IEnumerator),
            };

    //黑名单
    [BlackList]
    public static List<List<string>> BlackList = new List<List<string>>()  {
                new List<string>(){"System.Xml.XmlNodeList", "ItemOf"},
                new List<string>(){"UnityEngine.WWW", "movie"},
    #if UNITY_WEBGL
                new List<string>(){"UnityEngine.WWW", "threadPriority"},
    #endif
                new List<string>(){"UnityEngine.Texture2D", "alphaIsTransparency"},
                new List<string>(){"UnityEngine.Security", "GetChainOfTrustValue"},
                new List<string>(){"UnityEngine.CanvasRenderer", "onRequestRebuild"},
                new List<string>(){"UnityEngine.Light", "areaSize"},
                new List<string>(){"UnityEngine.Light", "lightmapBakeType"},
                new List<string>(){"UnityEngine.WWW", "MovieTexture"},
                new List<string>(){"UnityEngine.WWW", "GetMovieTexture"},
                new List<string>(){"UnityEngine.AnimatorOverrideController", "PerformOverrideClipListCleanup"},
    #if !UNITY_WEBPLAYER
                new List<string>(){"UnityEngine.Application", "ExternalEval"},
    #endif
                new List<string>(){"UnityEngine.GameObject", "networkView"}, //4.6.2 not support
                new List<string>(){"UnityEngine.Component", "networkView"},  //4.6.2 not support
                new List<string>(){"System.IO.FileInfo", "GetAccessControl", "System.Security.AccessControl.AccessControlSections"},
                new List<string>(){"System.IO.FileInfo", "SetAccessControl", "System.Security.AccessControl.FileSecurity"},
                new List<string>(){"System.IO.DirectoryInfo", "GetAccessControl", "System.Security.AccessControl.AccessControlSections"},
                new List<string>(){"System.IO.DirectoryInfo", "SetAccessControl", "System.Security.AccessControl.DirectorySecurity"},
                new List<string>(){"System.IO.DirectoryInfo", "CreateSubdirectory", "System.String", "System.Security.AccessControl.DirectorySecurity"},
                new List<string>(){"System.IO.DirectoryInfo", "Create", "System.Security.AccessControl.DirectorySecurity"},
                new List<string>(){"UnityEngine.MonoBehaviour", "runInEditMode"},

                new List<string>(){"UnityEngine.Light", "shadowRadius"},
                new List<string>(){"UnityEngine.Light", "SetLightDirty"},
                new List<string>(){"UnityEngine.Light", "shadowAngle"},
                new List<string>(){"UnityEngine.Light", "shadowAngle"}
            };
}
