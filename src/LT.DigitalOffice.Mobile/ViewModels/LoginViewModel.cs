using LT.DigitalOffice.Mobile.Models.RequestsModels;
using LT.DigitalOffice.Mobile.Models.ResponsesModels;
using LT.DigitalOffice.Mobile.Views;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LT.DigitalOffice.Mobile.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }

        public bool isAutoLogin;

        private Page _page;
        private string _userLoginData;
        private string _userPassword;

        private const string LOGIN_URL = "https://178.154.210.148:9817/auth/login";

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

            SaveUserLoginData();

            string responseConvert = await response.Content.ReadAsStringAsync();

            UserData userData = JsonConvert.DeserializeObject<UserData>(responseConvert);

            SaveUserDataInRepository(userData);

            await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        }

        private async Task<HttpResponseMessage> SendUserCredentialsToAuthService()
        {
            // TODO remove this when we will have normal certificate
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            HttpClient client = new HttpClient(handler);

            HttpResponseMessage response = await client.PostAsync(new Uri(LOGIN_URL), GetRequestContent());

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