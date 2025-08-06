namespace CimtasHrPanel.Models
{
    public class LeaveRequest
    {
        public int Id { get; set; }
        public Person PersonId { get; set; }
        public Department PersonDepartment { get; set; }
        public DateTime LeaveTime { get; set; }
        public DateTime EntryTime { get; set; }

    }
}