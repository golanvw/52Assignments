using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _52Assignments.MVVM.Models
{
    [SQLite.Table("Rating")]
    public class Rating
    {
        [PrimaryKey, AutoIncrement]
        public int RatingId { get; set; }
        public int SubmissionID { get; set; }
        public int Score { get; set; }
    }
}
