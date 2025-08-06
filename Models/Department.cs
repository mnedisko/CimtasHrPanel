namespace CimtasHrPanel.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        public ICollection<Person> Persons { get; set; }
        public ICollection<LeaveRequest> LeaveRequests { get; set; }
    }
}