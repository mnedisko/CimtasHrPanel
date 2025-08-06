namespace CimtasHrPanel.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public Department PersonDepartment { get; set; }

        
    }
}