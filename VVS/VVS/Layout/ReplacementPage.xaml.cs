using Plugin.ExternalMaps;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVS.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VVS.Layout
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReplacementPage : ContentPage
    {
        private Replacement _replacement;
        private Position _position;

        public ReplacementPage(Replacement currentReplacement)
        {
            if (currentReplacement == null)
                throw new ArgumentNullException(nameof(currentReplacement));
            _replacement = currentReplacement;
            InitializeComponent();
            SetLabelsOnScreen();
        }


        private async void Location_OnClicked(object sender, EventArgs e)
        {
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;

                _position = await locator.GetPositionAsync(10000);
                if (_position == null)
                    return;

                Debug.WriteLine("Position Status: {0}", _position.Timestamp);
                Debug.WriteLine("Position Latitude: {0}", _position.Latitude);
                Debug.WriteLine("Position Longitude: {0}", _position.Longitude);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to get location, may need to increase timeout: " + ex);
            }
            try
            {
                var success = await CrossExternalMaps.Current.NavigateTo(_replacement.CustomerName, _position.Latitude, _position.Longitude);
                Debug.WriteLine("Succes Status: {0}", success);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to get open map, Nullpointer? " + ex);
            }
        }

        private void OldMeter_OnClicked(object sender, EventArgs e)
        {
            LaunchMeterPage(2);
        }    

        private void NewMeter_OnClicked(object sender, EventArgs e)
        {
            LaunchMeterPage(3);
        }

        private void BeforeReport_OnClicked(object sender, EventArgs e)
        {
            LaunchReportPage(1);
        }

        private void AfterReport_OnClicked(object sender, EventArgs e)
        {
            LaunchReportPage(4);
        }

        //identifier: 2 oldMeter, 3 newMeter
        private async void LaunchMeterPage(int identifier)
        {
            try
            {
                await Navigation.PushAsync(new MeterPage(_replacement, identifier));
            }
            catch (Exception)
            {
                Debug.WriteLine(GetErrorText(identifier));
                throw;
            }
        }

        private async void LaunchReportPage(int identifier)
        {
            try
            {
                await Navigation.PushAsync(new ReportPage(_replacement, identifier));
            }
            catch (Exception)
            {
                Debug.WriteLine(GetErrorText(identifier));
                throw;
            }
        }

        // get Error teext based on identifier
        private string GetErrorText(int identifier)
        {
            string errorText = "Fejl i ";
            if (identifier == 1)
            {
                errorText += "før installationsrapport";
            }
            else if (identifier == 4)
            {
                errorText += "efter installationsrapport";
            }
            else if (identifier == 2)
            {
                errorText += "Gammel Meter";
            }
            else if (identifier == 3)
            {
                errorText += "Nyt Meter";
            }
            return errorText;
        }

        private void SetLabelsOnScreen()
        {
            CustomerName.Text = _replacement.CustomerName;
            ReplacementAddress.Text = _replacement.Location.Address;
            ReplacementTime.Text = _replacement.Time.ToString();
            string status = "";
            if (_replacement.Status == -1)
            {
                status = "udskiftning kunne ikke foretages";
            }
            else if (_replacement.Status == 0) { status = "udskiftning ikke startet"; } else if (_replacement.Status == 1) { status = "gps kordinat registreret"; } else if (_replacement.Status == 2) { status = "før installations rapport registreret"; } else if (_replacement.Status == 3) { status = "Gammelt Meter registreret"; } else if (_replacement.Status == 4) { status = "Nyt Meter Registeret"; } else if (_replacement.Status == 5) { status = "Efter installations rapport registreret"; } else if (_replacement.Status == 6) { status = "Udskiftning færdig og registrert i Databasen"; }
            ReplacementStatus.Text = status;
        }
    }
}
