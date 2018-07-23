using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FlyingScreen.Common
{
   public class VideoThumbnail
    {
        [DllImport("FFmpegWrapper.dll", EntryPoint = "_GenMp4Image@8", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 GenMp4Image(String infilename, String outfilename);///////
        public static void ConvertImage2Thumbnail(string imageFilePath, ImageFormat format, string imageFullName, int width, int height)
        {
            System.Drawing.Image imageFrom = null;

            try
            {
                imageFrom = System.Drawing.Image.FromFile(imageFilePath);
            }
            catch
            {
                //throw;
            }

            if (imageFrom == null)
            {
                return;
            }

            // 源图宽度及高度
            int imageFromWidth = imageFrom.Width;
            int imageFromHeight = imageFrom.Height;

            // 生成的缩略图实际宽度及高度
            int bitmapWidth = width;
            int bitmapHeight = height;

            // 生成的缩略图在上述"画布"上的位置
            int X = 0;
            int Y = 0;

            // 根据源图及欲生成的缩略图尺寸,计算缩略图的实际尺寸及其在"画布"上的位置
            if (bitmapHeight * imageFromWidth > bitmapWidth * imageFromHeight)
            {
                bitmapHeight = imageFromHeight * width / imageFromWidth;
                Y = (height - bitmapHeight) / 2;
            }
            else
            {
                bitmapWidth = imageFromWidth * height / imageFromHeight;
                X = (width - bitmapWidth) / 2;
            }

            // 创建画布
            Bitmap bmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bmp);

            // 清除整个绘图面并以透明背景色填充
            g.Clear(System.Drawing.Color.White);

            // 指定高质量的双三次插值法。执行预筛选以确保高质量的收缩。此模式可产生质量最高的转换图像。
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            // 指定高质量、低速度呈现。
            g.SmoothingMode = SmoothingMode.HighQuality;

            // 在指定位置并且按指定大小绘制指定的 Image 的指定部分。
            g.DrawImage(imageFrom, new System.Drawing.Rectangle(X, Y, bitmapWidth, bitmapHeight), new System.Drawing.Rectangle(0, 0, imageFromWidth, imageFromHeight), GraphicsUnit.Pixel);

            imageFrom.Dispose();

            try
            {
                //经测试 .jpg 格式缩略图大小与质量等最优
                bmp.Save(imageFullName, format);
            }
            catch
            {
            }
            finally
            {
                //显式释放资源
                //imageFrom.Dispose();
                bmp.Dispose();
                g.Dispose();
            }
        }
        public static BitmapImage PathToBitmapImage(string ImagePath, string ControlType = "")
        {
            //ImagePath = ImagePath.Replace("//", "\\").Replace("/", "\\");
            var uri = new Uri(ImagePath, UriKind.RelativeOrAbsolute);

            if (!uri.IsAbsoluteUri && !string.IsNullOrEmpty(ControlType))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("/CustomControls;component/");
                sb.Append(ControlType);
                sb.Append("/");
                sb.Append(ImagePath);
                return new BitmapImage(new Uri(sb.ToString(), UriKind.Relative));
            }

            if (uri.IsFile && !File.Exists(uri.LocalPath))
                return null;

            return PathToBitmapImage(uri);
        }
        public static BitmapImage PathToBitmapImage(Uri ImagePath)
        {
            try
            {
                BitmapImage bitMapImage = new BitmapImage();
                bitMapImage.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
                bitMapImage.BeginInit();
                bitMapImage.CacheOption = BitmapCacheOption.OnLoad; //增加这一行
                bitMapImage.UriSource = ImagePath;
                bitMapImage.EndInit();
                return bitMapImage;
            }
            catch (Exception e)
            {
                return null;
            }

        }
    }
}
