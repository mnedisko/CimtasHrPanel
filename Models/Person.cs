namespace CimtasHrPanel.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string PersonName { get; set; }
        public string PersonLastName { get; set; }
        public int DepartmentId { get; set; }
        public int MaxAnnualLeaveLimit { get; set; } = 20;
        public Department Department { get; set; }
        public List<LeaveRequest> LeaveHistory { get; set; }

        
    }
}