using System.ComponentModel.DataAnnotations;

namespace CimtasHrPanel.Models
{
    public class LeaveCreateViewModel
    {
        [Required(ErrorMessage = "Lütfen bir personel seçiniz")]
        public int? PersonId { get; set; }
        
        public int LeaveTypeId { get; set; }
        [Required(ErrorMessage = "Lütfen izin başlangıç tarihi seçiniz")]
        public DateTime StartDate { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Lütfen İzin bitiş tarihi seçiniz")]
        public DateTime EndDate { get; set; } = DateTime.Now;
        public List<Department> Departments { get; set; }
        [Required(ErrorMessage = "Lütfen Bir personel Seçiniz")]
        public List<Person> Persons { get; set; }
        public List<LeaveType> LeaveTypes { get; set; }
        public int SelectedDepartmentId { get; set; }
        public string? WarningMessages { get; set; }
    }
}