using _52Assignments.Data;
using _52Assignments.MVVM.Views;
using Plugin.LocalNotification;

namespace _52Assignments
{
    public partial class App : Application
    {
        public static AppDatabase Database { get; private set; }
        public App()
        {
            InitializeComponent();
            InitializeDatabase();
            if (SecureStorage.GetAsync("IsLoggedIn").Result == "true")
            {
                MainPage = new NavigationPage(new TabbedNavPage());
            }
            else
            {
                MainPage = new NavigationPage(new MainPage());
            }
            Task.Run(async () =>
            {
                while (true) // Oneindige lus
                {
                    var notification = new NotificationRequest
                    {
                        NotificationId = 1004,
                        Title = "Herinnering",
                        Description = "Dit is een melding die elke minuut verschijnt!",
                        Schedule = new NotificationRequestSchedule
                        {
                            NotifyTime = DateTime.Now.AddSeconds(5)
                        }
                    };

                    LocalNotificationCenter.Current.Show(notification);
                    await Task.Delay(TimeSpan.FromMinutes(1));
                }
            });
        }

        private void InitializeDatabase()
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "52Assignments.db");
            Database = new AppDatabase(dbPath);
        }



        //protected override Window CreateWindow(IActivationState? activationState)
        //{
        //    return new Window(new AppShell());
        //}
    }
}