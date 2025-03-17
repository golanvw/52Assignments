using _52Assignments.MVVM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace _52Assignments.MVVM.Viewmodels
{
    public class AccountViewModel : INotifyPropertyChanged
    {
        private string _name { get; set; }
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(_name));
                }
            }
        }
        private string _frequency { get; set; }
        public string Frequency
        {
            get => _frequency;
            set
            {
                if (_frequency != value)
                {
                    _frequency = value;
                    OnPropertyChanged(nameof(_frequency));
                }
            }
        }
        private string _newAssignment { get; set; }
        public string NewAssignment
        {
            get => _newAssignment;
            set
            {
                if (_newAssignment != value)
                {
                    _newAssignment = value;
                    OnPropertyChanged(nameof(_newAssignment));
                }
            }
        }

        public ICommand SetFreqDailyCommand { get; set; }
        public ICommand SetFreqWeeklyCommand { get; set; }
        public ICommand AddAssignmentCommand { get; set; }
        public AccountViewModel()
        {
            LoadFrequency();
            SetFreqDailyCommand = new Command(async => SetFrequency("Daily"));
            SetFreqWeeklyCommand = new Command(async => SetFrequency("Weekly"));
            AddAssignmentCommand = new Command(async => AddAssignment());
        }

        public async Task SetFrequency(string frequency)
        {
            var database = App.Database;
            var currentUser = await database.GetUserById(int.Parse(SecureStorage.GetAsync("userId").Result));
            await database.SetFrequency(frequency);
            LoadFrequency();
        }

        public async Task LoadFrequency()
        {
            var database = App.Database;
            var currentUser = await database.GetUserById(int.Parse(SecureStorage.GetAsync("userId").Result));
            Frequency = currentUser.Frequency;
            Name = currentUser.UserName;
        }

        public async Task AddAssignment()
        {
            var database = App.Database;
            Assignment newAssignment = new Assignment()
            {
                Name = NewAssignment,
                ThemeId = 1
            };
            await database.AddAssignment(newAssignment);
            await database.AddPointsToUser(5);
            NewAssignment = string.Empty;
            await Application.Current.MainPage.DisplayAlert("Gelukt", "Uw opdracht is ingedient en u krijgt 5 punten", "Ok");

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
