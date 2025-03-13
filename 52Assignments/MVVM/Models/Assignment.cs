using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _52Assignments.MVVM.Models
{
    [SQLite.Table("Assignment")]
    public class Assignment
    {
        [PrimaryKey, AutoIncrement]
        public int AssignmentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ThemeId { get; set; }
    }
}
