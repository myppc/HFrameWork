
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HFrameWork.Script.SceneMgr
{
    public struct SceneInfo
    {
        public string sceneName;
        public List<object> args;
    }

    public class SceneMgr:SingletonClass<SceneMgr>
    {
        // 场景信息列表
        private Dictionary <string, SceneInfo> sceneInfoDict;
        private List<string> sortList;


        public SceneMgr()
        {
            sortList = new List<string>();
            sceneInfoDict = new Dictionary<string, SceneInfo>();
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="finish"></param>
        /// <param name="progressCallback"></param>
        /// <returns></returns>
        private async UniTask LoadScene(string sceneName, Action finish = null, Action<float> progressCallback = null)
        {

            var succ = sceneInfoDict.TryGetValue(sceneName, out SceneInfo sceneInfo);
            AsyncOperation ao = SceneManager.LoadSceneAsync(sceneInfo.sceneName, LoadSceneMode.Additive);
            while (!ao.isDone)
            {
                progressCallback?.Invoke(ao.progress);
                await UniTask.WaitForEndOfFrame();
            }

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneInfo.sceneName));
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
        public void OpenScene(string sceneName, List<object> args = null,Action finish = null, Action<float> progressCallback = null)
        {

            var index = sortList.FindIndex((item)=> {
                return item == sceneName;
            });
            //当前场景没有存在栈里
            if (index != -1)
            {
                var curSceneName = sortList.Last();
                //如果需要打开的场景和当前场景一样
                if (index == sortList.Count - 1)
                {
                    //只清理临时缓存
                    GoPoolManager.Ins.ClearSceneCache(curSceneName, true);
                    sceneInfoDict.TryGetValue(sceneName, out SceneInfo sceneInfo);
                    sceneInfo.args = args;
                }
                else
                {
                    //清理场景缓存和当前场景的临时缓存,并且将栈里的场景信息弹出，直到栈顶为当前场景方可停止
                    GoPoolManager.Ins.ClearSceneCache(curSceneName);
                    for (var i = sortList.Count - 1; i >= index; i--)
                    {
                        var name = sortList[i];
                        sortList.RemoveAt(i);
                        sceneInfoDict.Remove(name);
                    }
                    //将场景信息压入栈
                    sortList.Add(sceneName);
                    SceneInfo info;
                    info.sceneName = sceneName;
                    info.args = args;
                    sceneInfoDict.Add(sceneName, info);
                    //加载场景缓存
                    GoPoolManager.Ins.LoadCacheByScene(sceneName);

                }
                UnloadScene(curSceneName);
            }
            else 
            {
                //栈内没有相关信息
                sortList.Add(sceneName);
                SceneInfo info;
                info.sceneName = sceneName;
                info.args = args;
                sceneInfoDict.Add(sceneName, info);
                //加载场景缓存
                GoPoolManager.Ins.LoadCacheByScene(sceneName);
            }
            LoadScene(sceneName, finish, progressCallback);
        }

        #endregion
    }

}
