﻿using Azure.Core;
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
    public class VideoController : ControllerBase
    {
        private ServiceVideo _service;

        public VideoController(ServiceVideo service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        // GET: api/<GenreController>
        [HttpGet]
        public ActionResult<IEnumerable<RequestVideo>> GetAllGenres()
        {
            try
            {
                var allVideos = _service.GetAll();

                IList<RequestVideo> listOfVideos = allVideos.Select(video => new RequestVideo
                {
                    Name = video.Name,
                    Description = video.Description,
                    GenreId = video.GenreId,
                    TotalSeconds = video.TotalSeconds,
                    StreamingUrl = video.StreamingUrl,
                    Genre = video.Genre,
                    Image = video.Image,

                }).ToList();

                if (!listOfVideos.Any())
                {
                    return NotFound($"Currently there is no Users in database!");
                }

                return Ok(listOfVideos);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        // GET api/<GenreController>/5
        [HttpGet("{id}")]
        public ActionResult<RequestVideo> GetGenre(int id)
        {
            try
            {
                var videoById = _service.GetById(id);
                if (videoById == null)
                {
                    return NotFound($"Video with ID number: {id} not found.");
                }
                else
                {
                    var video = new RequestVideo
                    {
                        Name = videoById.Name,
                        Description = videoById.Description,
                        GenreId = videoById.GenreId,
                        TotalSeconds = videoById.TotalSeconds,
                        StreamingUrl = videoById.StreamingUrl,
                        Genre = videoById.Genre,
                        Image = videoById.Image,
                    };
                    return Ok(video);
                }
            }
            catch (Exception)
            {

                return StatusCode(500, "Internal Server Error");
            }
        }

        // POST api/<GenreController>
        [HttpPost]
        public ActionResult<RequestVideo> Post(RequestVideo request)
        {
            try
            {
                if (request == null)
                {
                    BadRequest($"Request object is null.");
                }

                var videoExists = _service?.GetAll()?.FirstOrDefault(g => g.Name.ToLower() == request.Name.ToLower());

                if (videoExists != null)
                {
                    return Conflict($"User with the name '{request.Name}' already exists");
                }
                else
                {
                    var video = new Video
                    {
                        Name = request.Name,
                        Description = request.Description,
                        GenreId = request.GenreId,
                        TotalSeconds = request.TotalSeconds,
                        StreamingUrl = request.StreamingUrl,
                        Genre = request.Genre,
                        Image = request.Image,
                    };
                    _service.Add(video);
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
        public ActionResult<RequestVideo> Put(int id, RequestVideo request)
        {
            try
            {
                if (request == null)
                {
                    BadRequest($"Request object is null.");
                }

                var foundVideo = _service.GetById(id);

                if (foundVideo == null)
                {
                    return Conflict($"User with ID number {id} not found!");
                }
                else
                {
                    foundVideo.Name = request.Name;
                    foundVideo.Description = request.Description;
                    foundVideo.GenreId = request.GenreId;
                    foundVideo.TotalSeconds = request.TotalSeconds;
                    foundVideo.StreamingUrl = request.StreamingUrl;
                    foundVideo.Genre = request.Genre;
                    _service.Update(foundVideo);
                    return Ok(foundVideo);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        // DELETE api/<GenreController>/5
        [HttpDelete("identifier")]
        public ActionResult<RequestVideo> Delete(string identifier)
        {
            try
            {
                int id;
                bool isNumeric = int.TryParse(identifier, out id);

                Video foundVideo = new Video();

                if (isNumeric)
                {
                    foundVideo = _service.GetById(id);
                    if (foundVideo == null)
                    {
                        return NotFound($"User: {foundVideo.Name} with ID  '{identifier}' not found!");
                    }
                    else
                    {
                        _service.DeleteById(foundVideo.Id);
                        return Ok($"Genre {foundVideo.Name} with ID {foundVideo.Id} has been deleted!");
                    }
                }
                else
                {
                    foundVideo = _service.GetByName(identifier);
                    if (foundVideo == null)
                    {
                        return NotFound($"Genre with ID or name '{identifier}' not found!");
                    }
                    else
                    {
                        _service.DeleteByName(foundVideo.Name);
                        return Ok($"Genre {foundVideo.Name} has been deleted!");
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