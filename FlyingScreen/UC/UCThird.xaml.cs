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
    /// UCThird.xaml 的交互逻辑
    /// </summary>
    public partial class UCThird : UserControl
    {
        UCThirdViewModel vm;
        public UCThird()
        {
            InitializeComponent();
            vm = new UCThirdViewModel();
            this.DataContext = vm;
            this.Loaded += UCThird_Loaded;
            //listBoxview.AddHandler(ListBox.MouseMoveEvent, new MouseWheelEventHandler(list_MouseWheel), true);
           
        }

        private void UCThird_Loaded(object sender, RoutedEventArgs e)
        {
            listBoxview.AddHandler(ListBox.MouseWheelEvent, new MouseWheelEventHandler(list_MouseWheel), true);

        }

        public static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            if (obj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                    if (child != null && child is T)
                    {
                        return (T)child;
                    }
                    T childItem = FindVisualChild<T>(child);
                    if (childItem != null) return childItem;
                }
            }
            return null;
        }


        private void list_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            ItemsControl items = (ItemsControl)sender;
            ScrollViewer scroll = FindVisualChild<ScrollViewer>(items);
            if (scroll != null)
            {
                int d = e.Delta;
                if (d > 0)
                {
                    scroll.LineLeft();
                }
                if (d < 0)
                {
                    scroll.LineRight();
                }
                //scroll.ScrollToTop();
            }
        }

    }
}
