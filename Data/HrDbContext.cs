﻿using HrManagementSystem.Models;

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
