using Microsoft.EntityFrameworkCore;

namespace CimtasHrPanel.Models
{
 
    public class LeaveSettings
    {
        public int Id { get; set; }
        public int MaxAnnualLeaveLimit { get; set; }
    }
}