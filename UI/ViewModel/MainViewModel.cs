using GalaSoft.MvvmLight;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using UI.Pages;

namespace UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private Page edit;
        private Page regExp;
        private Page settings;
        public MainViewModel()
        {
            edit = new Edit();
            regExp = new RegularExpression();
            settings = new Settings();

            CurrentPage = edit;
        }

        private Page _currentPage;

        public ICommand bEdit_Click
        {
            get
            {
                return new RelayCommand(() => CurrentPage = edit);
            }
        }
        public ICommand bRegExp_Click
        {
            get
            {
                return new RelayCommand(() => CurrentPage = regExp);
            }
        }
        public ICommand bSettings_Click
        {
            get
            {
                return new RelayCommand(() => CurrentPage = settings);
            }
        }
        public Page CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
                RaisePropertyChanged(() => CurrentPage);
            }
        }

    }
}