using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace _52Assignments.MVVM.Viewmodels
{
    public class ShopViewModel : INotifyPropertyChanged
    {
        public ICommand BuyPoint1Command { get; set; }
        public ICommand BuyPoints10Command { get; set; }

        private int _pointAmount { get; set; }
        public int PointAmount
        {
            get => _pointAmount;
            set
            {
                _pointAmount = value;
                OnPropertyChanged(nameof(_pointAmount));
            }
        }


        public ShopViewModel()
        {
            GetPointsFromUser();
            BuyPoint1Command = new Command(async => AddPoints(1));
            BuyPoints10Command = new Command(async => AddPoints(10));
        }
        
        public async Task AddPoints(int amount)
        {
            var database = App.Database;
            await database.AddPointsToUser(amount);
            await GetPointsFromUser();
        }
        
        public async Task GetPointsFromUser()
        {
            var database = App.Database;
            var userid = int.Parse(SecureStorage.GetAsync("userId").Result);
            var CurrentUser = await database.GetUserById(userid);
            PointAmount = CurrentUser.Points;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
