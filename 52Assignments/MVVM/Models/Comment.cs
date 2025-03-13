using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _52Assignments.MVVM.Models
{
    [SQLite.Table("Comment")]
    public class Comment
    {
        [PrimaryKey, AutoIncrement]
        public int CommentId { get; set; }
        public string Text { get; set; }
        public int UserId { get; set; }
        public int AssignmentId { get; set; }
    }
}
