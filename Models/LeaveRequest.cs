namespace CimtasHrPanel.Models
{
    public class LeaveRequest
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }
        public int LeaveTypeId { get; set; }
        public LeaveType LeaveType { get; set; }
        public DateTime LeaveTime { get; set; }
        public DateTime EntryTime { get; set; }

    }
}