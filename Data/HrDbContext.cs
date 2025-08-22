using HrManagementSystem.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace HrManagementSystem.Data
{
    public class HrDbContext : IdentityDbContext<User>
    {
        public HrDbContext(DbContextOptions<HrDbContext> options) : base(options)
        {
            //ChangeTracker.LazyLoadingEnabled = true;
        }
        public DbSet<User> Employees { get; set; }
        //public DbSet<Client> Clients { get; set; }
        public DbSet<ActivityTasks> Activities { get; set; }

        public DbSet<Projects> Project { get; set; }
        public DbSet<Timesheet> Timesheets { get; set; }
        public DbSet<ClientLeavePolicy> ClientLeavePolicies { get; set; }

        //public DbSet<Approval> Approvals { get; set; }
        public DbSet<Role> Roles { get; set; }
        // public DbSet<TimesheetEmployee> TimesheetEmployee { get; set; }

        public DbSet<Department> Departments { get; set; }
        public DbSet<EmployeeClientAssignment> EmployeeClientAssignments { get; set; }
        //public DbSet<AppraisalTemplate> appraisalTemplates { get; set; }
        public DbSet<AppTemplateLatest> appraisalTemplatesLatest { get; set; }
        public DbSet<AppraisalResponse> appraisalResponses { get; set; }
        public DbSet<JobType> JobTypes { get; set; }
        public DbSet<AssetType> AssetTypes { get; set; }
        public DbSet<SalaryComponent> SalaryComponents { get; set; }


        public DbSet<EmployeeAsset> EmployeeAssets { get; set; }
        public DbSet<EmployeeSalary> EmployeeSalaries { get; set; }

        public DbSet<Designation> Designations { get; set; }
        public DbSet<EmployeeOffDetails> EmployeeOffDetails { get; set; }
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<EmployeeLeave> EmployeeLeaves { get; set; }
        public DbSet<LeaveApplication> LeaveApplications { get; set; }
        public DbSet<EmployeeLoan> LoanApplication { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Apply Identity configurations

            // Define primary keys for Identity entities
            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
            });

            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });
            });

            modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            });

//            modelBuilder.Entity<JobType>().HasData(
//    new JobType { JobTypeId = 1, Name = "Permanent" },
//    new JobType { JobTypeId = 2, Name = "Contract" },
//    new JobType { JobTypeId = 3, Name = "Freelancer" },
//    new JobType { JobTypeId = 4, Name = "Consultant" },
//    new JobType { JobTypeId = 5, Name = "Probation" }
//);

            modelBuilder.Entity<Timesheet>()
     .HasOne(t => t.Employee)
     .WithMany()
     .HasForeignKey(t => t.EmployeeId)
     .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Timesheet>()
                .HasOne(t => t.Manager)
                .WithMany()
                .HasForeignKey(t => t.ManagerID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Timesheet>()
                .HasOne(t => t.Client)
                .WithMany()
                .HasForeignKey(t => t.ClientId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<AppTemplateLatest>()
.Property(s => s.Amount)
 .HasConversion(
v => JsonConvert.SerializeObject(v),
v => JsonConvert.DeserializeObject<List<string>>(v));

            modelBuilder.Entity<AppTemplateLatest>()
.Property(s => s.Remarks)
.HasConversion(
v => JsonConvert.SerializeObject(v),
v => JsonConvert.DeserializeObject<List<string>>(v));



        }
    }
}
