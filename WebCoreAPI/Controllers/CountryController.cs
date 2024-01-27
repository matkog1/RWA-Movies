using Azure.Core;
using BLayer.Service;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using DAL.APIRequests;
using DAL.APIResponse;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebCoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private ServiceCountry _service;

        public CountryController(ServiceCountry service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        // GET: api/<GenreController>
        [HttpGet]
        public ActionResult<IEnumerable<RequestCountry>> GetAllCountries()
        {
            try
            {
                var allCountries = _service.GetAll();

                IList<RequestCountry> listOfCountries = allCountries.Select(country => new RequestCountry
                {
                    Code = country.Code,
                    Name = country.Name,
                }).ToList();

                if (!allCountries.Any())
                {
                    return NotFound($"Currently there is no Country in database!");
                }

                return Ok(allCountries);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        // GET api/<GenreController>/5
        [HttpGet("{id}")]
        public ActionResult<RequestCountry> GetCountry(int id)
        {
            try
            {
                var countryById = _service.GetById(id);
                if (countryById == null)
                {
                    return NotFound($"Country with ID number {id} not found.");
                }
                else
                {
                    var country = new RequestCountry
                    {
                        Code = countryById.Code,
                        Name = countryById.Name,
                    };
                    return Ok(country);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        // POST api/<GenreController>
        [HttpPost]
        public ActionResult<RequestCountry> Post(RequestCountry request)
        {
            try
            {
                if (request == null)
                {
                    BadRequest($"Request object is null.");
                }

                var countryExists = _service?.GetAll()?.FirstOrDefault(country => country.Name.ToLower() == request.Name.ToLower());

                if (countryExists != null)
                {
                    return Conflict($"Genre with the name '{request.Name}' already exists");
                }
                else
                {
                    var country = new Country
                    {
                        Code = request.Code,
                        Name = request.Name,
                    };
                    _service.Add(country);
                    return Ok(request);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        // PUT api/<GenreController>/5
        [HttpPut("{id}")]
        public ActionResult<RequestCountry> Put(int id, RequestCountry request)
        {
            try
            {
                if (request == null)
                {
                    BadRequest($"Request object is null.");
                }

                var foundCountry = _service.GetById(id);

                if (foundCountry == null)
                {
                    return Conflict($"Country with ID number {id} not found!");
                }
                else
                {
                    foundCountry.Code = request.Code;
                    foundCountry.Name = request.Name;
                    _service.Update(foundCountry);
                    return Ok(foundCountry);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        // DELETE api/<GenreController>/5
        [HttpDelete("identifier")]
        public ActionResult<RequestCountry> Delete(string identifier)
        {
            try
            {
                int id;
                bool isNumeric = int.TryParse(identifier, out id);

                Country foundCountry = new Country();

                if (isNumeric)
                {
                    foundCountry = _service.GetById(id);
                    if (foundCountry == null)
                    {
                        return NotFound($"Genre {foundCountry.Name} with ID  '{identifier}' not found!");
                    }
                    else
                    {
                        _service.DeleteById(foundCountry.Id);
                        return Ok($"Country {foundCountry.Name} with ID {foundCountry.Id} has been deleted!");
                    }
                }
                else
                {
                    foundCountry = _service.GetByName(identifier);
                    if (foundCountry == null)
                    {
                        return NotFound($"Found with ID or name '{identifier}' not found!");
                    }
                    else
                    {
                        _service.DeleteByName(foundCountry.Name);
                        return Ok($"Genre {foundCountry.Name} has been deleted!");
                    }
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
