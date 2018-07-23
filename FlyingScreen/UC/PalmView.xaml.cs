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

namespace FlyingScreen.UC
{
    /// <summary>
    /// PalmView.xaml 的交互逻辑
    /// </summary>
    public partial class PalmView : UserControl
    {
        public PalmView()
        {
            InitializeComponent();
        }

        private void btngesture_Click(object sender, RoutedEventArgs e)
        {
            AppMessage appMessage = new AppMessage();
            appMessage.MsgType = AppMsg.Palm;
            EventHub.SysEvents.PubEvent(appMessage);
        }
    }
}
