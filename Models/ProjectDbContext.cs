using Microsoft.EntityFrameworkCore;

namespace CimtasHrPanel.Models
{
    public class ProjectDbContext : DbContext
    {
        public ProjectDbContext(DbContextOptions<ProjectDbContext>options) : base(options)
        {
            
        }
        
    }
}