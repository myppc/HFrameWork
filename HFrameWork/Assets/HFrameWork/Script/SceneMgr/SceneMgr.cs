
using Assets.HFrameWork.Script.Res;
using Cysharp.Threading.Tasks;
using HFrameWork.Script.Pool;
using System;
using System.Collections;
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
        #region 公共方法
        public async UniTask LoadScene(string name, Action callback, Action<float> progressCallback)
        {
            // 叠加方式加载场景
            AsyncOperation ao = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);

            while (!ao.isDone)
            {
                // 回调
                progressCallback?.Invoke(ao.progress);

                await UniTask.WaitForEndOfFrame();
            }

            // 设置激活
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(name));
            // 回调
            progressCallback?.Invoke(ao.progress);
            callback?.Invoke();
        }

        /// <summary>
        /// 卸载场景
        /// </summary>
        /// <param name="callnameback"></param>
        /// <param name="callback"></param>
        /// <param name="progressCallback"></param>
        /// <returns></returns>
        public async UniTask UnloadScene(string name, Action callback, Action<float> progressCallback)
        {
            if (name == null)
            {
                progressCallback?.Invoke(100f);
                callback?.Invoke();
                return;
            }
            // 卸载
            AsyncOperation ao = SceneManager.UnloadSceneAsync(name);

            while (!ao.isDone)
            {
                Debug.Log(ao.isDone);
                // 回调
                progressCallback?.Invoke(ao.progress);

                await UniTask.WaitForEndOfFrame();
            }
            Resources.UnloadUnusedAssets();
            // 回调
            progressCallback?.Invoke(ao.progress);
            callback?.Invoke();
        }

        /// <summary>
        /// 清理当前场景资源
        /// </summary>
        //public void CleanCurScene(string sceneName)
        //{
        //    //如果是切换到Loadscene，就先不清理资源
        //    if (sceneName != LoadingName)
        //    {
        //        //如果需要打开的场景和当前场景一样，就只清理池子中的临时缓存
        //        if (curSceneName == sceneName)
        //        {
        //            //只清理临时缓存
        //            GoPoolManager.Ins.ClearSceneCache(curSceneName, true);
        //        }
        //        else
        //        {
        //            //清理预加载Prefab
        //            ResMgr.Ins.ClearPreGoCache();
        //            //清理场景缓存和当前场景的临时缓存,并且将栈里的场景信息弹出，直到栈顶为当前场景方可停止
        //            GoPoolManager.Ins.ClearSceneCache(curSceneName);
        //            //清理当前没有用到的AB包
        //            AssetsBundleMgr.Ins.UnLoadAllABCache();
        //        }
        //    }
        //}

        #endregion
    }

}
