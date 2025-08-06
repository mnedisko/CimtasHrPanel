using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CimtasHrPanel.Models;
using Microsoft.Extensions.Options;

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
    private string CalcuteLeaveStatus(DateTime leave, DateTime entry)
    {
        string status = "";
        DateTime today = DateTime.Now;
        if (today >= leave && today <= entry)
        {
            status = "İzinli";
        }
        else
        {
            status = "İzinli değil";
        }
        return status;
    }
        public async Task<IActionResult> Index(int? departmentId, DateTime? startDate, DateTime? endDate)
    {
        var settings = await _projectDbContext.LeaveSettings.FirstOrDefaultAsync();
        
        ViewBag.Departments = await _projectDbContext.Departments.ToListAsync();

        
        var allLeaveRequests = await _projectDbContext.LeaveRequests
            .Include(lr => lr.Person)
            .ThenInclude(p => p.Department)
            .Include(lr => lr.LeaveType)
            .ToListAsync();

       
        var filteredRequests = allLeaveRequests.AsEnumerable();

       
        if (departmentId.HasValue && departmentId.Value > 0)
        {
            filteredRequests = filteredRequests.Where(lr => lr.Person.DepartmentId == departmentId.Value);
        }

        
        if (startDate.HasValue)
        {
            filteredRequests = filteredRequests.Where(lr => lr.LeaveTime >= startDate.Value);
        }

       
        if (endDate.HasValue)
        {
            filteredRequests = filteredRequests.Where(lr => lr.EntryTime <= endDate.Value);
        }

       
        var leaveRequestsList = filteredRequests.ToList();
        var result = new List<LeaveHistoryViewModel>();

        foreach (var request in leaveRequestsList)
        {
           
            var totalAnnualLeaveDays = allLeaveRequests
                .Where(lr => lr.PersonId == request.PersonId && lr.LeaveType.IsIncreaseAnnualValue==true)
                .Sum(lr => lr.DurationDays);

            
            var viewModel = new LeaveHistoryViewModel
            {
                PersonName = request.Person.PersonName + " " + request.Person.PersonLastName,
                DepartmentName = request.Person.Department.DepartmentName,
                LeaveTypeName = request.LeaveType.LeaveTypeName,
                StartDate = request.LeaveTime,
                EndDate = request.EntryTime,
                DurationDays = request.DurationDays,
                Status = CalcuteLeaveStatus(request.LeaveTime, request.EntryTime),
                IsOverLeaveUsed = totalAnnualLeaveDays > settings.MaxAnnualLeaveLimit,
                PersonMaxAnnualLeaveLimit = settings.MaxAnnualLeaveLimit ,
                PersonTotalAnnualLeaveLimit = settings.MaxAnnualLeaveLimit - totalAnnualLeaveDays,
                
                
                
            };

            result.Add(viewModel);
        }

        var sortedResult = result.OrderByDescending(x => x.StartDate).ToList();

        return View(sortedResult);
    }
}