using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.HFrameWork.Script.SBP
{
    /// <summary>
    /// ab配置数据
    /// </summary>
    [Serializable]
    public class ManifestAB
    {
        #region 不序列化
        /// <summary>
        /// 该ab的hash值
        /// </summary>
        public string hash;

        /// <summary>
        /// 该ab的crc值
        /// </summary>
        public string crc;

        /// <summary>
        /// 该ab依赖其他ab的列表
        /// </summary>
        public List<string> dependencies;

        /// <summary>
        /// 文件大小
        /// </summary>
        public long size;
        #endregion

        #region 构造方法
        public ManifestAB() : base()
        {

        }
        public ManifestAB(string hash, string crc, List<string> dependencies, long size) : base()
        {
            this.hash = hash;
            this.crc = crc;
            this.dependencies = dependencies;
            this.size = size;
        }
        #endregion

        #region 公共方法
        #endregion
    }
}
