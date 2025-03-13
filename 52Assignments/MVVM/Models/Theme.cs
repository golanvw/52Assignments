using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _52Assignments.MVVM.Models
{
    [SQLite.Table("Theme")]
    public class Theme
    {
        [PrimaryKey, AutoIncrement]
        public int ThemeId { get; set; }
        public string ThemeName { get; set; }
    }
}
