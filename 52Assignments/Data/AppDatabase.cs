using _52Assignments.MVVM.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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
            SeedDatabase();
        }

        public async Task SeedDatabase()
        {
            var themes = await _database.Table<Theme>().ToListAsync();
            if (themes == null)
            {
                Debug.WriteLine("seeden van themes wordt gedaan");
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
                Debug.WriteLine("seeden van assignments wordt gedaan");
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
                Debug.WriteLine("seeden van users en themeusers wordt gedaan");
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
            Debug.WriteLine("seeden gedaan");
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

        public async Task AddThemeUserAsync(ThemeUser themeUser)
        {
            await _database.InsertAsync(themeUser);
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

        public async Task<List<Submission>> GetUserSubmissions()
        {
            var userId = int.Parse(SecureStorage.GetAsync("userId").Result);
            var currentUser = await GetUserById(userId);
            return await _database.Table<Submission>().Where(u => u.UserId == currentUser.UserId).ToListAsync();
        }

        public async Task AddPointsToUser(int points)
        {
            var UserId = SecureStorage.GetAsync("userId").Result;
            var CurrentUser = await GetUserById(int.Parse(UserId));
            CurrentUser.Points = CurrentUser.Points + points;
            await _database.UpdateAsync(CurrentUser);
        }

        public async Task<Assignment> GetAssignment()
        {
            var currentUserId = int.Parse(SecureStorage.GetAsync("userId").Result);
            var userThemes = await _database.Table<ThemeUser>().Where(u => u.UserId == currentUserId).ToListAsync();
            List<Assignment> assignments = new List<Assignment>();
            
            foreach (var theme in userThemes)
            {
                var AssignmentList = await _database.Table<Assignment>().Where(u => u.ThemeId == theme.ThemeId).ToListAsync();
                foreach (var assignment in AssignmentList)
                {
                    assignments.Add(assignment);
                }
            }
            var random = new Random();
            var randomNumber = random.Next(0, assignments.Count);
            var currentAssignment = assignments[randomNumber];
            return await _database.Table<Assignment>().FirstOrDefaultAsync(u => u.AssignmentId == currentAssignment.AssignmentId);

        }

        public async Task<Submission> GetSubmissionById(int id)
        {
            return await _database.Table<Submission>().Where(s => s.SubmissionId == id).FirstOrDefaultAsync();
        }

        public async Task<List<Comment>> GetComments(int id)
        {
            return await _database.Table<Comment>().Where(u => u.SubmissionId == id).ToListAsync();
        }

        public async Task<Comment> GetComment(int id)
        {
            return await _database.Table<Comment>().Where(u => u.CommentId == id).FirstOrDefaultAsync();
        }
        
        public async Task DeleteComment(int id)
        {
            var comment = await GetComment(id);
            await _database.DeleteAsync(comment);
        }

    }
}
