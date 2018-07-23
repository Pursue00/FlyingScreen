using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyingScreen.Common.SendMessageHelper
{
    public class EventHub
    {
        private static EventAggregator eventAggregator = new EventAggregator();
        private static EventAggregator serverDataPushEvents = new EventAggregator();
        private static EventAggregator sysEvents = new EventAggregator();

        /// <summary>
        /// Socket事件聚合器
        /// </summary>
        public static IEventAggregator SocketEvents
        {
            get { return eventAggregator; }
        }

        /// <summary>
        /// 服务端主动数据推送事件聚合器
        /// </summary>
        public static IEventAggregator ServerDataPushEvents
        {
            get
            {
                return serverDataPushEvents;
            }
        }

        /// <summary>
        /// 系统内部事件聚合器
        /// </summary>
        public static IEventAggregator SysEvents
        {
            get { return sysEvents; }
        }

        public static void PubNavMsg(NavCmd navCmd)
        {
            SysEvents.PubEvent<NavCmd>(navCmd);
        }
    }
}
