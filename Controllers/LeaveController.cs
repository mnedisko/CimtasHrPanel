using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CimtasHrPanel.Models;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
    public async Task<IActionResult> LeaveModule(int personId,int leaveTypeId,DateTime StartDate,DateTime EndDate)
    {
     
     
         var person = await _projectDbContext.Persons.FirstOrDefaultAsync(p => p.Id == personId);
         var leaveType = await _projectDbContext.LeaveTypes.FirstOrDefaultAsync(l => l.Id == leaveTypeId);
        /* var leaveTypeName = _projectDbContext.LeaveTypes.FirstOrDefault(l => l.Id == leaveTypeId); */
        if (person == null)
        {
            return BadRequest("Kullanıcı Bulunamadı veya kullanıcı seçmediyseniz lütfen kullanıcı seçiniz");
        }

        if (leaveType == null)
        {
            return BadRequest("İzin türü Seçin");
        }

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
                LeaveRequestId = request.Id,
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

    public async Task<IActionResult> Edit(int id)
    {
        var leaveRequest = await _projectDbContext.LeaveRequests.FindAsync(id);
        if (leaveRequest == null)
        {
            return NotFound();
        }

        ViewBag.Persons = await _projectDbContext.Persons.ToListAsync();
        ViewBag.LeaveTypes = await _projectDbContext.LeaveTypes.ToListAsync();
        return View(leaveRequest);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, int personId, int leaveTypeId, DateTime leaveTime, DateTime entryTime)
    {
        var leaveRequest = await _projectDbContext.LeaveRequests.FindAsync(id);
        if (leaveRequest == null)
        {
            return NotFound();
        }

        leaveRequest.PersonId = personId;
        leaveRequest.LeaveTypeId = leaveTypeId;
        leaveRequest.LeaveTime = leaveTime;
        leaveRequest.EntryTime = entryTime;
        leaveRequest.DurationDays = CalculateBusinessDays(leaveTime, entryTime);
        _projectDbContext.LeaveRequests.Update(leaveRequest);
        await _projectDbContext.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var leaveRequest = await _projectDbContext.LeaveRequests.FindAsync(id);
        if (leaveRequest == null)
        {
            return NotFound();
        }

        _projectDbContext.LeaveRequests.Remove(leaveRequest);
        await _projectDbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}