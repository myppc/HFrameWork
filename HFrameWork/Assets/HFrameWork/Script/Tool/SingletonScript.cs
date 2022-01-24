/*
 * 时间 : 2019/7/5
 * 作者 : LeeroyLin
 * 描述 : 单例脚本基类
 */
using UnityEngine;

/// <summary>
/// 单例脚本基类
/// </summary>
public class SingletonScript<T> : MonoBehaviour where T : class 
{		
	#region 私有字段
    /// <summary>
    /// 单例对象
    /// </summary>
    private static T _instance;
	#endregion
		
	#region 属性
    /// <summary>
    /// 获得单例
    /// </summary>
    public static T Ins
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType(typeof(T)) as T;
            }
            return _instance;
        }
    }
    #endregion

    #region 公共方法
    /// <summary>
    /// 检测实例
    /// </summary>
    /// <returns></returns>
    public static bool CheckIns()
    {
        return _instance != null;
    }
    #endregion
}