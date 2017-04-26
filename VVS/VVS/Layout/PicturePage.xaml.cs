using System;
using Plugin.Media;
using Plugin.Media.Abstractions;
using VVS.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VVS.Layout
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PicturePage : ContentPage
    {
        private Replacement _replacement;
        private int _identifier;
        private string _Directory;
        private string _Name;
        private string _filePath;

        public PicturePage(Replacement currentReplacement, int identifier)
        {
            InitializeComponent();
            _replacement = currentReplacement;
            _identifier = identifier;
            SetDirectoryAndFilename();
            takePhoto();
        }

        private void ButtonRetake_Clicked(object sender, EventArgs e)
        {
            takePhoto();
        }
        private async void ButtonApprove_Clicked(object sender, EventArgs e)
        {
            if (_identifier == 1)
            {
                _replacement.BeforeReport.PicturePath = _filePath;
            } else if (_identifier == 2)
            {
                _replacement.OldMeter.PicturePath = _filePath;
            }
            else if (_identifier == 3)
            {
                _replacement.NewMeter.PicturePath = _filePath;
            }
            else if (_identifier == 4)
            {
                _replacement.AfterReport.PicturePath = _filePath;
            }
            await Navigation.PopAsync();
        }

        private async void takePhoto()
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
                Directory = _Directory,
                Name = _replacement.Id + " " + DateTime.Now + _Name,
                PhotoSize = PhotoSize.Small,
                //Save to album makes the photo visable in your gallary app
                SaveToAlbum = true
            });

            if (file == null)
                return;
            //Shows a alert with the full path to the picture, it's only meant to be used for debugging
            _filePath = file.Path;
            //await DisplayAlert("File Location", _filePath, "OK");

            image.Source = ImageSource.FromStream(() =>
                        {
                            var stream = file.GetStream();
                            file.Dispose();
                            return stream;
                        });
        }
        private void SetDirectoryAndFilename()
        {
            if (_identifier == 1)
            {
                _Directory = "OldInstalation";
                _Name = " FULL.jpg";
            }
            else if (_identifier == 4)
            {
                _Directory = "NewInstalation";
                _Name = " FULL.jpg";
            }
            else if (_identifier == 2)
            {
                _Directory = "OldMeter";
                _Name = " Meter.jpg";
            }
            else if (_identifier == 3)
            {
                _Directory = "NewMeter";
                _Name = " Meter.jpg";
            }
        }
    }
}
