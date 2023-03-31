using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcTaskManager.Identity;
using MvcTaskManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Controllers
{
    public class ClientLocationsController : Controller
    {
        private ApplicationDbContext _context;
        public ClientLocationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("api/clientlocations")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Get()
        {
            List<ClientLocation> clientLocations = _context.ClientLocations.ToList();
            return Ok(clientLocations);
        }


        [HttpGet]
        [Route("api/clientlocations/searchbyclientlocationid/{ClientLocationID}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetByClientLocationID(int ClientLocationID)
        {
            ClientLocation clientLocation = _context.ClientLocations.Where(temp => temp.ClientLocationID == ClientLocationID).FirstOrDefault();
            if (clientLocation != null)
            {
                return Ok(clientLocation);
            }
            else
                return NoContent();
        }


        [HttpPost]
        [Route("api/clientlocations")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ClientLocation Post([FromBody] ClientLocation clientLocation)
        {
            _context.ClientLocations.Add(clientLocation);
            _context.SaveChanges();

            ClientLocation existingClientLocation = _context.ClientLocations.Where(temp => temp.ClientLocationID == clientLocation.ClientLocationID).FirstOrDefault();
            return clientLocation;
        }


        [HttpPut]
        [Route("api/clientlocations")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ClientLocation Put([FromBody] ClientLocation project)
        {
            ClientLocation existingClientLocation = _context.ClientLocations.Where(temp => temp.ClientLocationID == project.ClientLocationID).FirstOrDefault();
            if (existingClientLocation != null)
            {
                existingClientLocation.ClientLocationName = project.ClientLocationName;
                _context.SaveChanges();
                return existingClientLocation;
            }
            else
            {
                return null;
            }
        }

        [HttpDelete]
        [Route("api/clientlocations")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public int Delete(int ClientLocationID)
        {
            ClientLocation existingClientLocation = _context.ClientLocations.Where(temp => temp.ClientLocationID == ClientLocationID).FirstOrDefault();
            if (existingClientLocation != null)
            {
                _context.ClientLocations.Remove(existingClientLocation);
                _context.SaveChanges();
                return ClientLocationID;
            }
            else
            {
                return -1;
            }
        }
    }
}
