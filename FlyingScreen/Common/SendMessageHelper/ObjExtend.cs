using Prism.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Xml.Serialization;

namespace FlyingScreen.Common.SendMessageHelper
{
    public static class ObjExtend
    {

        public static bool IsAcrossWeek(this DateTime date1, DateTime date2)
        {
            if (Math.Abs((date2 - date1).TotalDays) > 6) return true;
            if (date1.DayOfWeek >= date2.DayOfWeek) return true;
            return false;
        }

        public static bool IsAcrossMonth(this DateTime date1, DateTime date2)
        {
            if (date1.Year != date2.Year) return true;
            if (date1.Month != date2.Month) return true;
            return false;
        }


        public static void RefreshAtOnce(this FrameworkElement element)
        {
            var frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Send,
                new DispatcherOperationCallback(ExitFrames), frame);
            try
            {
                Dispatcher.PushFrame(frame);
            }
            catch (InvalidOperationException)
            {
            }
        }
        private static object ExitFrames(object frame)
        {
            ((DispatcherFrame)frame).Continue = false;
            return null;
        }

        //public static void DoEvents(this FrameworkElement element)
        //{
        //    DispatcherFrame frame = new DispatcherFrame(true);
        //    Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate (object arg)
        //    {
        //        DispatcherFrame fr = arg as DispatcherFrame;
        //        fr.Continue = false;
        //    }, frame);
        //    Dispatcher.PushFrame(frame);
        //}

        public static string ToStockCode(this int data)
        {
            return string.Format("{0}", data % 1000000).PadLeft(6, '0');
        }

        //public static MarketType ToMarketType(this int data)
        //{
        //    return (MarketType)(data / 1000000);
        //}

        public static DateTime UnixTimeToLocalTime(this int dateTime)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0);
            return start.AddSeconds(dateTime);
        }

        public static int ToUnixTime(this DateTime dateTime)
        {
            return (int)(dateTime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
        }

        public static long ToLong(this DateTime dateTime)
        {
            return Convert.ToInt64(dateTime.ToString("yyyyMMddHHmmss"));
        }

        public static DateTime ToYMDTime(this int time)
        {
            try
            {
                int year = time / 10000;
                int mouth = (time - year * 10000) / 100;
                int day = (time - year * 10000 - mouth * 100);
                DateTime dt = new DateTime(year, mouth, day);
                return dt;
            }
            catch (Exception)
            {

                return DateTime.Now;
            }

            //dt.Year= time/
        }

        public static DateTime ToHMSTodayTime(this int time)
        {
            DateTime dt = DateTime.Now;
            int hour = time / 10000;
            int min = (time - hour * 10000) / 100;
            int sec = (time - hour * 10000 - min * 100);
            return new DateTime(dt.Year, dt.Month, dt.Day, hour, min, sec);
        }

        /// <summary>
        /// YYYYMMDD
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static int DateToInt(this DateTime dateTime)
        {
            return dateTime.Year * 10000 + dateTime.Month * 100 + dateTime.Day;
        }

        /// <summary>
        /// HHMM
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static int TimeToInt(this DateTime dateTime)
        {
            return dateTime.Hour * 100 + dateTime.Minute;
        }

        //public static PageGroup GetGroup(this AppPage appPage)
        //{
        //    var type = typeof(AppPage);
        //    var temp = type.GetMember("PageGroup");
        //    var obj = Attribute.GetCustomAttribute(temp[0], typeof(PageGroupAttribute));
        //    var info = obj as PageGroup;
        //    return info;
        //    //var temp= typeof(AppPage);
        //    // temp.GetCustomAttribute(temp);
        //    // Attribute.GetCustomAttribute()
        //}

        //public static PageGroup GetGroup(this AppPage @this)
        //{
        //    var name = @this.ToString();
        //    var field = @this.GetType().GetField(name);
        //    if (field == null) return PageGroup.None;
        //    var att = System.Attribute.GetCustomAttribute(field, typeof(PageGroupAttribute), false);
        //    return att == null ? PageGroup.None : ((PageGroupAttribute)att).PageGroup;
        //}

        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventAggregator"></param>
        /// <param name="action"></param>
        public static void SubEvent<T>(this Prism.Events.IEventAggregator eventAggregator, Action<T> action)
        {
            eventAggregator.GetEvent<Prism.Events.PubSubEvent<T>>().Subscribe(action);
        }

        public static void SubEvent<T>(this Prism.Events.IEventAggregator eventAggregator, Action<T> action, ThreadOption threadOption)
        {
            eventAggregator.GetEvent<Prism.Events.PubSubEvent<T>>().Subscribe(action, threadOption, false);
        }

        /// <summary>
        /// 发布事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventAggregator"></param>
        /// <param name="obj"></param>
        public static void PubEvent<T>(this Prism.Events.IEventAggregator eventAggregator, T obj)
        {
            eventAggregator.GetEvent<Prism.Events.PubSubEvent<T>>().Publish(obj);
        }

        /// <summary>
        /// 注销订阅事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventAggregator"></param>
        /// <param name="action"></param>
        public static void UnSubEvent<T>(this Prism.Events.IEventAggregator eventAggregator, Action<T> action)
        {
            eventAggregator.GetEvent<Prism.Events.PubSubEvent<T>>().Unsubscribe(action);
        }

        public static string FieldsToString(this object obj)
        {
            var te = ">k__BackingField".ToCharArray();
            if (obj == null) return string.Empty;
            var arr = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            string ret = string.Empty;
            foreach (var item in arr)
            {
                var temp = item.GetValue(obj);
                var str = temp == null ? string.Empty : temp.ToString();
                var ss = item.Name.TrimStart('<');
                ss = ss.Remove(ss.Length - te.Length, te.Length);
                ret += string.Format(string.Format("{0}:{1}  ", ss, str));
            }
            return ret;


        }

        /// <summary>
        /// 格式化成价格字符串，统一保留2位小数。
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ToPriceString(this double data)
        {
            return data.ToString("F2");
        }

        /// <summary>
        /// 格式化 金额 自动匹配 万 、亿 单位 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ToAomString(this double data)
        {

            var temp = Math.Abs(data);
            var ret = data > 0 ? string.Empty : "-";
            if (temp > 100000000)
            {
                return ret + (temp / 100000000d).ToString("F2") + "亿";
            }
            if (data > 10000)
            {
                return ret + (temp / 10000).ToString("F2") + "万";
            }
            return ret + temp.ToString("F2");
        }

        public static string ToVolString(this long data)
        {
            if (data > 100000000)
            {
                return (data / 100000000d).ToString("F2") + "亿";
            }
            if (data > 10000)
            {
                return (data / 10000d).ToString("F2") + "万";
            }
            return data.ToString("F2");
        }

        public static string ToVolIntString(this double data)
        {
            if (data > 100000000)
            {
                return Math.Round(data / 100000000d).ToString() + "亿";
            }
            if (data > 10000)
            {
                return Math.Round(data / 10000d).ToString() + "万";
            }
            return Math.Round(Convert.ToDouble(data)).ToString();
        }

        public static string ToVolString(this int data)
        {
            if (data > 100000000)
            {
                return (data / 100000000d).ToString("F2") + "亿";
            }
            if (data > 10000)
            {
                return (data / 10000d).ToString("F2") + "万";
            }
            return data.ToString();
        }

        /// <summary>
        /// 将对象序列化为二进制数据 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] SerializeToBinary(object obj)
        {
            MemoryStream stream = new MemoryStream();
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(stream, obj);

            byte[] data = stream.ToArray();
            stream.Close();

            return data;
        }

        /// <summary>
        /// 将对象序列化为XML数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] SerializeToXml(object obj)
        {
            MemoryStream stream = new MemoryStream();
            XmlSerializer xs = new XmlSerializer(obj.GetType());
            xs.Serialize(stream, obj);

            byte[] data = stream.ToArray();
            stream.Close();

            return data;
        }

        /// <summary>
        /// 将二进制数据反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static object DeserializeWithBinary(byte[] data)
        {
            MemoryStream stream = new MemoryStream();
            stream.Write(data, 0, data.Length);
            stream.Position = 0;
            BinaryFormatter bf = new BinaryFormatter();
            object obj = bf.Deserialize(stream);

            stream.Close();

            return obj;
        }

        /// <summary>
        /// 将二进制数据反序列化为指定类型对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T DeserializeWithBinary<T>(byte[] data)
        {
            return (T)DeserializeWithBinary(data);
        }


        public static T DeepClone<T>(this object obj)
        {
            var datas = SerializeToBinary(obj);
            return DeserializeWithBinary<T>(datas);
        }

        //public static System.Drawing.Pen ToGdiPen(this Pen pen)
        //{
        //    var color = (pen.Brush as SolidColorBrush).Color.ToGDIColor();
        //    System.Drawing.Pen p = new System.Drawing.Pen(color, 1.0f);
        //    if (pen.DashStyle == DashStyles.Dot)
        //    {
        //        p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
        //    }
        //    p.Alignment = System.Drawing.Drawing2D.PenAlignment.Center;
        //    return p;
        //}

        //public static GDI.Brush ToGdiBrush(this Brush brush)
        //{
        //    var color = (brush as SolidColorBrush).Color.ToGDIColor();
        //    return new GDI.SolidBrush(color);
        //}


        //public static System.Drawing.Color ToGDIColor(this Color color)
        //{
        //    return System.Drawing.Color.FromArgb(color.R, color.G, color.B);
        //}

        //public static GDI.PointF ToGdiPoint(this Point point)
        //{
        //    return new GDI.PointF((float)point.X, (float)point.Y);
        //}


        public static bool IsEqual(this float f1, float f2)
        {
            return Math.Abs(f1 - f2) < 0.00001;
        }

        public static void DoEvents(this FrameworkElement frameworkElement)
        {
            DispatcherFrame nestedFrame = new DispatcherFrame();
            DispatcherOperation exitOperation = Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, exitFrameCallback, nestedFrame);
            Dispatcher.PushFrame(nestedFrame);
            if (exitOperation.Status !=
            DispatcherOperationStatus.Completed)
            {
                exitOperation.Abort();
            }
        }

        private static DispatcherOperationCallback exitFrameCallback = new DispatcherOperationCallback(ExitFrame);

        private static object ExitFrame(object f)
        {
            ((DispatcherFrame)f).Continue = false;
            return null;
        }

    }
}
