using Assets.HFrameWork.Script.SBP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HFrameWork.Script.Res
{
    /// <summary>
    /// 用于加载指定Ab包资源
    /// </summary>
    public class ABLoader
    {
        #region 静态方法
        /// <summary>
        /// 生成ABLoader
        /// </summary>
        /// <param name="abName"></param>
        /// <returns></returns>
        //public static ABLoader Create(string abName)
        //{

        //}

        #endregion

        #region 私有变量
        private string abName;
        private AssetBundleCreateRequest abr;
        private Action<string,AssetBundle> onFinish;
        private List<string> depends;
        ///是否正在加载当前ab
        private bool isLoading = false;
        #endregion

        #region 公有变量
        public bool isDone;
        #endregion


        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="abName"></param>
        public ABLoader(string abName)
        {
            this.abName = abName;
        }

        #region 私有方法

        private void OnLoadCompleted(AsyncOperation operation,string dependsName)
        {

            foreach(var depend in depends)
            {
                if (AssetsBundleMgr.Ins.GetBundle(depend) == null)
                {
                    return;
                }
            }
            isDone = true;
            isLoading = false;
            onFinish?.Invoke(abName,AssetsBundleMgr.Ins.GetBundle(abName));
        }

        #endregion

        #region 共有方法
        /// <summary>
        /// 异步加载ab包
        /// </summary>
        /// <param name="abName"></param>
        public void LoadAsync(Action<string,AssetBundle> finishCallBack)
        {
            onFinish = finishCallBack;
            var ab = AssetsBundleMgr.Ins.GetBundle(abName);
            if (ab != null)
            {
                finishCallBack?.Invoke(abName,ab);
                return;
            }

            isLoading = true;
            depends = ToolFunc.FilterDepends(abName);
            foreach (var depend in depends)
            {

                var dependData = AssetsBundleMgr.Ins.GetBundle(depend);
                if (dependData != null)
                {
                    continue;
                }
                var dependname = depend;

                var request = ABRequestMgr.Ins.TryGetRequest(dependname);
                if (request == null)
                {
                    request = AssetBundle.LoadFromFileAsync(Path.Combine(AppConfig.AB_LOAD_PATH, dependname));
                    ABRequestMgr.Ins.AddRequest(dependname, request);
                }
                ABRequestMgr.Ins.RegisterCompleteCallBack(dependname, OnLoadCompleted);


            }
        }

        /// <summary>
        /// 同步加载AB包
        /// </summary>
        public AssetBundle Load()
        {
            var ab = AssetsBundleMgr.Ins.GetBundle(abName);
            if (ab != null)
            {
                return ab;
            }
            var depends = ToolFunc.FilterDepends(abName);
            foreach (var depend in depends)
            { 
                ab = AssetBundle.LoadFromFile(Path.Combine(AppConfig.AB_LOAD_PATH, depend));
                if (depend == "lua_script")
                {
                    var names = ab.GetAllAssetNames();
                    var allAssets = ab.LoadAllAssets();
                    var asset = ab.LoadAsset(names[0]);
                }
                AssetsBundleMgr.Ins.SetBundle(depend, ab);
            }
            isDone = true;
            return AssetsBundleMgr.Ins.GetBundle(abName);
        }
        #endregion
    }
}
