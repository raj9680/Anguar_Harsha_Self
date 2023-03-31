using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MvcTaskManager.Models;


namespace MvcTaskManager.Identity
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser, ApplicationRole, String>
    {
        public ApplicationDbContext(DbContextOptions options): base(options)
        { }

        
        public DbSet<ClientLocation> ClientLocations { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<IdentityRole> ApplicationRoles { get; set; } // ApplcationRole
        public DbSet<Country> Countries { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<TaskPriority> TaskPriorities { get; set; }
        public DbSet<TaskStatus> TaskStatuses { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<TaskStatusDetail> TaskStatusDetail { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // seeding data to ClientLocation Table
            builder.Entity<ClientLocation>().HasData(
                new ClientLocation() { ClientLocationID = 1, ClientLocationName = "Boston"},
                new ClientLocation() { ClientLocationID = 2, ClientLocationName = "New Delhi" },
                new ClientLocation() { ClientLocationID = 3, ClientLocationName = "New Jersy" },
                new ClientLocation() { ClientLocationID = 4, ClientLocationName = "New York" },
                new ClientLocation() { ClientLocationID = 5, ClientLocationName = "London" },
                new ClientLocation() { ClientLocationID = 6, ClientLocationName = "Tokyo" }
            );

            // seeding data to Projects Table
            builder.Entity<Project>().HasData(
                new Project() { ProjectID = 1, ProjectName = "Hospital Management System", DateOfStart = Convert.ToDateTime("2017-8-1"), Active = true, ClientLocationID = 2, Status = "In Force", TeamSize = 14},
                new Project() { ProjectID = 2, ProjectName = "Library Management System", DateOfStart = Convert.ToDateTime("2016-8-1"), Active = false, ClientLocationID = 1, Status = "In Force", TeamSize = 16 },
                new Project() { ProjectID = 3, ProjectName = "School Management System", DateOfStart = Convert.ToDateTime("2015-8-1"), Active = true, ClientLocationID = 3, Status = "In Force", TeamSize = 18 },
                new Project() { ProjectID = 4, ProjectName = "Bitcoin CRM", DateOfStart = Convert.ToDateTime("2014-8-1"), Active = true, ClientLocationID = 2, Status = "In Force", TeamSize = 20 }
            );
            // seeding data to Country Table
            builder.Entity<Country>().HasData(
                new Country() { CountryID=1, CountryName="India"},
                new Country() { CountryID=2, CountryName="Russia"},
                new Country() { CountryID=3, CountryName="Japan"},
                new Country() { CountryID=4, CountryName="Australia"}
            );

            //builder.Entity<IdentityUserRole<Guid>>().HasKey(p => new { p.UserId, p.RoleId });

            builder.Entity<TaskPriority>().HasData(
                new TaskPriority() { TaskPriorityID = 1, TaskPriorityName = "Urgent" },
                new TaskPriority() { TaskPriorityID = 2, TaskPriorityName = "Normal" },
                new TaskPriority() { TaskPriorityID = 3, TaskPriorityName = "Below Normal" },
                new TaskPriority() { TaskPriorityID = 4, TaskPriorityName = "Low" }
             );

            builder.Entity<TaskStatus>().HasData(
                new TaskStatus() { TaskStatusID = 1, TaskStatusName = "Holding" }, //Tasks that need to be documented still
                new TaskStatus() { TaskStatusID = 2, TaskStatusName = "Prioritized" }, //Tasks that are placed in priority order; so need to start ASAP
                new TaskStatus() { TaskStatusID = 3, TaskStatusName = "Started" }, //Tasks that are currently working
                new TaskStatus() { TaskStatusID = 4, TaskStatusName = "Finished" }, //Tasks that are finished workng
                new TaskStatus() { TaskStatusID = 5, TaskStatusName = "Reverted" } //Tasks that are reverted back, with comments or issues
             );


            var keysProperties = builder.Model.GetEntityTypes().Select(x => x.FindPrimaryKey()).SelectMany(x => x.Properties);
            foreach (var property in keysProperties)
            {
                property.ValueGenerated = ValueGenerated.OnAdd;
            }

        }
    }
}
