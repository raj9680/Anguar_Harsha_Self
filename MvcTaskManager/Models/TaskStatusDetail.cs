using MvcTaskManager.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Models
{
    public class TaskStatusDetail
    {
        public int TaskStatusDetailID { get; set; } // PK
        public int TaskID { get; set; } // FK TaskTable
        public int TaskStatusID { get; set; } // FK TaskStatus Table
        public string UserID { get; set; } // FK AspNetUser Table
        public string Description { get; set; }
        public DateTime StatusUpdationDateTime { get; set; }

        [NotMapped]
        public string StatusUpdationDateTimeString { get; set; }

        [ForeignKey("TaskStatusID")]
        public virtual TaskStatus TaskStatus { get; set; }

        [ForeignKey("UserID")]
        public virtual ApplicationUser User { get; set; }
    }
}
