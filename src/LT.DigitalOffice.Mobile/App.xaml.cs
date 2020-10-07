using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using LT.DigitalOffice.Mobile.Services;
using LT.DigitalOffice.Mobile.Views;

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

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
