using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CimtasHrPanel.Models;
using Microsoft.AspNetCore.Components.Forms;

public class LeaveController : Controller
{
    private readonly ProjectDbContext _projectDbContext;

    public LeaveController(ProjectDbContext dbContext)
    {
        _projectDbContext = dbContext;
    }
    public async  Task<IActionResult> LeaveModule(int? departmentId)
    {
           var viewModel = new LeaveCreateViewModel
        {
            Departments = await _projectDbContext.Departments.ToListAsync(),
            LeaveTypes = await _projectDbContext.LeaveTypes.ToListAsync()
        };

        if (departmentId.HasValue)
        {
            viewModel.SelectedDepartmentId = departmentId.Value;
            viewModel.Persons = await _projectDbContext.Persons
                .Where(p => p.DepartmentId == departmentId.Value)
                .ToListAsync();
        }
        else
        {
            viewModel.Persons = new List<Person>();
        }

        return View(viewModel);
    }
    //Person name ve person last name birleştir

    [HttpPost]
    public IActionResult LeaveModule(int personId,int leaveTypeId,DateTime StartDate,DateTime EndDate)
    {
        var person = _projectDbContext.Persons.FirstOrDefault(p => p.Id == personId);
        /* var leaveTypeName = _projectDbContext.LeaveTypes.FirstOrDefault(l => l.Id == leaveTypeId); */

       var leaveRequest = new LeaveRequest
        {
            PersonId = personId,
            LeaveTime = StartDate,
            EntryTime = EndDate,
            LeaveTypeId = leaveTypeId,
            DurationDays = CalculateBusinessDays(StartDate,EndDate)
        };
        ViewBag.Persons = leaveRequest;
        _projectDbContext.LeaveRequests.Add(leaveRequest);
        _projectDbContext.SaveChanges();


        return Ok("İzin başarı ile kaydedildi");
    }
    private int CalculateBusinessDays(DateTime leave, DateTime entry)
{
    int businessDays = 0;
    for (var date = leave; date <= entry; date = date.AddDays(1))
    {
        if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
        {
            businessDays++;
        }
    }
    return businessDays;
}
    public async Task<IActionResult> Index(int? departmentId, DateTime? startDate, DateTime? endDate)
    {
        ViewBag.Departments = _projectDbContext.Departments.ToList();

        var query = _projectDbContext.LeaveRequests
            .Include(lr => lr.Person)
                .ThenInclude(p => p.Department)
            .Include(lr => lr.LeaveType)
            .AsQueryable();

        // 1. Departman bazında filtreleme
        if (departmentId.HasValue && departmentId.Value > 0)
        {
            query = query.Where(lr => lr.Person.DepartmentId == departmentId.Value);
        }

        // 2. Tarih filtresiyle sorgulama
        if (startDate.HasValue)
        {
            query = query.Where(lr => lr.LeaveTime >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(lr => lr.EntryTime <= endDate.Value);
        }

        var leaveHistory = await query
            .Select(lr => new LeaveHistoryViewModel
            {
                PersonName = $"{lr.Person.PersonName} {lr.Person.PersonLastName}",
                DepartmentName = lr.Person.Department.DepartmentName,
                LeaveTypeName = lr.LeaveType.LeaveTypeName,
                StartDate = lr.LeaveTime,
                EndDate = lr.EntryTime,
                DurationDays = lr.DurationDays,
                Status = lr.status,
            })
            .ToListAsync();

        // 3. Aşırı izin kullanımına uyarı (yıllık izin sınırı kontrolü)
        var annualLeaveLimit = 20; // Parametrik olarak ayarlanabilir
        foreach (var person in leaveHistory.GroupBy(x => x.PersonName))
        {
            var totalAnnualLeave = await _projectDbContext.LeaveRequests
                .Where(lr => lr.Person.PersonName + " " + lr.Person.PersonLastName == person.Key && lr.LeaveType.IsIncreaseAnnualValue)
                .SumAsync(lr => lr.DurationDays);

            if (totalAnnualLeave > annualLeaveLimit)
            {
                foreach (var leave in leaveHistory.Where(x => x.PersonName == person.Key))
                {
                    leave.IsOverLeaveUsed = true;
                }
            }
        }

        return View(leaveHistory);
    }
}