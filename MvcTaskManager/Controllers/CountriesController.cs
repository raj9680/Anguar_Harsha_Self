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
    public class CountriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CountriesController(ApplicationDbContext context)
        {
            _context = context;
        }


        [Route("api/countries")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetCountries()
        {
            List<Country> countries = _context.Countries.OrderBy(t => t.CountryName).ToList();
            return Ok(countries);
        }


        [HttpGet]
        [Route("api/countries/searchbycountryid/{CountryID}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetCountryID(int CountryID)
        {
            Country country = _context.Countries.Where(temp => temp.CountryID == CountryID).FirstOrDefault();
            if(country != null)
            {
                return Ok(country);
            }
            else
            {
                return NoContent();
            }
        }


        [HttpPost]
        [Route("api/countries")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public Country Post([FromBody] Country country)
        {
            _context.Countries.Add(country);
            _context.SaveChanges();

            Country existingCountry = _context.Countries.Where(temp => temp.CountryID == country.CountryID).FirstOrDefault();
            return existingCountry;
        }


        [HttpPut]
        [Route("api/countries")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public Country Put([FromBody] Country country)
        {
            Country existingCountry = _context.Countries.Where(temp => temp.CountryID == country.CountryID).FirstOrDefault();
            if(existingCountry != null)
            {
                existingCountry.CountryName = country.CountryName;
                _context.SaveChanges();
                return existingCountry;
            }
            else
            {
                return null;
            }
        }


        [HttpDelete]
        [Route("api/countries")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public int Delete(int CountryID)
        {
            Country existingCountry = _context.Countries.Where(temp => temp.CountryID == CountryID).FirstOrDefault();
            if (existingCountry != null)
            {
                _context.Countries.Remove(existingCountry);
                _context.SaveChanges();
                return CountryID;
            }
            else
            {
                return -1;
            }
        }
    }
}
