using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace VVS.Model
{
    public class Replacement
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public int OldMeterId { get; set; }
        public int NewMeterId { get; set; }
        public DateTime  Time { get; set; }
        public int LocId { get; set; }
        [Ignore]
        public Location Location { get; set; }
        //Status 0 = not started, -1 job not possible, 1 location confirmed, 2 before report registered, 3 oldmeter registered,
        //4 newmeter registered, 5 after report registered, 6 Job completed.
        public int Status { get; set; }
        public int EmployeeId { get; set; }

      

        public Replacement()
        {
            
        }

        public Replacement(int id, string customerName, int oldMeterId, int newMeterId, DateTime time, int locId, int status, int employeeId)
        {
            Id = id;
            CustomerName = customerName;
            OldMeterId = oldMeterId;
            NewMeterId = newMeterId;
            Time = time;
            LocId = locId;
            Status = status;
            EmployeeId = employeeId;
        }

        public Replacement(string customerName, int oldMeterId, DateTime time, int locId, int status, int employeeId)
        {
            CustomerName = customerName;
            OldMeterId = oldMeterId;            
            Time = time;
            LocId = locId;
            Status = status;
            EmployeeId = employeeId;
        }
    }
}
