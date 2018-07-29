using FlyingScreen.Common;
using FlyingScreen.Common.SendMessageHelper;
using FlyingScreen.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FlyingScreen.UC
{
    public class UCThirdViewModel : BindingModelBase
    {
        #region Fileds
        public RelayCommand<object> BtnCommand { get; private set; }
        public RelayCommand<object> SelectedPageCommand { get; private set; }
        ObservableCollection<MediaItem> thumbnailCollection = new ObservableCollection<MediaItem>();
        ObservableCollection<int> speedList = new ObservableCollection<int>();
        ObservableCollection<int> positionList = new ObservableCollection<int>();
        int thumbnailsInProgress = 0;
        #endregion

        #region Property
        private bool _isVisibilityRank;

        public bool IsVisibilityRank
        {
            get { return _isVisibilityRank; }
            set { _isVisibilityRank = value; OnPropertyChanged(() => IsVisibilityRank); }
        }

        private bool _isVisibilitySetting;

        public bool IsVisibilitySetting
        {
            get { return _isVisibilitySetting; }
            set { _isVisibilitySetting = value; OnPropertyChanged(() => IsVisibilitySetting); }
        }

        private ImageSource _CameraSource;

        public ImageSource CameraSource
        {
            get { return _CameraSource; }
            set { _CameraSource = value; OnPropertyChanged(() => CameraSource); }
        }
        
        private ObservableCollection<SignBrowseSouce> _SignBrowseSource;

        public ObservableCollection<SignBrowseSouce> SignBrowseSource
        {
            get { return _SignBrowseSource; }
            set { _SignBrowseSource = value; OnPropertyChanged(() => SignBrowseSource); }
        }
        #endregion

        #region Constructure
        public UCThirdViewModel()
        {
            this.BtnCommand = new RelayCommand<object>(BtnCommandExcute);
            this.SelectedPageCommand = new RelayCommand<object>(SelectedPageCommandExcute);
            InitializationData();
            string camera = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LocalFile", "Video");
            //ImportMediaThreadProc(System.IO.Path.Combine(camera, "自我兑现的预言.mp4"), 4, 1);
            
        }
        #endregion

        #region Public Method
        private void InitializationData()
        {
            List<SignBrowseSouce> _SignBrowseSouce = new List<SignBrowseSouce>();
            string camera = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LocalFile","Picture");
            if (!Directory.Exists(camera))
                Directory.CreateDirectory(camera);
            else
            {
                DirectoryInfo _info = new DirectoryInfo(camera);
                foreach (var item in _info.GetFiles().Reverse())
                {
                    string url = System.IO.Path.Combine(camera, item.Name);
                    _SignBrowseSouce.Add(new SignBrowseSouce { CameraImgUrl = url, Width = 200.0, Height = 200.0 });
                }
            }
           
            SignBrowseSource = new ObservableCollection<SignBrowseSouce>(_SignBrowseSouce);
        }

        public void SelectedPageCommandExcute(object arg)
        {
            var item = ((ListBox)arg).SelectedItem as SignBrowseSouce;
            if (item != null)
            {
                this.CameraSource = item.CameraSource;
            }
        }

        public void BtnCommandExcute(object arg)
        {
            AppMessage appMessage = new AppMessage();
            switch (arg)
            {
                case "exit":
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    break;
                case "rotate":

                    break;
                case "recycling":

                    break;
                case "history":

                    break;
                case "rank":
                    this.IsVisibilityRank = true;
                    break;
                case "setting":
                    this.IsVisibilitySetting = true;
                    break;
                case "clear":

                    break;
                case "return":

                    break;
                case "desktop":
                    appMessage.MsgType = AppMsg.Desktop;
                    EventHub.SysEvents.PubEvent<AppMessage>(appMessage);
                    break;
            }
        }

        #region 视频缩略图转换
        //We would like to create thumbnails one after the other; so we will use a lock to give 
        //access to only one path of execution to this method
        object importMediaLock = new object();
        void ImportMediaThreadProc(string mediaFile, int waitTime, int position)
        {
            lock (importMediaLock)
            {
                MediaPlayer player = new MediaPlayer { Volume = 0, ScrubbingEnabled = true };
                player.Open(new Uri(mediaFile));
                player.Pause();
                player.Position = TimeSpan.FromMilliseconds(position * 1000);
                //We need to give MediaPlayer some time to load. The efficiency of the MediaPlayer depends                 
                //upon the capabilities of the machine it is running on
                System.Threading.Thread.Sleep(waitTime * 1000);

                //120 = thumbnail width, 90 = thumbnail height and 96x96 = horizontal x vertical DPI
                //An in actual application, you wouldn's probably use hard coded values!
                RenderTargetBitmap rtb = new RenderTargetBitmap(120, 90, 96, 96, PixelFormats.Pbgra32);
                DrawingVisual dv = new DrawingVisual();
                using (DrawingContext dc = dv.RenderOpen())
                {
                    dc.DrawVideo(player, new Rect(0, 0, 120, 90));
                }
                rtb.Render(dv);
                Duration duration = player.NaturalDuration;
                int videoLength = 0;
                if (duration.HasTimeSpan)
                {
                    videoLength = (int)duration.TimeSpan.TotalSeconds;
                }
                BitmapFrame frame = BitmapFrame.Create(rtb).GetCurrentValueAsFrozen() as BitmapFrame;
                BitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(frame as BitmapFrame);

                //We cannot create the thumbnail here, as we are not in the UI thread right now
                //It is the responsibility of the calee to close the MemoryStream
                //We will instead call a method which will do its stuff on the UI thread!
                MemoryStream memoryStream = new MemoryStream();
                encoder.Save(memoryStream);
                CreateMediaItem(memoryStream, mediaFile, videoLength);
                player.Close();
            }
        }

        void CreateMediaItem(MemoryStream ms, string name, int duration)
        {
            Application.Current.MainWindow.Dispatcher.Invoke(new Action(delegate
          {
              ms.Position = 0;
              MediaItem mediaItem = new MediaItem();
              BitmapImage bitmapImage = new BitmapImage();
              bitmapImage.BeginInit();
              bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
              bitmapImage.StreamSource = ms;
              bitmapImage.EndInit();
              string filepath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "1.png");
              SaveBitmapImageIntoFile(bitmapImage, filepath);
              mediaItem.Thumbnail = bitmapImage;
              mediaItem.Duration = duration;
              mediaItem.Name = System.IO.Path.GetFileName(name);
                //The protocol we defined expects us to close the MemoryStream here.
                //Otherwise our memory consumption would not be optimized. GC can take its time; so we better do it ourselves!
                ms.Close();
              thumbnailCollection.Add(mediaItem);
              if (--thumbnailsInProgress == 0)
              {
                    //downloadAnimation.StopAnimation();
                    // animationGrid.Visibility = System.Windows.Visibility.Collapsed;
                }
          }));
        }

        /// <summary>
        /// 把内存里的BitmapImage数据保存到硬盘中
        /// </summary>
        /// <param name="bitmapImage">BitmapImage数据</param>
        /// <param name="filePath">输出的文件路径</param>
        void SaveBitmapImageIntoFile(BitmapImage bitmapImage, string filePath)
        {
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));

            using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
            {
                encoder.Save(fileStream);
            }
        }

        #endregion
        #endregion
    }

    class MediaItem
    {
        public BitmapImage Thumbnail { get; set; }
        public int Duration { get; set; }
        public string Name { get; set; }
    }
}
