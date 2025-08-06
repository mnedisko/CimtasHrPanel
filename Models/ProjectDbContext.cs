using Microsoft.EntityFrameworkCore;

namespace CimtasHrPanel.Models
{
    public class ProjectDbContext : DbContext
    {
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options)
        {

        }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<LeaveSettings> LeaveSettings { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Department>().HasData(
                new Department { Id = 1, DepartmentName = "Pazarlama" },
                new Department { Id = 2, DepartmentName = "İnsan kaynakları" },
                new Department { Id = 3, DepartmentName = "Üretim" }


            );
            modelBuilder.Entity<Person>().HasData(
                new Person { Id = 1, PersonName = "Ahmet", PersonLastName = "Yılmaz", DepartmentId = 1 },
                new Person { Id = 2, PersonName = "Büşra", PersonLastName = "Soy", DepartmentId = 1 },
                new Person { Id = 3, PersonName = "Kadir", PersonLastName = "Ay", DepartmentId = 2 },
                new Person { Id = 4, PersonName = "Mehmet", PersonLastName = "Yılmaz", DepartmentId = 3 }

            );

            modelBuilder.Entity<LeaveType>().HasData(
                new LeaveType { Id = 1, LeaveTypeName = "Yıllık İzin", IsIncreaseAnnualValue = true },
                new LeaveType { Id = 2, LeaveTypeName = "Hastalık İzni", IsIncreaseAnnualValue = false },
                new LeaveType { Id = 3, LeaveTypeName = "Ücretsiz İzin", IsIncreaseAnnualValue = false }
            );
            modelBuilder.Entity<LeaveRequest>().HasData(
                new LeaveRequest { Id = 1, PersonId = 1, LeaveTypeId = 1, LeaveTime = new DateTime(2025, 7, 1), EntryTime = new DateTime(2025, 7, 7), DurationDays = 7 },
                new LeaveRequest { Id = 2, PersonId = 2, LeaveTypeId = 1, LeaveTime = new DateTime(2025, 7, 1), EntryTime = new DateTime(2025, 7, 22), DurationDays = 22 }
            );
            modelBuilder.Entity<LeaveSettings>().HasData(
                new LeaveSettings {Id=1, MaxAnnualLeaveLimit = 20 }
            );
        }
        


        
        
    }
}