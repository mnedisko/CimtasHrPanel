using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CimtasHrPanel.Models;

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
    public IActionResult PersonalData(int personId)
    {
        var person = _projectDbContext.Persons.FirstOrDefault(p => p.Id == personId);
        if (person==null)
        {
            return Ok("Kullanıcı bulunamadı");
        }
        var personmw = new PersonModelView
        {
            Name = person.PersonName,
            
        };
        
        return View();
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
