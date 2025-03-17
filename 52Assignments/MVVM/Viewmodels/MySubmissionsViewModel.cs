using _52Assignments.MVVM.Models;
using _52Assignments.MVVM.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;

namespace _52Assignments.MVVM.Viewmodels
{
    public class MySubmissionsViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Submission> _submissions;
        public ObservableCollection<Submission> Submissions
        {
            get => _submissions;
            set
            {
                if (_submissions != value)
                {
                    _submissions = value;
                    OnPropertyChanged(nameof(Submissions));
                }
            }
        }
        private Submission _selectedSubmission;
        public Submission SelectedSubmission
        { 
            get => _selectedSubmission;
            set
            {
                if (_selectedSubmission != value)
                {
                    _selectedSubmission = value;
                    OnPropertyChanged(nameof(SelectedSubmission));
                }
            }
        }
        public ICommand Refreshcommand { get; set; }
        public ICommand NavigateCommand { get; set; }
        
        public MySubmissionsViewModel()
        {
            LoadSubmissions();
            Refreshcommand = new Command(async => LoadSubmissions());
            NavigateCommand = new Command(async => GoToSubmission());
        }


        public async Task LoadSubmissions()
        {
            var database = App.Database;
            var submissions = await database.GetUserSubmissions();
            foreach (var submission in submissions)
            {
                Debug.WriteLine($"submission id: {submission.SubmissionId} asignmentname: {submission.AssignmentName} userid: {submission.UserId}");
            }
            Submissions = new ObservableCollection<Submission>(submissions);
            SelectedSubmission = Submissions.FirstOrDefault();
        }

        public async Task GoToSubmission()
        {

            Debug.WriteLine($"nu id: {SelectedSubmission.SubmissionId}");
            await SecureStorage.SetAsync("currentSub", SelectedSubmission.SubmissionId.ToString());
            
            if (Application.Current.MainPage is NavigationPage navPage)
            {
                await navPage.Navigation.PushAsync(new SubmissionInfoPage());
            }
            else
            {
                Debug.WriteLine("Navigatie kon niet worden uitgevoerd, MainPage is geen NavigationPage.");
            }
            Debug.WriteLine("en als laatste dit");
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
