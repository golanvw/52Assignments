using _52Assignments.MVVM.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

        public async Task AddUserAsync(User user)
        {
            await _database.InsertAsync(user);
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
    }
}
