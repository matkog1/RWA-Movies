using BLayer.Service;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

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
        public async Task<IEnumerable<Genre>> GetAllGenres()
        {
            var allGenres = await _service.GetAll();
            return allGenres;
        }

        // GET api/<GenreController>/5
        [HttpGet("{id}")]
        public async Task<IEnumerable<Genre>> Get(int id)
        {
            var genre = await _service.ge
        }

        // POST api/<GenreController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<GenreController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<GenreController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
