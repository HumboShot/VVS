using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace VVS.Model
{
    public class Meter
    {
        [PrimaryKey]
        public int SerialNumber { get; set; }
        public double Consumtion { get; set; }
        public string PicturePath { get; set; }
        public string Comment { get; set; }
        [Ignore]
        public bool SNinDB { get; set; }

        public Meter(int serialNumber, double consumtion, string picturePath, string comment)
        {
            SerialNumber = serialNumber;
            Consumtion = consumtion;
            PicturePath = picturePath;
            Comment = comment;
        }

        public Meter(int serialNumber)
        {
            SerialNumber = serialNumber;
        }

        public Meter()
        {
        }
    }
}
