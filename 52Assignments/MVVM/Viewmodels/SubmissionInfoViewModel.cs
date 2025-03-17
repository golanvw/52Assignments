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
    public class SubmissionInfoViewModel : INotifyPropertyChanged
    {
        private string _imagePath;
        public string ImagePath
        { 
            get => _imagePath;
            set
            {
                if (_imagePath != value)
                {
                    _imagePath = value;
                    OnPropertyChanged(nameof(ImagePath));
                }
            }
        }
        private string _assignmentName;
        public string AssignmentName
        {
            get => _assignmentName;
            set
            {
                if (_assignmentName != value)
                {
                    _assignmentName = value;
                    OnPropertyChanged(nameof(AssignmentName));
                }
            }
        }
        private string _rating;
        public string Rating
        {
            get => _rating;
            set
            {
                if (_rating != value)
                {
                    _rating = value;
                }
            }
        }
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

        private string _userName;
        public string UserName
        {
            get => _userName;
            set
            {
                if (_userName != value)
                {
                    _userName = value;
                }
            }
        }
        private string _commentText;
        public string CommentText
        {
            get => _commentText;
            set
            {
                if (_commentText != value)
                {
                    _commentText = value;
                }
            }
        }

        private ObservableCollection<Comment> _comments;
        public ObservableCollection<Comment> Comments
        {
            get => _comments;
            set
            {
                if (_comments != value)
                {
                    _comments = value;
                    OnPropertyChanged(nameof(Comments));
                }
            }
        }
        private Comment _selectedComment;
        public Comment SelectedComment
        {
            get => _selectedComment;
            set
            {
                if (_selectedComment != value)
                {
                    _selectedComment = value;
                    OnPropertyChanged(nameof(SelectedComment));
                }
            }
        }
        private string _newComment;
        public string NewComment
        {
            get => _newComment;
            set
            {
                if (_newComment != value)
                {
                    _newComment = value;
                    OnPropertyChanged(nameof(NewComment));
                }
            }
        }

        public ICommand DeleteCommentCommand { get; set; }
        public ICommand AddCommentCommand { get; set; }
        public SubmissionInfoViewModel()
        {
            GetSubmissionInfo();
            DeleteCommentCommand = new Command(async => DeleteComment());
            AddCommentCommand = new Command(async => AddComment());
        }

        public async Task GetSubmissionInfo()
        {
            var database = App.Database;
            var currentSubmissionId = int.Parse(SecureStorage.GetAsync("currentSub").Result);
            var currentSubmission = await database.GetSubmissionById(currentSubmissionId);
            ImagePath = currentSubmission.ImagePath;
            AssignmentName = currentSubmission.AssignmentName;
            var userFromSub = await database.GetUserById(currentSubmission.UserId);
            Name = userFromSub.UserName;
            await LoadComments(currentSubmission.SubmissionId);

        }
        
        public async Task LoadComments(int id)
        {
            
            var database = App.Database;
            var comments = await database.GetComments(id);
            Comments = new ObservableCollection<Comment>(comments);
            

            SelectedComment = Comments.FirstOrDefault();
        }

        public async Task DeleteComment()
        {
            var database = App.Database;
            await database.DeleteComment(SelectedComment.CommentId);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task AddComment()
        {
            Debug.WriteLine($"{SecureStorage.GetAsync("currentSub").Result}");
            var database = App.Database;
            Comment newComment = new Comment
            {
                Text = NewComment,
                UserId = int.Parse(SecureStorage.GetAsync("userId").Result),
                SubmissionId = int.Parse(SecureStorage.GetAsync("currentSub").Result)
            };
            Debug.WriteLine("dit wordt gedaan");
            Debug.WriteLine($"text: {newComment.Text} userid: {newComment.UserId} subid: {newComment.SubmissionId}");
            await database.AddCommentAsync(newComment);
            NewComment = string.Empty;
            await LoadComments(int.Parse(SecureStorage.GetAsync("currentSub").Result));
        }
    }
}
