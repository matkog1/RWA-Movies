using Azure.Core;
using BLayer.Service;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using DAL.APIRequests;
using DAL.APIResponse;
using Azure;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebCoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private ServiceTag _service;

        public TagController(ServiceTag service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        // GET: api/<GenreController>
        [HttpGet]
        public ActionResult<IEnumerable<RequestTag>> GetAllTags()
        {
            try
            {
                var allTags = _service.GetAll();

                IList<RequestTag> listOfTags = allTags.Select(tag => new RequestTag
                {
                    Name = tag.Name,

                }).ToList();

                if (!listOfTags.Any())
                {
                    return NotFound($"Currently there is no Tags in database!");
                }

                return Ok(listOfTags);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        // GET api/<GenreController>/5
        [HttpGet("{id}")]
        public ActionResult<RequestTag> GetTag(int id)
        {
            try
            {
                var tagById = _service.GetById(id);
                if (tagById == null)
                {
                    return NotFound($"Tag with ID number: {id} not found.");
                }
                else
                {
                    var tag = new RequestTag
                    {
                        Name = tagById.Name,
                    };
                    return Ok(tag);
                }
            }
            catch (Exception)
            {

                return StatusCode(500, "Internal Server Error");
            }
        }

        // POST api/<GenreController>
        [HttpPost]
        public ActionResult<RequestTag> Post(RequestTag request)
        {
            try
            {
                if (request == null)
                {
                    BadRequest($"Request object is null.");
                }

                var tagExists = _service?.GetAll()?.FirstOrDefault(g => g.Name.ToLower() == request.Name.ToLower());

                if (tagExists != null)
                {
                    return Conflict($"Tag with the name '{request.Name}' already exists");
                }

                var tag = new Tag
                {
                    Name = request.Name
                };
               
                _service.Add(tag);
                
                var response = new ResponseTag
                {
                    Id = tag.Id,
                    Name = tag.Name,
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
        public ActionResult<RequestTag> Put(int id, RequestTag request)
        {
            try
            {
                if (request == null)
                {
                    BadRequest($"Request object is null.");
                }

                var foundTag = _service.GetById(id);

                if (foundTag == null)
                {
                    return Conflict($"Tag with ID number {id} not found!");
                }
                else
                {
                    foundTag.Name = request.Name;
                    _service.Update(foundTag);
                    return Ok(foundTag);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        // DELETE api/<GenreController>/5
        [HttpDelete("identifier")]
        public ActionResult<RequestTag> Delete(string identifier)
        {
            try
            {
                int id;
                bool isNumeric = int.TryParse(identifier, out id);

                Tag foundTag = new Tag();

                if (isNumeric)
                {
                    foundTag = _service.GetById(id);
                    if (foundTag == null)
                    {
                        return NotFound($"Tag: {foundTag.Name} with ID  '{identifier}' not found!");
                    }
                    else
                    {
                        _service.DeleteById(foundTag.Id);
                        return Ok($"Tag {foundTag.Name} with ID {foundTag.Id} has been deleted!");
                    }
                }
                else
                {
                    foundTag = _service.GetByName(identifier);
                    if (foundTag == null)
                    {
                        return NotFound($"Tag with ID or name '{identifier}' not found!");
                    }
                    else
                    {
                        _service.DeleteByName(foundTag.Name);
                        return Ok($"Tag {foundTag.Name} has been deleted!");
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
