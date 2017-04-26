using Plugin.ExternalMaps;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using SQLite;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using VVS.Database;
using VVS.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VVS.Layout
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReplacementPage : ContentPage
    {
        private SQLiteAsyncConnection _connection;
        private Replacement _replacement;
        private Position _position;

        public ReplacementPage(Replacement currentReplacement)
        {
            if (currentReplacement == null)
                throw new ArgumentNullException(nameof(currentReplacement));
            _replacement = currentReplacement;
            InitializeComponent();            
            _connection = DependencyService.Get<ISQLiteDB>().GetConnection();
        }

        protected override void OnAppearing()
        {
            SetOnScreen();

            base.OnAppearing();
        }

        private async void LocationShowRoute_OnClicked(object sender, EventArgs e)
        {
            await LoadMap(_replacement.Location.Latitude, _replacement.Location.Longitude);
            LocNew.IsEnabled = true;
        }

        private async void LocationCurrent_OnClicked(object sender, EventArgs e)
        {
            await LocationCurrent();
            _replacement.Location.Latitude = _position.Latitude;
            _replacement.Location.Longitude = _position.Longitude;
            _replacement.Status = 1;
            await _connection.UpdateAsync(_replacement.Location);
            SetOnScreen();
        }

        private async Task LocationCurrent()
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
        }

        private async Task LoadMap(double latitude, double longitude)
        {
            try
            {
                var success = await CrossExternalMaps.Current.NavigateTo(_replacement.CustomerName, latitude, longitude);
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

        private async void ReplacementDone_Clicked(object sender, EventArgs e)
        {
            if (_replacement.BeforeReport != null && _replacement.AfterReport != null && _replacement.Location != null && _replacement.OldMeter != null && _replacement.OldMeter != null)
            {
                _replacement.Status = 6;
                await _connection.UpdateAsync(_replacement);
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Husk det hele", "En eller flere rapporter er ikke udfyldt, eller noget gik galt", "OK");
                return;
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

        //sets xaml text and disable buttons based on Status.
        private void SetOnScreen()
        {
            CustomerName.Text = _replacement.CustomerName;
            ReplacementAddress.Text = _replacement.Location.Address;
            ReplacementTime.Text = _replacement.Time.ToString();
            string status = "";
            if (_replacement.Status == -1)
            {
                status = "udskiftning kunne ikke foretages";
            }else if (_replacement.Status == 0)
            {
                status = "udskiftning ikke startet";
            } else if (_replacement.Status == 1)
            {
                status = "Lokation registreret";
                LocOld.IsEnabled = false;
                LocNew.IsEnabled = false;
            } else if (_replacement.Status == 2)
            {
                status = "før installations rapport registreret";
                ReportBefore.IsEnabled = false;
                ReportBefore.BackgroundColor = Color.Green;
            } else if (_replacement.Status == 3)
            {
                status = "Gammelt Meter registreret";
                MeterOld.IsEnabled = false;
                MeterOld.BackgroundColor = Color.Green;
            } else if (_replacement.Status == 4)
            {
                status = "Nyt Meter Registeret";
                MeterNew.IsEnabled = false;
                MeterNew.BackgroundColor = Color.Green;
            } else if (_replacement.Status == 5)
            {
                status = "Efter installations rapport registreret";
                ReportAfter.IsEnabled = false;
                ReportAfter.BackgroundColor = Color.Green;
            } else if (_replacement.Status == 6)
            {
                status = "Udskiftning færdig og registrert i Databasen";
            }
            ReplacementStatus.Text = status;
        }
    }
}
