using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace VVS.Layout
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewMeter : ContentPage
    {
        public NewMeter()
        {
            InitializeComponent();
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            var scanPage = new ZXingScannerPage();
            await Navigation.PushAsync(scanPage);

            scanPage.OnScanResult += (result) =>
            {
                // Stop scanning
                scanPage.IsScanning = false;

                // Pop the page and show the result
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();
                    await DisplayAlert("Scanned Barcode", result.Text, "OK");
                    BarcodeField.Text = result.Text;
                });
            };
        }
    }
}
