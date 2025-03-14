using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _52Assignments.MVVM.Models
{
    [SQLite.Table("ThemeUser")]
    public class ThemeUser
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ThemeId { get; set; }
    }
}
