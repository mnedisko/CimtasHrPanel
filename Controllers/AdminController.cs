using System.Data.Entity;
using CimtasHrPanel.Models;
using Microsoft.AspNetCore.Mvc;

namespace CimtasHrPanel.Controllers
{
    public class AdminController : Controller
    {
        private readonly ProjectDbContext _projectDbContext;
        public AdminController(ProjectDbContext projectDbContext)
        {
            _projectDbContext = projectDbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddLeaveType(string leaveTypeName,bool isIncreaseAnnualValue)
        {
            var recurringTypeName = _projectDbContext.LeaveTypes.FirstOrDefault(lt => lt.LeaveTypeName == leaveTypeName);

            if (leaveTypeName.Length <= 3)
            {
                return BadRequest("Lütfen izin türünü 3 karakterden fazla tanımlayın");
            }
            if (recurringTypeName!=null)
            {
                return BadRequest("bu isimle izin türü daha önce kaydedilmiş");
            }
            var newLeaveType = new LeaveType
            {
                LeaveTypeName = leaveTypeName,
                IsIncreaseAnnualValue = isIncreaseAnnualValue

            };
            _projectDbContext.LeaveTypes.Add(newLeaveType);
            _projectDbContext.SaveChanges();


            return Ok("Yeni İzin türü başarı ile eklendi");
        }
        [HttpPost]
        public IActionResult AddDepartment(string departmentName)
        {
            var recurringDepartmentName = _projectDbContext.Departments.FirstOrDefault(d => d.DepartmentName == departmentName);
            if (recurringDepartmentName != null)
            {
                return BadRequest("Bu isimle departman türü zaten tanımlanmış");

            }
            if (departmentName.Length <= 2)
            {
                return BadRequest("Lütfen 3 veya daha fazla karakter girin");
            }
            var newDepartment = new Department
            {
                DepartmentName = departmentName,

            };
            _projectDbContext.Departments.Add(newDepartment);
            _projectDbContext.SaveChanges();


            return Ok("Departman başarı ile eklendi");
        }
        [HttpPost]
        public async Task<IActionResult> UpdateMaxAnnuelLeaveLimit(int newLimit)
        {
            if (newLimit < 1)
            {
                return BadRequest("Lütfen 0dan yüksek bir sayı girin");
            }
            var settings =  _projectDbContext.LeaveSettings.FirstOrDefault();
            if (settings != null)
            {
                settings.MaxAnnualLeaveLimit = newLimit;
                _projectDbContext.LeaveSettings.Update(settings);
            }
            else
            {
                _projectDbContext.LeaveSettings.Add(new LeaveSettings { MaxAnnualLeaveLimit = newLimit });
            }
            await _projectDbContext.SaveChangesAsync();

            return Ok("Maksimum izin günü ayarı başarı ile değiştirildi");
        }
    }
}
