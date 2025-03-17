using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using _52Assignments.MVVM.Models;
using System.Diagnostics;


namespace _52Assignments.MVVM.Viewmodels
{
    public partial class CameraViewModel : ObservableObject
    {
        [ObservableProperty]
        private ImageSource photoSource;

        private string _assignmentName;
        public string AssignmentName
        {
            get => _assignmentName;
            set
            {
                if (_assignmentName != value)
                {
                    
                    _assignmentName = value;
                    OnPropertyChanged(nameof(AssignmentName));
                }
            }
        }

        public CameraViewModel()
        {
            TakePhotoCommand = new AsyncRelayCommand(TakePhotoAsync);
            GetAssignmentCommand = new Command(async => GetCurrentAssignment());
            GetCurrentAssignment();
        }

        public ICommand TakePhotoCommand { get; }
        public ICommand GetAssignmentCommand { get; }

        public Assignment CurrentAssignment { get; set; }

        public async Task GetCurrentAssignment()
        {
            Debug.WriteLine("deze functie wordt uitgevoerd");
            var database = App.Database;
            var currentAssignment = await database.GetAssignment();
            AssignmentName = currentAssignment.Name;
            CurrentAssignment = currentAssignment;
        }

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
                var database = App.Database;
                Submission newSubmission = new Submission
                {
                    AssignmentName = CurrentAssignment.Name,
                    UserId = int.Parse(SecureStorage.GetAsync("userId").Result),
                    ImagePath = localFilePath
                };
                await database.AddSubmissionAsync(newSubmission);

                // Update de ImageSource voor databinding
                PhotoSource = ImageSource.FromFile(localFilePath);
                }
        }



    }
}
