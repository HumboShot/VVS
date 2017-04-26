using SQLite;
using System;
using System.Diagnostics;
using VVS.Database;
using VVS.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace VVS.Layout
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MeterPage : ContentPage
    {
        private SQLiteAsyncConnection _connection;
        private Replacement _replacement;
        //identifier: 2 for old meter, 3 for new meter.
        private int _identifier;
        private Meter _meter = new Meter();

        public MeterPage(Replacement currentReplacement, int identifier)
        {
            if (currentReplacement == null)
                throw new ArgumentNullException(nameof(currentReplacement));
            InitializeComponent();
            _replacement = currentReplacement;
            _identifier = identifier;
            _connection = DependencyService.Get<ISQLiteDB>().GetConnection();

            //bind meter to Replacement.
            if (identifier == 3)
            {
                _replacement.NewMeter = _meter;
            }
            else if (_replacement.OldMeter != null)
            {
                _meter = _replacement.OldMeter;
            }else if(identifier == 2)
            {
                _replacement.OldMeter = _meter;
            }
            
        }


        private async void TakePhotoMeter(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PicturePage(_replacement, _identifier));
        }


        private async void ButtonScanner_OnClicked(object sender, EventArgs e)
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
                    //await DisplayAlert("Scanned Barcode", result.Text, "OK");
                    if (String.IsNullOrWhiteSpace(BarcodeField.Text))
                    {
                        BarcodeField.Text = result.Text;                        
                        ScanButton.Text = "Valider Scan";
                    }
                    else if (String.IsNullOrWhiteSpace(BarcodeField2.Text))
                    {
                        BarcodeField2.Text = result.Text;
                        ScanButton.IsEnabled = false;                        
                    }    
                });
            };
        }

        private async void Onsave(object sender, EventArgs e)
        {
            // Validate serialnumber
            string serialNo1Raw = BarcodeField.Text;
            string serialNo2Raw = BarcodeField2.Text;
            string enterSN = "Vær venlig at indtast serienumre i begge felter";
            CheckNullorWhiteSpace(serialNo1Raw, enterSN);
            serialNo1Raw = serialNo1Raw.ToLower().Trim();
            CheckNullorWhiteSpace(serialNo2Raw, enterSN);
            serialNo2Raw = serialNo2Raw.ToLower().Trim();

            int serialNo1 = -1;
            Int32.TryParse(serialNo1Raw, out serialNo1);
            int serialNo2 = -1;
            Int32.TryParse(serialNo2Raw, out serialNo2);

            if (serialNo2 != serialNo1)
            {
                await DisplayAlert("Error", "Serienumre skal være identiske", "OK");
                return;
            }
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

            if (consumption1 != consumption2)
            {
                await DisplayAlert("Error", "Forbrugs tallene skal være identiske", "OK");
                return;
            }

            //set Meter Values
            _meter.SerialNumber = serialNo1;
            _meter.Consumtion = consumption1;                      
            _meter.Comment = Comment.Text;

            //save meter to DB and update Replacement.
            try
            {
                await _connection.InsertOrReplaceAsync(_meter);

                if(_identifier == 2)
                {
                    _replacement.Status = 3;
                    _replacement.OldMeterId = _meter.SerialNumber;
                }
                else if (_identifier == 3)
                {
                    _replacement.Status = 4;
                    _replacement.NewMeterId = _meter.SerialNumber;
                }

                await _connection.UpdateAsync(_replacement);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERRRORR" + ex.Message);
            }

            await Navigation.PopAsync();
        }

        private async void CheckNullorWhiteSpace(string text, string errorMessage)
        {
            if (String.IsNullOrWhiteSpace(text))
            {
                await DisplayAlert("Error", errorMessage, "OK");
                return;
            }
        }
    }
}
