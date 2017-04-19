using System;
using System.Diagnostics;
using VVS.Layout;
using Xamarin.Forms;

namespace VVS
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Debug_OnClicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new Replacement());
            }
            catch (Exception)
            {
                Debug.WriteLine("Replacement didn't work");
                throw;
            }
        }
    }
}
