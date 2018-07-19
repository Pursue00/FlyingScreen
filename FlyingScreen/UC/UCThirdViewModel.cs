using FlyingScreen.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyingScreen.UC
{
    public class UCThirdViewModel : BindingModelBase
    {
        #region Fileds
        public RelayCommand<object> BtnCommand { get; private set; }
        #endregion

        #region Property
        private bool _isVisibilityRank;

        public bool IsVisibilityRank
        {
            get { return _isVisibilityRank; }
            set { _isVisibilityRank = value; OnPropertyChanged(() => IsVisibilityRank); }
        }

        #endregion

        #region Constructure
        public UCThirdViewModel()
        {
            this.BtnCommand = new RelayCommand<object>(BtnCommandExcute);
        }
        #endregion

        #region Public Method
        public void BtnCommandExcute(object arg)
        {
            switch (arg)
            {
                case "exit":
                    this.IsVisibilityRank = true;
                    break;
                case "rotate":
                    this.IsVisibilityRank = true;
                    break;
                case "recycling":
                    this.IsVisibilityRank = true;
                    break;
                case "history":
                    this.IsVisibilityRank = true;
                    break;
                case "rank":
                    this.IsVisibilityRank = true;
                    break;
                case "setting":
                    this.IsVisibilityRank = true;
                    break;
                case "clear":
                    this.IsVisibilityRank = true;
                    break;
                case "return":
                    this.IsVisibilityRank = true;
                    break;
            }
        }
        #endregion
    }
}
