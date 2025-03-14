using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using _52Assignments.MVVM.Models;

namespace _52Assignments.MVVM.Viewmodels
{
    public class RegisterViewModel 
    {
        public ICommand RegisterCommand { get; set; }
        private string _name;
        public string Name 
        { 
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                }
            }
        }
        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                }
            }
        }
        private async Task RegisterAsync()
        {
            var database = App.Database;
            if (database.GetUserByName(Name) == null)
            {
                User user = new User
                {
                    UserName = Name,
                    Password = Password,
                    Role = "Free",
                    Points = 5,
                    Frequency = "Daily"
                };
                await database.AddUserAsync(user);
                Debug.WriteLine("Gebruiker is toegevoegd");
                Application.Current.MainPage.DisplayAlert("Prima", "Uw account is aangemaakt, login nu in", "Ok");
                Application.Current.MainPage = new NavigationPage(new MainPage());
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Fout", "Deze username wordt al gebruikt, kies een andere.", "Ok");
            }
        }

        public RegisterViewModel() 
        { 
            RegisterCommand = new Command(async () => await RegisterAsync());
        }
    }
}
