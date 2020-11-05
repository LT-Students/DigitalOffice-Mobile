using LT.DigitalOffice.Mobile.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LT.DigitalOffice.Mobile.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }

        private Page _page;

        private string _userLoginData;
        private string _userPassword;

        public bool isAutoLogin;

        public string LoginData
        {
            get => _userLoginData;
            set => SetProperty(ref _userLoginData, value);
        }

        public string Password
        {
            get => _userPassword;
            set => SetProperty(ref _userPassword, value);
        }

        public LoginViewModel(Page page)
        {
            _page = page;
            LoginCommand = new Command(OnLoginClicked);
        }

        private async void OnLoginClicked(object obj)
        {
            SaveUserLoginData();

            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        }

        private void SaveUserLoginData()
        {
            if (isAutoLogin)
            {
                Preferences.Set(nameof(isAutoLogin), isAutoLogin);
                Preferences.Set(nameof(_userLoginData), _userLoginData);
                Preferences.Set(nameof(_userPassword), _userPassword);
            }
            else
            {
                Preferences.Set(nameof(isAutoLogin), isAutoLogin);
            }
        }
    }
}
