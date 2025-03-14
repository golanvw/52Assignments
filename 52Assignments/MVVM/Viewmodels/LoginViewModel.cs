using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using _52Assignments.MVVM.Views;

namespace _52Assignments.MVVM.Viewmodels
{
    public class LoginViewModel
    {
        public ICommand LoginCommand { get; set; }
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
        private async Task LoginAsync()
        {
            var database = App.Database;
            var LoggingInUser = await database.GetUserByName(Name);
            if (LoggingInUser != null)
            { 
                if(Password == LoggingInUser.Password)
                {
                    await SecureStorage.SetAsync("userId", LoggingInUser.UserId.ToString());
                    await SecureStorage.SetAsync("IsLoggedIn", "true");
                    Application.Current.MainPage = new NavigationPage(new HomePage());
                    Debug.WriteLine("inloggen succesvol");
                    Application.Current.MainPage.DisplayAlert("gelukt", "u bent succesvol ingelogd", "ok");
                }
            }
        }
        public LoginViewModel()
        {
            LoginCommand = new Command(async () => await LoginAsync());
        }
    }
}
