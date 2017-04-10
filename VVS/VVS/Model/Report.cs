using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVS.Model
{
    class Report
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public string PicturePath { get; set; }
        public DateTime Time { get; set; }

        public Report(int id, string comment, string picturePath, DateTime time)
        {
            Id = id;
            Comment = comment;
            PicturePath = picturePath;
            Time = time;
        }

        public Report()
        {
        }
    }
}
