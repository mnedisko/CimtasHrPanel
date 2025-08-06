namespace CimtasHrPanel.Models
{
    public class PersonModelView
    {
        public int PersonId { get; set; }
        public string Name { get; set; }
        public string DepartmantName { get; set; }
        public DateTime LastLeaveDate { get; set; }
        public DateTime LeaveDates { get; set; }
        public bool IsOverLeaveUsed { get; set; }
        public List<LeaveRequest> leaveHistory { get; set; }
        public int TotalAnnualLeaveDaysUsed { get; set; }
        public int RemainingAnnualLeaveDays { get; set; }


    }
}
