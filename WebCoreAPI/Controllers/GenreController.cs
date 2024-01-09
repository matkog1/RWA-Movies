using Azure.Core;
using BLayer.Service;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebCoreAPI.APIRequests;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebCoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private RwaMoviesContext _context;
        private ServiceGenre _service;

        public GenreController(ServiceGenre service)
        {
            _service = service;
        }

        // GET: api/<GenreController>
        [HttpGet]
        public ActionResult<IEnumerable<RequestGenre>> GetAllGenres()
        {
            IList<RequestGenre> request  = new List<RequestGenre>();
            request?.ToList().ForEach(genre =>request.Add(new RequestGenre
                   {
                       Name = genre.Name,
                       Description = genre.Description
                   }
                   ));

            return Ok(request);
        }

        // GET api/<GenreController>/5
        [HttpGet("{id}")]
        public ActionResult<RequestGenre> GetGenre(int id)
        {
            var genreById = _service.GetById(id);
            if (genreById == null)
            {
                return BadRequest($"Genre not found");
            }
            else
            {
                RequestGenre genre = new RequestGenre();
                genre.Name = genreById.Name;
                genre.Description = genre.Description;
                return Ok(genre);
            }
        }

        // POST api/<GenreController>
        [HttpPost]
        public ActionResult<RequestGenre> Post(RequestGenre request)
        {
            var genreExists = _service?.GetAll()?.FirstOrDefault(g => g.Name.ToLower() == request.Name.ToLower());
            
            if (genreExists == null)
            {
                var genre = new Genre();
                genre.Name = request.Name;
                genre.Description = request.Description;
                _service.Add(genre);
                return Ok(request);
            }
            else
            {
                throw new Exception("Genre already exits");
            }
        }
       
        // PUT api/<GenreController>/5
        [HttpPut("{id}")]
        public ActionResult<RequestGenre> Put(int id, RequestGenre request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var foundGenre = _service.GetById(id);
            if (foundGenre == null)
            {
                return NotFound($"Genre with id {id} not found in database");
            }
            else
            {
                foundGenre.Name = request.Name;
                foundGenre.Description = request.Description;
                return Ok(request);
            }
        }

        // DELETE api/<GenreController>/5
        [HttpDelete("{id}")]
        public ActionResult<RequestGenre> Delete(int id)
        {
            var foundGenre = _service.GetById(id);
            if (foundGenre == null)
            {
                return NotFound($"Genre not found");
            }
            else
            {
                _service.Delete(id);
                return Ok(new RequestGenre { Name = foundGenre.Name, Description = foundGenre.Description });
            }
        }
    }
}
