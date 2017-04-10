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
        public int OldMeterId { get; set; }
        public int NewMeterId { get; set; }
        public int BeforeReportId { get; set; }
        public int AfterReportId { get; set; }
        public int LocId { get; set; }
        public bool Status { get; set; }

        public Job(int id, int oldMeterId, int newMeterId, int beforeReportId, int afterReportId, int locId, bool status)
        {
            Id = id;
            OldMeterId = oldMeterId;
            NewMeterId = newMeterId;
            BeforeReportId = beforeReportId;
            AfterReportId = afterReportId;
            LocId = locId;
            Status = status;
        }

        public Job()
        {
            
        }
    }
}
