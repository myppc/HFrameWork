using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.HFrameWork.Script.SBP
{
    /// <summary>
    /// 资源数据
    /// </summary>
    [Serializable]
    public class ManifestAsset
    {
        #region 公共字段
        /// <summary>
        /// 资源所在的ab包名
        /// </summary>
        public string abName;

        /// <summary>
        /// 资源路径
        /// </summary>
        public string path;
        #endregion

        #region 构造方法
        public ManifestAsset(string abName, string path)
        {
            this.abName = abName;
            this.path = path;
        }
        #endregion
    }
}
