using MvcTaskManager.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Models
{
    public class Task
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskID { get; set; } // Primary Key for Task table

        public string TaskName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }

        public int ProjectID { get; set; }  // FK refers to project table
        public string CreatedBy { get; set; } // FK refers to AspNetUsers table, created by
        public string AssignedTo { get; set; } // FK refers to AspNetUsers table, assigned to
        public int TaskPriorityID { get; set; } // FK refers to TaskPriorities table

        public DateTime LastUpdatedOn { get; set; }
        public string CurrentStatus { get; set; }
        public int CurrentTaskStatusID { get; set; }


        [NotMapped]
        public string CreatedOnString { get; set; }
        [NotMapped]
        public string LastUpdatedOnString { get; set; }

        [ForeignKey("ProjectID")]
        public virtual Project Project { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual ApplicationUser CreatedByUser { get; set; }

        [ForeignKey("AssignedTo")]
        public virtual ApplicationUser AssignedToUser { get; set; }

        [ForeignKey("TaskPriorityID")]
        public virtual TaskPriority TaskPriority { get; set; }

        public virtual ICollection<TaskStatusDetail> TaskStatusDetails { get; set; }
    }

    public class GroupedTask
    {
        public string TaskStatusName { get; set; }
        public List<Task> Tasks { get; set; }
    }
}
