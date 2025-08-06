using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using CimtasHrPanel.Models;

public class ProjectDbContextFactory : IDesignTimeDbContextFactory<ProjectDbContext>
{
    public ProjectDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ProjectDbContext>();
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=sql_server;User Id=sa;Password=3280Strongp;Encrypt=False;TrustServerCertificate=True;");

        return new ProjectDbContext(optionsBuilder.Options);
    }
}