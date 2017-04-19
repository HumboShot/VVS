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

        private async void OldMeter_OnClicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new OldMeter());
            }
            catch (Exception)
            {
                Debug.WriteLine("Old meter didn't work");
                throw;
            }
        }

        private async void NewMeter_OnClicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new NewMeter());
            }
            catch (Exception)
            {
                Debug.WriteLine("New meter didn't work");
                throw;
            }
        }

        private async void OldMeterPictures_OnClicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new OldMeterPicture());
            }
            catch (Exception)
            {
                Debug.WriteLine("Old meter picture didn't work");
                throw;
            }
        }
    }
}
