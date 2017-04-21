using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVS.Model
{
    public class GenerateData
    {
        public List<Meter> ListMeters { get; set; }
        public List<Replacement> ListReplacements { get; set; }
        public List<Location> ListLocations { get; set; }

        public GenerateData()
        {
            ListMeters = new List<Meter>();
            ListLocations = new List<Location>();
            ListReplacements = new List<Replacement>();
            
            ListLocations.Add(new Location("Sofiendalsvej 60, 9200 Aalborg SV", 57.021055, 9.885131000000001));
            ListLocations.Add(new Location("Vandværksvej 38, 9800 Hjørring", 57.43890809999999, 10.007306400000061));
            ListLocations.Add(new Location("Harald Jensens Vej 9, 9000 Aalborg", 57.0517669, 9.89858819999995));

            for (int i = 0; i < 3; i++)
            {
                ListMeters.Add(new Meter(10000000 + i));                
            }

            ListReplacements.Add(new Replacement("UCN Aalborg - Campus Sofiendalsvej", 10000001, DateTime.Now.AddHours(2), 1, 0, 1));
            ListReplacements.Add(new Replacement("Hjørring Rørteknik A/S", 10000002, DateTime.Now.AddHours(3), 2, 0, 1));
            ListReplacements.Add(new Replacement("Aalborg Stadion", 10000003, DateTime.Now.AddHours(4), 3, 0, 1));
            
        }
    }
}
