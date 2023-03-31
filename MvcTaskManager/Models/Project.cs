using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Models
{
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }

        //[DisplayFormat(DataFormatString = "d/M/yyyy")]
        public virtual DateTime DateOfStart { get; set; }
        public int? TeamSize { get; set; }
        public bool Active { get; set; }
        public string Status { get; set; }
        public int ClientLocationID { get; set; }

        // From the Project object in order to access the parent object i.e client location
        [ForeignKey("ClientLocationID")]
        public virtual ClientLocation ClientLocation { get; set; }
    }

    //public class TaskManagerDbContext: DbContext
    //{
    //    public DbSet<Project> Projects { get; set; }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    {
    //        base.OnConfiguring(optionsBuilder);
    //        optionsBuilder.UseSqlServer("data source=localhost; integrated security=yes; initial catalog=TaskManager");
    //    }
    //}


}
