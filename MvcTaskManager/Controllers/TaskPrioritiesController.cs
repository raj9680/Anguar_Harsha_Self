using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcTaskManager.Identity;
using MvcTaskManager.Models;
using System.Collections.Generic;
using System.Linq;

namespace MvcTaskManager.Controllers
{
    public class TaskPrioritiesController : Controller
    {
        private ApplicationDbContext _context;
        public TaskPrioritiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("api/taskpriorities")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public List<TaskPriority> Get()
        {
            List<TaskPriority> taskPriorities = _context.TaskPriorities.ToList();
            return taskPriorities;
        }

        [HttpGet]
        [Route("api/taskpriorities/searchbytaskpriorityid/{TaskPriorityID}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetByTaskPriorityID(int TaskPriorityID)
        {
            TaskPriority taskPriority = _context.TaskPriorities.Where(temp => temp.TaskPriorityID == TaskPriorityID).FirstOrDefault();
            if (taskPriority != null)
            {
                return Ok(taskPriority);
            }
            else
                return NoContent();
        }

        [HttpPost]
        [Route("api/taskpriorities")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public TaskPriority Post([FromBody] TaskPriority taskPriority)
        {
            _context.TaskPriorities.Add(taskPriority);
            _context.SaveChanges();

            TaskPriority existingTaskPriority = _context.TaskPriorities.Where(temp => temp.TaskPriorityID == taskPriority.TaskPriorityID).FirstOrDefault();
            return taskPriority;
        }

        [HttpPut]
        [Route("api/taskpriorities")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public TaskPriority Put([FromBody] TaskPriority project)
        {
            TaskPriority existingTaskPriority = _context.TaskPriorities.Where(temp => temp.TaskPriorityID == project.TaskPriorityID).FirstOrDefault();
            if (existingTaskPriority != null)
            {
                existingTaskPriority.TaskPriorityName = project.TaskPriorityName;
                _context.SaveChanges();
                return existingTaskPriority;
            }
            else
            {
                return null;
            }
        }

        [HttpDelete]
        [Route("api/taskpriorities")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public int Delete(int TaskPriorityID)
        {
            TaskPriority existingTaskPriority = _context.TaskPriorities.Where(temp => temp.TaskPriorityID == TaskPriorityID).FirstOrDefault();
            if (existingTaskPriority != null)
            {
                _context.TaskPriorities.Remove(existingTaskPriority);
                _context.SaveChanges();
                return TaskPriorityID;
            }
            else
            {
                return -1;
            }
        }
    }
}
