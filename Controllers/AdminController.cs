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
        public IActionResult AddDepartment(string departmentName)
        {
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
