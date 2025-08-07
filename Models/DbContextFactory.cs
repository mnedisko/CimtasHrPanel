using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using CimtasHrPanel.Models;

public class ProjectDbContextFactory : IDesignTimeDbContextFactory<ProjectDbContext>
{
    public ProjectDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ProjectDbContext>();
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=IzinTakipPaneliDb;Trusted_Connection=True;");

        return new ProjectDbContext(optionsBuilder.Options);
    }
}