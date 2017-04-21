using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VVS.Layout
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OldMeter : ContentPage
    {
        public OldMeter()
        {
            InitializeComponent();
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
                    Directory = "OldInstallion",
                    Name = DateTime.Now + " FULL.jpg",
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
                    Directory = "OldInstallion",
                    Name = DateTime.Now + " METER.jpg",
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
    }
}
