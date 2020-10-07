using System;
using System.Collections.Generic;
using LT.DigitalOffice.Mobile.ViewModels;
using LT.DigitalOffice.Mobile.Views;
using Xamarin.Forms;

namespace LT.DigitalOffice.Mobile
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
