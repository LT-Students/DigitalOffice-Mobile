using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LT.DigitalOffice.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LT.DigitalOffice.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel(this);
        }

        protected void OnCheckBoxChangedSaveLoginData(object sender, CheckedChangedEventArgs eventArgs)
        {
            LoginViewModel loginViewModel = (LoginViewModel)BindingContext;

            loginViewModel.isAutoLogin = eventArgs.Value;
        }
    }
}