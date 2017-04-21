using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace VVS.Model
{
    public class Location
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        

        public Location(int id, string address, double latitude, double longitude)
        {
            Id = id;
            Address = address;
            Longitude = longitude;
            Latitude = latitude;
        }

        public Location(string address, double latitude, double longitude)
        {
            Address = address;
            Longitude = longitude;
            Latitude = latitude;
        }

        public Location()
        {
        }
    }
}
