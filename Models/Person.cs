namespace CimtasHrPanel.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string PersonName { get; set; }
        public string PersonLastName { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public ICollection<LeaveRequest> LeaveRequests { get; set; }

        
    }
}