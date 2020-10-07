using System.ComponentModel;
using Xamarin.Forms;
using LT.DigitalOffice.Mobile.ViewModels;

namespace LT.DigitalOffice.Mobile.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}