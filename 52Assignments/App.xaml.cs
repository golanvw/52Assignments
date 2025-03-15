using _52Assignments.Data;
using _52Assignments.MVVM.Views;

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