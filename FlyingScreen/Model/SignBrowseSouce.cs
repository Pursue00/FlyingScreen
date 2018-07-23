using FlyingScreen.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FlyingScreen.Model
{
    public class SignBrowseSouce : BindingModelBase
    {
        //相机照片路径
        private ImageSource _CameraSource;

        public ImageSource CameraSource
        {
            get { return _CameraSource; }
            set { _CameraSource = value; OnPropertyChanged(()=> CameraSource); }
        }

        private string _CameraImgUrl;
        public string CameraImgUrl
        {
            get { return _CameraImgUrl; }
            set
            {
                if (value == null)
                    return;
                _CameraImgUrl = value;
                var bitmap = BitmapEncoderHelper.PathToBitmapThumbImage(_CameraImgUrl);
                if (bitmap != null)
                    CameraSource = bitmap;
            }
        }

        private string _SelectedImgUrl;
        public string SelectedImgUrl
        {
            get { return _SelectedImgUrl; }
            set
            {
                if (value == null)
                    return;
                _SelectedImgUrl = value;
                OnPropertyChanged(()=> SelectedImgUrl);
            }
        }

        //宽度
        private double _Width;

        public double Width
        {
            get { return _Width; }
            set { _Width = value; OnPropertyChanged(()=> Width); }
        }
        //高度
        private double _Height;

        public double Height
        {
            get { return _Height; }
            set { _Height = value; OnPropertyChanged(()=> Height); }
        }

    }
}
