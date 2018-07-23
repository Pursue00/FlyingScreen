using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyingScreen.Common.SendMessageHelper
{
    public class AppMessage
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public AppMsg MsgType { get; set; }

        /// <summary>
        /// 消息附带的字符串值
        /// </summary>
        public string Tell { get; set; }

        /// <summary>
        /// 消息中用到是否选中的状态
        /// </summary>
        public bool? IsChecked { get; set; }

        //public AppPage RecPage { get; set; }

        public AppMessage() { }
        public AppMessage(AppMsg msg)
        {
            this.MsgType = msg;
        }

        /// <summary>
        /// 附加对象
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// 分时图拖动浮窗的时候需要将MinChartCalc对象传进来
        /// </summary>
        public object Tag2 { get; set; }


    }
}
