using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcTaskManager.Identity;
using MvcTaskManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MvcTaskManager.Controllers
{
    public class TaskStatusesController : Controller
    {
        private ApplicationDbContext _context;

        public TaskStatusesController(ApplicationDbContext db)
        {
            _context = db;
        }

        [HttpGet]
        [Route("api/taskstatuses")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public List<TaskStatus> Get()
        {
            List<TaskStatus> taskStatuses = _context.TaskStatuses.ToList();
            return taskStatuses;
        }

        [HttpGet]
        [Route("api/taskstatuses/searchbytaskstatusid/{TaskStatusID}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetByTaskStatusID(int TaskStatusID)
        {
            TaskStatus taskStatus = _context.TaskStatuses.Where(temp => temp.TaskStatusID == TaskStatusID).FirstOrDefault();
            if (taskStatus != null)
            {
                return Ok(taskStatus);
            }
            else
                return NoContent();
        }

        [HttpPost]
        [Route("api/taskstatuses")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public TaskStatus Post([FromBody] TaskStatus taskStatus)
        {
            _context.TaskStatuses.Add(taskStatus);
            _context.SaveChanges();

            TaskStatus existingTaskStatus = _context.TaskStatuses.Where(temp => temp.TaskStatusID == taskStatus.TaskStatusID).FirstOrDefault();
            return taskStatus;
        }

        [HttpPut]
        [Route("api/taskstatuses")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public TaskStatus Put([FromBody] TaskStatus project)
        {
            TaskStatus existingTaskStatus = _context.TaskStatuses.Where(temp => temp.TaskStatusID == project.TaskStatusID).FirstOrDefault();
            if (existingTaskStatus != null)
            {
                existingTaskStatus.TaskStatusName = project.TaskStatusName;
                _context.SaveChanges();
                return existingTaskStatus;
            }
            else
            {
                return null;
            }
        }

        [HttpDelete]
        [Route("api/taskstatuses")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public int Delete(int TaskStatusID)
        {
            TaskStatus existingTaskStatus = _context.TaskStatuses.Where(temp => temp.TaskStatusID == TaskStatusID).FirstOrDefault();
            if (existingTaskStatus != null)
            {
                _context.TaskStatuses.Remove(existingTaskStatus);
                _context.SaveChanges();
                return TaskStatusID;
            }
            else
            {
                return -1;
            }
        }
    }
}
