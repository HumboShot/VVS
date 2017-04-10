using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVS.Model
{
    class Location
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }

        public Location(int id, string address, float longitude, float latitude)
        {
            Id = id;
            Address = address;
            Longitude = longitude;
            Latitude = latitude;
        }
        public Location()
        {
        }
    }
}
