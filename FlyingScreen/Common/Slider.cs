using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;

namespace FlyingScreen.Common
{
    class Slider : Canvas
    {
        public Slider()
        {
            //this.Background = Brushes.LightBlue;
            this.RenderTransform = translate;
            //this.PreviewMouseDown += new MouseButtonEventHandler(Slider_MouseDown);
            //this.PreviewMouseUp += new MouseButtonEventHandler(Slider_MouseUp);
            this.RenderTransform = (transform = new TranslateTransform());
            Messenger.Default.Register<int>(this, NotificationFunc);
        }

        private void NotificationFunc(int size)
        {
            Go(size);
        }

        private void Slider_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.CaptureMouse();
            Point p = e.GetPosition(App.Current.MainWindow);
            x = p.X;
            e.Handled = true;
        }

        private void Slider_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //if (this.IsMouseCaptured)
            //{
            //    Point p = e.GetPosition(App.Current.MainWindow);
            //    var offsetX = p.X - x;
            //    if (offsetX > 10)
            //    {
            //        if (index > 0) { --index; }
            //        Go();
            //    } if (offsetX < -10)
            //    {
            //        if (index < page - 1) { ++index; }
            //        Go();
            //    }
            //    this.ReleaseMouseCapture();
            //}
            //e.Handled = true;
        }
        private void Go(int _index)
        {
            DoubleAnimation a = new DoubleAnimation(-_index * viewport.Width, TimeSpan.FromMilliseconds(500));
            a.AccelerationRatio = .3;
            a.DecelerationRatio = .3;
            transform.BeginAnimation(TranslateTransform.XProperty, a);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            double dWidth = Math.Floor(constraint.Width / 280.00);
            double dHeight = Math.Floor(constraint.Height / 280.00);
            Size s = new Size(Math.Ceiling(InternalChildren.Count / (dWidth * dHeight)) * constraint.Width, constraint.Height);

            Size extentTmp = new Size(s.Width * this.InternalChildren.Count, constraint.Height);
            foreach (UIElement each in InternalChildren)
            {
                each.Measure(new Size(280, 280));
            }
            if (extentTmp != extent)
            {
                extent = s;
            }
            if (viewport != constraint)
            {
                viewport = constraint;
            }
            return s;
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            int count = (int)Math.Floor(viewport.Width / 280.00);
            page = (int)Math.Ceiling((decimal)InternalChildren.Count / (count * 2));
            int temp = 0;
            int n = 2;
            int countView = 0;
            try
            {
                for (int i = 0; i < InternalChildren.Count; i++)
                {
                    this.InternalChildren[i].Arrange(new Rect((280 * (i - countView * 2 * count)) + (viewport.Width * countView) + ((viewport.Width - count * 280) / 2), 50, 280, 280));
                    temp++;
                    if (temp > count)
                    {
                        for (int j = i; j < n * count; j++)
                        {
                            this.InternalChildren[j].Arrange(new Rect(280 * (j - count - countView * 2 * count) + (viewport.Width * countView) + ((viewport.Width - count * 280) / 2), 280, 280, 280));
                            i = j;
                        }
                        countView++;
                        n += 2;
                        temp = 0;
                    }
                }
            }
            catch (ArgumentOutOfRangeException) { }
            return arrangeSize;
        }

        private Size extent = new Size();
        private Size viewport = new Size();
        private TranslateTransform translate = new TranslateTransform();
        private double x;
        private TranslateTransform transform;
        private int page;
        private int index;
    }
}
