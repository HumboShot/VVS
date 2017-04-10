using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVS.Model
{
    class Meter
    {
        public int SerialNumber { get; set; }
        public double Consumtion { get; set; }
        public string PicturePath { get; set; }

        public Meter(int serialNumber, double consumtion, string picturePath)
        {
            SerialNumber = serialNumber;
            Consumtion = consumtion;
            PicturePath = picturePath;
        }

        public Meter()
        {
        }
    }
}
