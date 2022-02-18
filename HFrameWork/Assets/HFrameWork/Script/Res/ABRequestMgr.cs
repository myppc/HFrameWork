using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：Assets.HFrameWork.Script.Res
* 类 名 称 ：ABRequestMgr
* 命名空间 ：Assets.HFrameWork.Script.Res
* 作    者 ：myppc
* 创建时间 ：2022/2/18 18:48:10
* 更新时间 ：2022/2/18 18:48:10
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ myppc 2022. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion
namespace Assets.HFrameWork.Script.Res
{
    public class ABRequestInfo
    {
        public string abName;
        public List<Action<AsyncOperation, string>> onCompleteList;
        public AssetBundleCreateRequest request;

        public ABRequestInfo(string name)
        {
            abName = name;
            onCompleteList = new List<Action<AsyncOperation, string>>();
        }
    }

    public class ABRequestMgr:SingletonClass<ABRequestMgr>
    {
        #region 私有变量
        private Dictionary<string, ABRequestInfo> requestDict;
        #endregion
        #region 公有变量

        #endregion
        #region 构造方法
        public ABRequestMgr()
        {
            requestDict = new Dictionary<string, ABRequestInfo>();
        }

        #endregion
        #region 私有方法
        #endregion
        #region 公有方法
        
        /// <summary>
        /// 尝试获取request
        /// </summary>
        /// <param name="abName"></param>
        /// <returns></returns>
        public AssetBundleCreateRequest TryGetRequest(string abName)
        {
            if(requestDict.TryGetValue(abName, out var item))
                return item.request;
            return null;
        }

        /// <summary>
        /// 添加一个request
        /// </summary>
        public void AddRequest(string abName, AssetBundleCreateRequest request)
        {
            if (request == null)
            {
                return;
            }
            if (requestDict.ContainsKey(abName))
            {
                Debug.LogError("重复添加 request");
            }
            var rqInfo = new ABRequestInfo(abName);
            rqInfo.request = request;
            requestDict.Add(abName, rqInfo);
            request.completed += new Action<AsyncOperation>((operation) => {
                
                AssetBundleCreateRequest request = (AssetBundleCreateRequest)operation;
                if (request.assetBundle != null && request.isDone)
                {
                    var ab = request.assetBundle;
                    AssetsBundleMgr.Ins.SetBundle(abName, ab);
                }

                var callList = requestDict[abName].onCompleteList;
                foreach (var call in callList)
                {
                    call?.Invoke(operation, abName);
                }
                requestDict.Remove(abName);
            });
        }

        /// <summary>
        /// 向request添加一个回调
        /// </summary>
        /// <param name="abName"></param>
        /// <param name="callBack"></param>
        public void RegisterCompleteCallBack(string abName, Action<AsyncOperation, string> callBack)
        {
            if (!requestDict.TryGetValue(abName,out var rqInfo))
            {
                Debug.LogError("添加回调失败");
            }
            rqInfo.onCompleteList.Add(callBack);
        }

        #endregion
    }
}
