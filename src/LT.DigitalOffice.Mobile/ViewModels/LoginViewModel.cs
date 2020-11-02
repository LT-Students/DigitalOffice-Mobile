﻿using Android.OS;
using LT.DigitalOffice.Mobile.Models.RequestsModels;
using LT.DigitalOffice.Mobile.Views;
using System;
using System.Net.Http;
using LT.DigitalOffice.Mobile.Models.ResponsesModels;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Xamarin.Forms;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace LT.DigitalOffice.Mobile.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }

        private Page _page;
        private string _userLoginData;
        private string _userPassword;

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
            HttpResponseMessage response = await SendUserCredentialsToAuthService();

            if (!response.IsSuccessStatusCode)
            {
                await _page.DisplayAlert("Error login", "Your data credentials is wrong", "Ok");

                return;
            }

            string responseConvert = await response.Content.ReadAsStringAsync();

            UserData userData = JsonConvert.DeserializeObject<UserData>(responseConvert);

            SaveUserDataInRepository(userData);

            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        }

        private async Task<HttpResponseMessage> SendUserCredentialsToAuthService()
        {
            string loginUri = "http://172.24.208.1:9818/api/authentication/login";

            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.PostAsync(new Uri(loginUri), GetRequestContent());

            return response;
        }

        private StringContent GetRequestContent()
        {
            var userCredentials = new UserCredentials
            {
                LoginData = _userLoginData,
                Password = _userPassword
            };

            string json = JsonConvert.SerializeObject(userCredentials);

            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private void SaveUserDataInRepository(UserData userData)
        {
            Preferences.Set(nameof(userData.UserId), userData.UserId.ToString());
            Preferences.Set(nameof(userData.Token), userData.Token);
        }
    }
}