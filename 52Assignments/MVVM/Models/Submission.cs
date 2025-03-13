using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _52Assignments.MVVM.Models
{
    [SQLite.Table("Submission")]
    public class Submission
    {
        [PrimaryKey, AutoIncrement]
        public int SubmissionId { get; set; }
        public string AssignmentName { get; set; }
        public int UserId { get; set; }
        public string ImagePath { get; set; }
    }
}
