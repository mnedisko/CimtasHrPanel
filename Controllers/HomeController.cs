using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CimtasHrPanel.Models;
using Microsoft.EntityFrameworkCore;

namespace CimtasHrPanel.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ProjectDbContext _projectDbContext;

    public HomeController(ILogger<HomeController> logger, ProjectDbContext projectDbContext)
    {
        _logger = logger;
        _projectDbContext = projectDbContext;
    }

    public IActionResult Index()
    {
        var persons = _projectDbContext.Persons.Select(p => new PersonModelView
        {
            PersonId=p.Id,
            Name = p.PersonName + " " + p.PersonLastName,
            DepartmantName = p.Department.DepartmentName

        }).ToList();
        return View(persons);
    }
    public  async Task<IActionResult> PersonalData(int personId)
    {
        var person = await _projectDbContext.Persons
            .Include(p => p.Department)
            .Include(p => p.LeaveHistory).ThenInclude(lh=>lh.LeaveType)
            .FirstOrDefaultAsync(p => p.Id == personId);

    if (person == null)
        {
            return NotFound("Kullanıcı bulunamadı.");
        }
        var annualLeaveLimit = _projectDbContext.LeaveSettings.FirstOrDefault();
        var totalAnnualLeaveDaysUsed = person.LeaveHistory.Where(lr => lr.LeaveType.IsIncreaseAnnualValue == true).Sum(lr => lr.DurationDays);
        var personmw = new PersonModelView
        {
            PersonId = person.Id,
            Name = person.PersonName + " " + person.PersonLastName,
            DepartmantName = person.Department.DepartmentName,
            TotalAnnualLeaveDaysUsed = totalAnnualLeaveDaysUsed,
            RemainingAnnualLeaveDays = annualLeaveLimit.MaxAnnualLeaveLimit - totalAnnualLeaveDaysUsed,
             leaveHistory=person.LeaveHistory.OrderByDescending(lh=>lh.LeaveTime).ToList()
             
            

            
        };
        
        return View(personmw);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
