using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;


namespace _52Assignments.MVVM.Viewmodels
{
    public partial class CameraViewModel : ObservableObject
    {
        [ObservableProperty]
        private ImageSource photoSource;

        public CameraViewModel()
        {
            TakePhotoCommand = new AsyncRelayCommand(TakePhotoAsync);
        }

        public ICommand TakePhotoCommand { get; }

        private async Task TakePhotoAsync()
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                var photo = await MediaPicker.Default.CapturePhotoAsync();
                if (photo == null) return;

                string localFilePath = Path.Combine(FileSystem.AppDataDirectory, photo.FileName);

                using (var sourceStream = await photo.OpenReadAsync())
                using (var localFile = File.OpenWrite(localFilePath))
                {
                    await sourceStream.CopyToAsync(localFile);
                }

                // Update de ImageSource voor databinding
                PhotoSource = ImageSource.FromFile(localFilePath);
                }
        }



    }
}
