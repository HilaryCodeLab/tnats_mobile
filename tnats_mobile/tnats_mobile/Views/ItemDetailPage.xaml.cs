using System.ComponentModel;
using Xamarin.Forms;
using tnats_mobile.ViewModels;

namespace tnats_mobile.Views
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