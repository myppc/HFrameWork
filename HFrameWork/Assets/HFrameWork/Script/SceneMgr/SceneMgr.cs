
using Assets.HFrameWork.Script.Res;
using Cysharp.Threading.Tasks;
using HFrameWork.Script.Pool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HFrameWork.Script.SceneMgr
{
    public class SceneMgr:SingletonClass<SceneMgr>
    {
        private static string LoadingName = "LOADING_SCENE";

        private string curSceneName = null;

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="finish"></param>
        /// <param name="progressCallback"></param>
        /// <returns></returns>
        private async UniTask LoadScene(string sceneName, Action finish = null, Action<float> progressCallback = null)
        {
            AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            while (!ao.isDone)
            {
                progressCallback?.Invoke(ao.progress);
                await UniTask.WaitForEndOfFrame();
            }

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            progressCallback?.Invoke(ao.progress);
            finish?.Invoke();
        }

        /// <summary>
        /// 卸载场景
        /// </summary>
        /// <param name="callnameback"></param>
        /// <param name="callback"></param>
        /// <param name="progressCallback"></param>
        /// <returns></returns>
        private async UniTask UnloadScene(string name, Action callback = null, Action<float> progressCallback = null)
        {
            // 卸载
            AsyncOperation ao = SceneManager.UnloadSceneAsync(name);
            
            while (!ao.isDone)
            {
                // 回调
                progressCallback?.Invoke(ao.progress);

                await UniTask.WaitForEndOfFrame();
            }

            // 回调
            progressCallback?.Invoke(ao.progress);
            callback?.Invoke();
        }

        #region 公共方法

        /// <summary>
        /// 打开场景
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="finish"></param>
        /// <param name="progressCallback"></param>
        public void OpenScene(string sceneName,Action finish = null, Action<float> progressCallback = null)
        {
            //如果是切换到Loadscene，就先不清理资源
            if (sceneName != LoadingName)
            {
                //如果需要打开的场景和当前场景一样，就只清理池子中的临时缓存
                if (curSceneName == sceneName)
                {
                    //只清理临时缓存
                    GoPoolManager.Ins.ClearSceneCache(curSceneName, true);
                }
                else
                {
                    //清理预加载Prefab
                    ResMgr.Ins.ClearPreGoCache();
                    //清理场景缓存和当前场景的临时缓存,并且将栈里的场景信息弹出，直到栈顶为当前场景方可停止
                    GoPoolManager.Ins.ClearSceneCache(curSceneName);
                    //清理当前没有用到的AB包
                    AssetsBundleMgr.Ins.UnLoadAllABCache();
                }
            }
            UnloadScene(curSceneName);
            if (sceneName != LoadingName)
            {
                curSceneName = sceneName;
            }
            //加载场景缓存
            GoPoolManager.Ins.LoadCacheByScene(sceneName);
            LoadScene(sceneName, finish, progressCallback);
        }

        #endregion
    }

}
