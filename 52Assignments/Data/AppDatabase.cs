using _52Assignments.MVVM.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace _52Assignments.Data
{
    public class AppDatabase
    {
        private readonly SQLiteAsyncConnection _database;
        public AppDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Assignment>().Wait();
            _database.CreateTableAsync<Comment>().Wait();
            _database.CreateTableAsync<Rating>().Wait();
            _database.CreateTableAsync<Submission>().Wait();
            _database.CreateTableAsync<Theme>().Wait();
            _database.CreateTableAsync<User>().Wait();
            _database.CreateTableAsync<ThemeUser>().Wait();
        }

        public async Task SeedDatabase()
        {
            var themes = await _database.Table<Theme>().ToListAsync();
            if (themes == null)
            {
                var newTheme1 = new Theme { ThemeName = "Autos" };
                await _database.InsertAsync(newTheme1);
                var newTheme2 = new Theme { ThemeName = "Gebouwen" };
                await _database.InsertAsync(newTheme2);
                var newTheme3 = new Theme { ThemeName = "Natuur" };
                await _database.InsertAsync(newTheme3);
            }
            var assignments = await _database.Table<Assignment>().ToListAsync();
            if (assignments == null)
            {
                var newAssignment1 = new Assignment { Name = "Mercedes", ThemeId = 1 };
                await _database.InsertAsync(newAssignment1);
                var newAssignment2 = new Assignment { Name = "Audi", ThemeId = 1 };
                await _database.InsertAsync(newAssignment2);
                var newAssignment3 = new Assignment { Name = "Mini", ThemeId = 1 };
                await _database.InsertAsync(newAssignment3);
                var newAssignment4 = new Assignment { Name = "Fiat", ThemeId = 1 };
                await _database.InsertAsync(newAssignment4);
                var newAssignment5 = new Assignment { Name = "Huis", ThemeId = 2 };
                await _database.InsertAsync(newAssignment5);
                var newAssignment6 = new Assignment { Name = "Bushalte", ThemeId = 2 };
                await _database.InsertAsync(newAssignment6);
                var newAssignment7 = new Assignment { Name = "Winkel", ThemeId = 2 };
                await _database.InsertAsync(newAssignment7);
                var newAssignment8 = new Assignment { Name = "Tunnel", ThemeId = 2 };
                await _database.InsertAsync(newAssignment8);
                var newAssignment9 = new Assignment { Name = "Boom", ThemeId = 3 };
                await _database.InsertAsync(newAssignment9);
                var newAssignment10 = new Assignment { Name = "Banaan", ThemeId = 3 };
                await _database.InsertAsync(newAssignment10);
                var newAssignment11 = new Assignment { Name = "Bosje", ThemeId = 3 };
                await _database.InsertAsync(newAssignment11);
                var newAssignment12 = new Assignment { Name = "Grasveld", ThemeId = 3 };
                await _database.InsertAsync(newAssignment12);
            }
            var users = _database.Table<User>();
            if(users == null)
            {
                var newUser1 = new User { UserName = "Golan1", Password = "Golan1", Role = "Free", Points = 5, Frequency = "Daily" };
                await _database.InsertAsync(newUser1);
                var newUser2 = new User { UserName = "GolanSuper", Password = "GolanSuper", Role = "Super", Points = 5, Frequency = "Daily" };
                await _database.InsertAsync(newUser2);
                var newUser3 = new User { UserName = "GolanAdmin", Password = "GolanAdmin", Role = "Admin", Points = 5, Frequency = "Daily" };
                await _database.InsertAsync(newUser3);
                var newThemeUser1 = new ThemeUser { ThemeId = 1, UserId = 1 };
                await _database.InsertAsync(newThemeUser1);
                var newThemeUser2 = new ThemeUser { ThemeId = 2, UserId = 1 };
                await _database.InsertAsync(newThemeUser2);
                var newThemeUser3 = new ThemeUser { ThemeId = 2, UserId = 2 };
                await _database.InsertAsync(newThemeUser3);
                var newThemeUser4 = new ThemeUser { ThemeId = 3, UserId = 2 };
                await _database.InsertAsync(newThemeUser4);
            }
        }

        public async Task AddUserAsync(User user)
        {
            await _database.InsertAsync(user);
            var newThemeUser = new ThemeUser { ThemeId = 1, UserId = user.UserId };
            await _database.InsertAsync(newThemeUser);
        }

        public async Task AddAssignment(Assignment assignment)
        {
            await _database.InsertAsync(assignment);
        }

        public async Task AddCommentAsync(Comment comment)
        {
            await _database.InsertAsync(comment);
        }

        public async Task RatingAsync(Rating rating)
        {
            await _database.InsertAsync(rating);
        }
        public async Task AddSubmissionAsync(Submission submission)
        {
            await _database.InsertAsync(submission);
        }
        public async Task<User> GetUserByName(string name)
        {
            return await _database.Table<User>().FirstOrDefaultAsync(u => u.UserName == name);
        }

        public async Task<User> GetUserById(int id)
        {
            return await _database.Table<User>().FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task AddPointsToUser(int points)
        {
            var UserId = SecureStorage.GetAsync("userId").Result;
            var CurrentUser = await GetUserById(int.Parse(UserId));
            CurrentUser.Points = CurrentUser.Points + points;
        }
    }
}
