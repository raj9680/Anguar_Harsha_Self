using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcTaskManager.Identity;
using MvcTaskManager.Models;
using MvcTaskManager.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace MvcTaskManager.Controllers
{

    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context = null;
        public ProjectsController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        [Route("api/projects")]
        public IActionResult Get()
        {
            System.Threading.Thread.Sleep(1000);
            List<Project> projects = _context.Projects.Include("ClientLocation").ToList();

            List<ProjectViewModel> projectsViewModel = new List<ProjectViewModel>();
            foreach (var project in projects)
            {
                projectsViewModel.Add(new ProjectViewModel() { ProjectID = project.ProjectID, ProjectName = project.ProjectName, TeamSize = project.TeamSize, DateOfStart = project.DateOfStart.ToString("dd/MM/yyyy"), Active = project.Active, ClientLocation = project.ClientLocation, Status = project.Status, ClientLocationID = project.ClientLocationID });
            }
            return Ok(projectsViewModel);
        }


        // Get project by project name
        [HttpGet]
        [Route("api/projects/searchbyprojectname/{ProjectName}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public bool GetProjectByProject(string ProjectName)
        {
            Project project = _context.Projects.Include("ClientLocation").Where(temp => temp.ProjectName == ProjectName).FirstOrDefault();

            if(project != null)
            {
                ProjectViewModel projectViewModel = new ProjectViewModel() { ProjectID = project.ProjectID, ProjectName = project.ProjectName, TeamSize = project.TeamSize, DateOfStart = project.DateOfStart.ToString("dd/MM/yyyy"), Active = project.Active, ClientLocation = project.ClientLocation, ClientLocationID = project.ClientLocationID, Status = project.Status };
                return true;
            }
            return false;
        }
        // End


        [HttpPost]
        [Route("api/projects")]
        //[ValidateAntiForgeryToken] // work only with cookie not with jwt
        public IActionResult Post([FromBody] Project project)
        {
            project.ClientLocation = null;
            _context.Projects.Add(project);
            _context.SaveChanges();

            Project existingProject = _context.Projects.Include("ClientLocation").Where(temp => temp.ProjectID == project.ProjectID).FirstOrDefault();
            ProjectViewModel projectViewModel = new ProjectViewModel() { ProjectID = existingProject.ProjectID, ProjectName = existingProject.ProjectName, TeamSize = existingProject.TeamSize, DateOfStart = existingProject.DateOfStart.ToString("dd/MM/yyyy"), Active = existingProject.Active, ClientLocation = existingProject.ClientLocation, Status = existingProject.Status };
            return Ok(projectViewModel);
        }

        
        [HttpPut]
        [Route("api/projects")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Put([FromBody] Project project)
        {
            Project existingProject = _context.Projects.Where(temp => temp.ProjectID == project.ProjectID).FirstOrDefault();
            if (existingProject != null)
            {
                existingProject.ProjectName = project.ProjectName;
                existingProject.DateOfStart = project.DateOfStart;
                existingProject.TeamSize = project.TeamSize;
                existingProject.ClientLocationID = project.ClientLocationID;
                existingProject.Active = project.Active;
                existingProject.Status = project.Status;
                _context.SaveChanges();

                Project existingProject2 = _context.Projects.Include("ClientLocation").Where(temp => project.ProjectID == project.ProjectID).FirstOrDefault();
                ProjectViewModel projectViewModel = new ProjectViewModel() { ProjectID = existingProject2.ProjectID, ProjectName = existingProject2.ProjectName, TeamSize = existingProject2.TeamSize, DateOfStart = existingProject2.DateOfStart.ToString("dd/MM/yyyy"), Active = existingProject2.Active, ClientLocation = existingProject2.ClientLocation, Status = existingProject2.Status };

                return Ok(existingProject2);
            }
            return null;
        }


        [HttpDelete]
        [Route("api/projects")]
        public int Delete(int ProjectID)
        {
            Project existingProject = _context.Projects.Where(temp => temp.ProjectID == ProjectID).FirstOrDefault();
            if (existingProject != null)
            {
                _context.Projects.Remove(existingProject);
                _context.SaveChanges();
                return existingProject.ProjectID;
            }
            return -1;
        }

        #region Search
        [HttpGet]
        [Route("api/projects/search/{searchby}/{searchtext}")]
        public IActionResult Search(string searchBy, string searchText)
        {
            List<Project> projects = null;
            if (searchBy=="ProjectID") 
            {
                projects = _context.Projects.Include("ClientLocation").Where(temp => temp.ProjectID.ToString().Contains(searchText)).ToList();
            }
            else if(searchBy=="ProjectName")
            {
                projects = _context.Projects.Include("ClientLocation").Where(temp => temp.ProjectName.Contains(searchText)).ToList();
            }
            else if(searchBy=="DateOfStart")
            {
                projects = _context.Projects.Include("ClientLocation").Where(temp => temp.DateOfStart.ToString().Contains(searchText)).ToList();
            }
            else if(searchBy=="TeamSize")
            {
                projects = _context.Projects.Include("ClientLocation").Where(temp => temp.TeamSize.ToString().Contains(searchText)).ToList();
            }
            else { }

            List<ProjectViewModel> projectViewModels = new List<ProjectViewModel>();
            foreach (var project in projects)
            {
                projectViewModels.Add(
                    new ProjectViewModel() { ProjectID = project.ProjectID, ProjectName = project.ProjectName, TeamSize = project.TeamSize, DateOfStart = project.DateOfStart.ToString("dd/MM/yyyy"), Active = project.Active, ClientLocation = project.ClientLocation, Status = project.Status }
                );
            }

            return Ok(projectViewModels);  
        }
        #endregion

        [HttpGet]
        [Route("api/projects/searchbyprojectid/{projectid}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetProjectByProject(int projectid)
        {
            Project project = _context.Projects.Include("ClientLocation").Where(temp => temp.ProjectID == projectid).FirstOrDefault();
            if (project != null)
            {
                ProjectViewModel projectViewModel = new ProjectViewModel() { ProjectID = project.ProjectID, ProjectName = project.ProjectName, TeamSize = project.TeamSize, DateOfStart = project.DateOfStart.ToString("dd/MM/yyyy"), Active = project.Active, ClientLocation = project.ClientLocation, ClientLocationID = project.ClientLocationID, Status = project.Status };
                return Ok(projectViewModel);
            }
            else
                return new EmptyResult();
        }
    }
}
