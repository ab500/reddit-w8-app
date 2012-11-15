using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using RedditStoreApp.Data.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace RedditStoreApp.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        private IRedditApi _dataService;

        private string _username;
        private string _password;

        private enum FormState { LoggedIn, LoggingIn, LoggedOut };
        private FormState _currentState;

        public SettingsViewModel(IRedditApi dataService)
        {
            _dataService = dataService;
            _currentState = FormState.LoggedOut;
            Login = new RelayCommand(DoLogin);
        }

        private void NotifyStateChange()
        {
            RaisePropertyChanged("IsFormEditable");
            RaisePropertyChanged("LoginHeader");
            RaisePropertyChanged("LoginText");
            RaisePropertyChanged("LoginButtonText");
            RaisePropertyChanged("IsProcessing");
        }

        private void DoLogin()
        {
            if (_currentState == FormState.LoggedOut)
            {
                _currentState = FormState.LoggingIn;
            }
            else if (_currentState == FormState.LoggingIn)
            {
                _currentState = FormState.LoggedIn;
            }
            else
            {
                _currentState = FormState.LoggedOut;
            }
            NotifyStateChange(); 
        }

        public RelayCommand Login { get; private set; }

        public string LoginHeader
        {
            get
            {
                if (_currentState == FormState.LoggedIn)
                {
                    return String.Format((string)Application.Current.Resources["AuthedHeader"], "wastingtime1");
                }
                else
                {
                    return (string)Application.Current.Resources["LoginHeader"];
                }
            }
        }

        public string LoginText
        {
            get
            {
                if (_currentState == FormState.LoggedIn)
                {
                    return String.Format((string)Application.Current.Resources["AuthedText"], "wastingtime1");
                }
                else
                {
                    return (string)Application.Current.Resources["LoginText"];
                }
            }
        }

        public string LoginButtonText
        {
            get
            {
                if (_currentState == FormState.LoggedIn)
                {
                    return (string)Application.Current.Resources["LoginBtn_Logout"];
                }
                else if (_currentState == FormState.LoggedOut)
                {
                    return (string)Application.Current.Resources["LoginBtn_Login"];
                }
                else
                {
                    return (string)Application.Current.Resources["LoginBtn_InProgress"];
                }
            }
        }

        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
                RaisePropertyChanged("Username");
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                RaisePropertyChanged("Password");
            }
        }

        public bool IsProcessing
        {
            get
            {
                return _currentState == FormState.LoggingIn;
            }
        }

        public bool IsFormEditable
        {
            get
            {
                return _currentState == FormState.LoggedOut;
            }
        }
    }
}
