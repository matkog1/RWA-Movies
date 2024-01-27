using Azure.Core;
using BLayer.Service;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using WebCoreAPI.APIRequests;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebCoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private ServiceUser _service;

        public UserController(ServiceUser service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        // GET: api/<GenreController>
        [HttpGet]
        public ActionResult<IEnumerable<RequestUser>> GetAllGenres()
        {
            try
            {
                var allUsers = _service.GetAll();

                IList<RequestUser> listOfUsers = allUsers.Select(user => new RequestUser
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Username = user.Username,
                    Email = user.Email,
                    Phone = user.Phone,
                    CountryId = user.CountryOfResidenceId,

                }).ToList();

                if (!listOfUsers.Any())
                {
                    return NotFound($"Currently there is no Users in database!");
                }

                return Ok(listOfUsers);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        // GET api/<GenreController>/5
        [HttpGet("{id}")]
        public ActionResult<RequestUser> GetGenre(int id)
        {
            try
            {
                var userById = _service.GetById(id);
                if (userById == null)
                {
                    return NotFound($"User with ID number: {id} not found.");
                }
                else
                {
                    var user = new RequestUser
                    {
                        FirstName = userById.FirstName,
                        LastName = userById.LastName,
                        Username = userById.Username,
                        Email = userById.Email,
                        Phone = userById.Phone,
                        CountryId = userById.CountryOfResidenceId,
                    };
                    return Ok(user);
                }
            }
            catch (Exception)
            {

                return StatusCode(500, "Internal Server Error");
            }
        }

        // POST api/<GenreController>
        [HttpPost]
        public ActionResult<RequestUser> Post(RequestUser request)
        {
            try
            {
                if (request == null)
                {
                    BadRequest($"Request object is null.");
                }

                var userExists = _service?.GetAll()?.FirstOrDefault(g => g.FirstName.ToLower() == request.FirstName.ToLower());

                if (userExists != null)
                {
                    return Conflict($"User with the name '{request.FirstName}' already exists");
                }
                else
                {
                    var user = new User
                    {
                        FirstName = userExists.FirstName,
                        LastName = userExists.LastName,
                        Username = userExists.Username,
                        Email = userExists.Email,
                        Phone = userExists.Phone,
                        CountryOfResidenceId = userExists.CountryOfResidenceId,
                    };
                    _service.Add(user);
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
        public ActionResult<RequestUser> Put(int id, RequestUser request)
        {
            try
            {
                if (request == null)
                {
                    BadRequest($"Request object is null.");
                }

                var foundUser = _service.GetById(id);

                if (foundUser == null)
                {
                    return Conflict($"User with ID number {id} not found!");
                }
                else
                {
                    foundUser.FirstName = request.FirstName;
                    foundUser.LastName = request.LastName;
                    foundUser.Email = request.Email;
                    foundUser.Phone = request.Phone;
                    foundUser.CountryOfResidenceId = request.CountryId;
                    _service.Update(foundUser);
                    return Ok(foundUser);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        // DELETE api/<GenreController>/5
        [HttpDelete("identifier")]
        public ActionResult<RequestUser> Delete(string identifier)
        {
            try
            {
                int id;
                bool isNumeric = int.TryParse(identifier, out id);

                User founderUser = new User();

                if (isNumeric)
                {
                    founderUser = _service.GetById(id);
                    if (founderUser == null)
                    {
                        return NotFound($"User: {founderUser.FirstName} with ID  '{identifier}' not found!");
                    }
                    else
                    {
                        _service.DeleteById(founderUser.Id);
                        return Ok($"Genre {founderUser.FirstName} with ID {founderUser.Id} has been deleted!");
                    }
                }
                else
                {
                    founderUser = _service.GetByName(identifier);
                    if (founderUser == null)
                    {
                        return NotFound($"Genre with ID or name '{identifier}' not found!");
                    }
                    else
                    {
                        _service.DeleteByName(founderUser.FirstName);
                        return Ok($"Genre {founderUser.FirstName} has been deleted!");
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
