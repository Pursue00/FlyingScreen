using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyingScreen.Common.SendMessageHelper
{
    /// <summary>
    /// 导航消息
    /// </summary>
    public class NavCmd
    {
        //public AppPage Source { get; set; }
        //public AppPage Target { get; set; }
        //public StockBaseInfo Stock { get; set; }

        /// <summary>
        /// 跳转消息
        /// </summary>
        public string JumpMsg { get; set; }

        /// <summary>
        /// 记录首页个股页面跳转信息
        /// </summary>
        public string RecordMsg { get; set; }

        /// <summary>
        /// 导航命令附带对象参数
        /// </summary>
        public Object Tag { get; set; }

    }
}
