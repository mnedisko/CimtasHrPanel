namespace CimtasHrPanel.Models
{
    public class LeaveHistoryViewModel
    {
        public string PersonName { get; set; }
        public string DepartmentName { get; set; }
        public string LeaveTypeName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DurationDays { get; set; }
        public string Status { get; set; }
        public bool IsOverLeaveUsed { get; set; }
        public int PersonMaxAnnualLeaveLimit { get; set; }
        public int PersonTotalAnnualLeaveLimit { get; set; }
    }
}