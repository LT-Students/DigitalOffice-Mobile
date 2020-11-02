using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using LT.DigitalOffice.Mobile.Services;
using LT.DigitalOffice.Mobile.Views;
using Xamarin.Essentials;
using LT.DigitalOffice.Mobile.Models.ResponsesModels;

namespace LT.DigitalOffice.Mobile
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected async override void OnStart()
        {
            if (!Preferences.ContainsKey(nameof(UserData.Token)))
            {
                await Shell.Current.GoToAsync("//LoginPage");
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
