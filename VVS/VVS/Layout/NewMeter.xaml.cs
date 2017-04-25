using SQLite;
using System;
using System.Diagnostics;
using Plugin.Media;
using Plugin.Media.Abstractions;
using VVS.Database;
using VVS.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;
using Plugin.Geolocator;

namespace VVS.Layout
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewMeter : ContentPage
    {
        private SQLiteAsyncConnection _connection;
        private Replacement _replacement;
        //use 1 for old meter, 2 for new meter.
        private int oldOrNew = 0;

        public NewMeter(Replacement currentReplacement, int oldOrNew)
        {
            if (currentReplacement == null)
                throw new ArgumentNullException(nameof(currentReplacement));
            InitializeComponent();
            _replacement = currentReplacement;
            this.oldOrNew = oldOrNew;
            _connection = DependencyService.Get<ISQLiteDB>().GetConnection();

            takePhotoFull.Clicked += async (sender, args) =>
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DisplayAlert("No Camera", ":( No camera available.", "OK");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    //TODO Insert id into filename 
                    Directory = "NewInstallion",
                    Name = _replacement.Id + " " + DateTime.Now + " FULL.jpg",
                    PhotoSize = PhotoSize.Small,
                    //Save to album makes the photo visable in your gallary app
                    SaveToAlbum = true
                });

                if (file == null)
                    return;
                //Shows a alert with the full path to the picture, it's only meant to be used for debugging
                await DisplayAlert("File Location", file.Path, "OK");

                imageFull.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    file.Dispose();
                    return stream;
                });
            };

            takePhotoMeter.Clicked += async (sender, args) =>
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DisplayAlert("No Camera", ":( No camera available.", "OK");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    //TODO Insert id into filename 
                    Directory = "NewInstallion",
                    Name = _replacement.Id + " " + DateTime.Now + " METER.jpg",
                    PhotoSize = PhotoSize.Small,
                    //Save to album makes the photo visable in your gallary app
                    SaveToAlbum = true
                });

                if (file == null)
                    return;
                //Shows a alert with the full path to the picture, it's only meant to be used for debugging
                await DisplayAlert("File Location", file.Path, "OK");

                imageMeter.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    file.Dispose();
                    return stream;
                });
            };
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
                    if (String.IsNullOrWhiteSpace(BarcodeField.Text))
                    {
                        BarcodeField.Text = result.Text;
                        BarcodeField.IsEnabled = false;                        
                    }
                    else
                    {
                        BarcodeField2.Text = result.Text;
                        BarcodeField2.IsEnabled = false;
                    }
                });
            };
        }

        private async void GetLokation_OnClicked(object sender, EventArgs e)
        {
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;

                var position = await locator.GetPositionAsync(10000);
                if (position == null)
                    return;
                Latitude.Text = position.Latitude.ToString();
                Longitude.Text = position.Longitude.ToString();

                Debug.WriteLine("Position Status: {0}", position.Timestamp);
                Debug.WriteLine("Position Latitude: {0}", position.Latitude);
                Debug.WriteLine("Position Longitude: {0}", position.Longitude);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to get location, may need to increase timeout: " + ex);
            }
        }

        private async void Onsave(object sender, EventArgs e)
        {
            // Validate serialnumber
            try
            {
                string serialNo1Raw = BarcodeField.Text;
                string serialNo2Raw = BarcodeField2.Text;
                string enterSN = "Vær venlig at indtast serienumre i begge felter";
                CheckNullorWhiteSpace(serialNo1Raw, enterSN);
                serialNo1Raw = serialNo1Raw.ToLower().Trim();
                CheckNullorWhiteSpace(serialNo2Raw, enterSN);
                serialNo2Raw = serialNo2Raw.ToLower().Trim();

                long serialNo1 = -1;
                long.TryParse(serialNo1Raw, out serialNo1);
                long serialNo2 = -1;
                long.TryParse(serialNo2Raw, out serialNo2);

               /* if (serialNo2 != serialNo1)
                {
                    await DisplayAlert("Error", "Serienumre skal være identiske", "OK");
                    return;
                }*/
                //validate Consumption
                string consumption1Str = Consumption.Text;
                string consumption2Str = Consumption2.Text;
                string enterConsumption = "Vær venlig at indtast forbrug i begge felter";
                CheckNullorWhiteSpace(consumption1Str, enterConsumption);
                consumption1Str = consumption1Str.ToLower().Trim();
                CheckNullorWhiteSpace(consumption2Str, enterConsumption);
                consumption2Str = consumption2Str.ToLower().Trim();

                int consumption1 = -1;
                Int32.TryParse(consumption1Str, out consumption1);
                int consumption2 = -1;
                Int32.TryParse(consumption2Str, out consumption2);

              /*  if (consumption1 != consumption2)
                {
                    await DisplayAlert("Error", "Forbrugs tallene skal være identiske", "OK");
                    return;
                }*/

                string comment = Comment.Text;

                //TODO add picture path from camera.

                //construct Meter
                var newMeterData = new Meter(serialNo1, consumption1, "picPath", comment);

                //Create Location object
                var locations = await _connection.Table<Location>().ToListAsync();
                var locId = _replacement.LocId;
                var address = _replacement.Location.Address;
                var location = new Location(locId, address, Double.Parse(Longitude.Text), Double.Parse(Latitude.Text));

                //check if the meter already exists in db
                var mList = await _connection.Table<Meter>().ToListAsync();
                var m = mList.Find(x => x.SerialNumber == serialNo1);
                //save meter to DB and update Job.

                try
                {
                    if (m == null)
                    {
                        await _connection.InsertAsync(newMeterData);

                        _replacement.Status = 4;
                        _replacement.NewMeterId = newMeterData.SerialNumber;
                        await _connection.UpdateAsync(_replacement);
                        //update Location with new gps coordinates
                        await _connection.UpdateAsync(location);
                        await DisplayAlert("Info", "Data er gemt", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Error", "Måleren med given serial nummer existerer allerede", "OK");
                    }
                    

                   
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("ERRRORR" + ex.Message);
                }
 
                await Navigation.PopAsync();
               
            }
            catch (Exception)
            {

                await DisplayAlert("Error", "Udfyld venligst alle felterne", "OK");
                return;
            }
        }

        private async void CheckNullorWhiteSpace(string text, string errorMessage)
        {
            if (String.IsNullOrWhiteSpace(text))
            {
                await DisplayAlert("Error", errorMessage, "OK");
                return;
            }            
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }
    }
}
