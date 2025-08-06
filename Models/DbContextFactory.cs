using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using CimtasHrPanel.Models;

public class ProjectDbContextFactory : IDesignTimeDbContextFactory<ProjectDbContext>
{
    public ProjectDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ProjectDbContext>();
        
        // Buraya PostgreSQL bağlantı dizenizi ekleyin.
        // Bu, `appsettings.json` dosyasındaki ile aynı olmalıdır.
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=izin_takip_db;Username=postgres;Password=mysecretpassword");

        return new ProjectDbContext(optionsBuilder.Options);
    }
}