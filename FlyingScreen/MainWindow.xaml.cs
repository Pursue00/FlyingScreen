using FlyingScreen.Common.SendMessageHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlyingScreen
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            EventHub.SysEvents.SubEvent<AppMessage>(OnRecMsg, Prism.Events.ThreadOption.UIThread);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }
        private void OnRecMsg(AppMessage appMessage)
        {
            if (appMessage.MsgType == AppMsg.FileType)
            {
                switch (appMessage.Tag.ToString())
                {
                    case "PDF":
                        break;
                    case "Word":
                        break;
                    case "视频":
                        break;
                    case "图片":
                        break;
                    case "PPT":
                        break;
                    case "Excel":
                        break;
                    case "网站":
                        break;
                }
                this.ucthird.Visibility = Visibility.Visible;
            }
            else if (appMessage.MsgType == AppMsg.Palm)
            {

                this.coolbtn.Visibility = Visibility.Visible;
            }
            else if (appMessage.MsgType==AppMsg.Desktop)
            {
                this.WindowState = WindowState.Minimized;
            }
        }
    }
}
