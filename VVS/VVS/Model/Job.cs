using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVS.Model
{
    class Job
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public int OldMeterId { get; set; }
        public int NewMeterId { get; set; }
        public DateTime  Time { get; set; }
        public int LocId { get; set; }
        public bool Status { get; set; }
        public int EmployeeId { get; set; }

      

        public Job()
        {
            
        }

        public Job(int id, string customerName, int oldMeterId, int newMeterId, DateTime time, int locId, bool status, int employeeId)
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
    }
}
