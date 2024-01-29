using GestionEmployee.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionEmployee.Infrastructure.DataBase
{
    public class GestionEmployeeContext : DbContext
    {
        public GestionEmployeeContext()
        {
        }

        public GestionEmployeeContext(DbContextOptions<GestionEmployeeContext> options)
      : base(options)
        {
        }
        public DbSet<Attendance> Attendances { get; set; } = null!;

        public DbSet<Department> Departments { get; set; } = null!;

        public DbSet<Employee> Employees { get; set; } = null!;

        public virtual DbSet<EmployeeDepartment> EmployeesDepartments { get; set; }

        public DbSet<LeaveRequest> LeaveRequests { get; set; }

        public virtual DbSet<LeaveRequestStatus> LeaveRequestStatuses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source = data.db");

    }
}
