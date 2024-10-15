using Lab2Indvidual.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Lab2Indvidual.Views
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