using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VVS.Layout
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Replacement : ContentPage
    {
        public Replacement()
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

        private async void NewMeterPictures_OnClicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new NewMeterPicture());
            }
            catch (Exception)
            {
                Debug.WriteLine("Old meter picture didn't work");
                throw;
            }
        }
    }
}
