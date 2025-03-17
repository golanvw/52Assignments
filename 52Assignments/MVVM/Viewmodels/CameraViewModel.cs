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
using Newtonsoft.Json;
using System.Text.Json;


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
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Authorization", ApiKey);
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
                await database.AddPointsToUser(1);

                
                PhotoSource = ImageSource.FromFile(localFilePath);
            }
        }

        private const string ApiKey = "JOUW_PEXELS_API_SLEUTEL";
        private const string ApiUrl = "https://api.pexels.com/v1/search?query={0}&per_page=1"; 

        private readonly HttpClient _httpClient;
        
        [ObservableProperty]
        private string searchQuery = "nature"; 

        [ObservableProperty]
        private string photoUrl;

        [RelayCommand]
        //public async Task SearchPhotoAsync()
        //{
        //    if (string.IsNullOrWhiteSpace(SearchQuery))
        //        return;

        //    string url = string.Format(ApiUrl, SearchQuery);

        //    try
        //    {
        //        var response = await _httpClient.GetStringAsync(url);
        //        var json = JsonSerializer.Deserialize<PexelsResponse>(response);

        //        if (json?.Photos.Count > 0)
        //        {
        //            PhotoUrl = json.Photos[0].Src.Medium; // Pak de eerste (en enige) foto
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Fout bij ophalen van foto: {ex.Message}");
        //    }
        //}
    }

    // 📌 Model voor Pexels API response
    public class PexelsResponse
    {
        public List<PexelsPhoto> Photos { get; set; } = new();
    }

    public class PexelsPhoto
    {
        public PexelsSrc Src { get; set; }
    }

    public class PexelsSrc
    {
        public string Medium { get; set; }
    }
}
}
