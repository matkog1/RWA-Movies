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
    public class GenreController : ControllerBase
    {
        private ServiceGenre _service;

        public GenreController(ServiceGenre service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        // GET: api/<GenreController>
        [HttpGet]
        public ActionResult<IEnumerable<ResponseGenre>> GetAllGenres()
        {
            try
            {
                var allGenres = _service.GetAll();

                IList<ResponseGenre> listOfGenres = allGenres.Select(genre => new ResponseGenre
                {
                    Id = genre.Id,
                    Name = genre.Name,
                    Description = genre.Description
                }).ToList();

                if (!listOfGenres.Any())
                {
                    return NotFound($"Currently there is no Genre in database!");
                }

                return Ok(listOfGenres);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        // GET api/<GenreController>/5
        [HttpGet("{id}")]
        public ActionResult<ResponseGenre> GetGenre(int id)
        {
            try
            {
                var genreById = _service.GetById(id);
                if (genreById == null)
                {
                    return NotFound($"Genre with ID number {id} not found.");
                }
                else
                {
                    var genre = new ResponseGenre
                    {
                        Id= genreById.Id,
                        Name = genreById.Name,
                        Description = genreById.Description
                    };
                    return Ok(genre);
                }
            }
            catch (Exception)
            {

                return StatusCode(500, "Internal Server Error");
            }
        }

        // POST api/<GenreController>
        [HttpPost]
        public ActionResult<RequestGenre> Post(RequestGenre request)
        {
            try
            {
                if (request == null)
                {
                    BadRequest($"Request object is null.");
                }

                var genreExists = _service?.GetAll()?.FirstOrDefault(g => g.Name.ToLower() == request.Name.ToLower());

                if (genreExists != null)
                {
                    return Conflict($"Genre with the name '{request.Name}' already exists");
                }
                
                var genre = new Genre
                {
                    Name = request.Name,
                    Description = request.Description
                };
                
                _service.Add(genre);

                var response = new ResponseGenre
                {
                    Id = genre.Id,
                    Name = genre.Name,
                    Description = genre.Description
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
        public ActionResult<RequestGenre> Put(int id, RequestGenre request)
        {
            try
            {
                if (request == null)
                {
                    BadRequest($"Request object is null.");
                }

                var foundGenre = _service.GetById(id);

                if (foundGenre == null)
                {
                    return Conflict($"Genre with ID number {id} not found!");
                }
                else
                {
                    foundGenre.Name = request.Name;
                    foundGenre.Description = request.Description;    
                    _service.Update(foundGenre);

                    var response = new ResponseGenre
                    {
                        Id = foundGenre.Id,
                        Name = foundGenre.Name,
                        Description = foundGenre.Description
                    };
                    
                    return Ok(response);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        // DELETE api/<GenreController>/5
        [HttpDelete("identifier")]
        public ActionResult<ResponseGenre> Delete(string identifier)
        {
            try
            {
                int id;
                bool isNumeric = int.TryParse(identifier, out id);

                Genre foundGenre = new Genre();

                if (isNumeric)
                {
                    foundGenre = _service.GetById(id);
                    if (foundGenre == null)
                    {
                        return NotFound($"Genre {foundGenre.Name} with ID  '{identifier}' not found!");
                    }
                    else
                    {
                        _service.DeleteById(foundGenre.Id);
                        return Ok($"Genre {foundGenre.Name} with ID {foundGenre.Id} has been deleted!");
                    }
                }
                else
                {
                    foundGenre = _service.GetByName(identifier);
                    if (foundGenre == null)
                    {
                        return NotFound($"Genre with ID or name '{identifier}' not found!");
                    }
                    else
                    {
                        _service.DeleteByName(foundGenre.Name);
                        return Ok($"Genre {foundGenre.Name} has been deleted!");
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
