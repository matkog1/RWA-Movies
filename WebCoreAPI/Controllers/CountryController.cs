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
        public ActionResult<IEnumerable<ResponseCountry>> GetAllCountries()
        {
            try
            {
                var allCountries = _service.GetAll();

                IList<ResponseCountry> listOfCountries = allCountries.Select(country => new ResponseCountry
                {
                    Id = country.Id,
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
        public ActionResult<ResponseCountry> GetCountry(int id)
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
                    var country = new ResponseCountry
                    {
                        Id = countryById.Id,
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
        public ActionResult<ResponseCountry> Post(RequestCountry request)
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
                    return Conflict($"Country with the name '{request.Name}' already exists");
                }
                
                var country = new Country
                {
                    Code = request.Code,
                    Name = request.Name,
                };
                
                _service.Add(country);

                var response = new ResponseCountry
                {
                    Id = country.Id,
                    Code = country.Code,
                    Name = country.Name,
                };

                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        // PUT api/<GenreController>/5
        [HttpPut("{id}")]
        public ActionResult<ResponseCountry> Put(int id, RequestCountry request)
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
                        return NotFound($"Country {foundCountry.Name} with ID  '{identifier}' not found!");
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
                        return NotFound($"Country with ID or name '{identifier}' not found!");
                    }
                    else
                    {
                        _service.DeleteByName(foundCountry.Name);
                        return Ok($"Country {foundCountry.Name} has been deleted!");
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
