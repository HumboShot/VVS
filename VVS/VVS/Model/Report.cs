using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace VVS.Model
{
    public class Report
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string PicturePath { get; set; }
        public string Comment { get; set; }
        public int MyProperty { get; set; }
        public DateTime Time { get; set; }
    }
}
