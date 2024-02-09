using Azure.Core;
using BLayer.Service;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using DAL.APIRequests;
using DAL.APIResponse;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Azure;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebCoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {

        private readonly ServiceVideo _videoService;
        private readonly ServiceTag _tagService;
        private readonly ServiceGenre _genreService;
        private readonly ServiceVideoTag _videoTagService;
        private readonly ServiceImage _imageService;

        public VideoController(ServiceVideo videoService, ServiceTag tagService, ServiceGenre genreService, ServiceVideoTag videoTagService, ServiceImage imageService)
        {
            _videoService = videoService;
            _tagService = tagService;
            _genreService = genreService;
            _videoTagService = videoTagService;
            _imageService = imageService;
        }

        [HttpGet("[action]")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<Video>> Search(string searchName)
        {

            try
            {
                var allVideos = _videoService.GetAll();

                if (!string.IsNullOrEmpty(searchName))
                {
                    allVideos = allVideos.Where(v => v.Name.ToLower() == searchName.ToLower() || v.Name.ToLower().Contains(searchName.ToLower()));
                }

                return Ok(allVideos);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }



        [HttpGet("[action]")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<Video>> SortBy(string sortBy, string orderingDirection)
        {
            var allVideos = _videoService.GetAll();

         
            // Apply sorting if sortBy and orderingDirection are provided
            if (!string.IsNullOrEmpty(sortBy) && !string.IsNullOrEmpty(orderingDirection))
            {
                switch (sortBy.ToLower())
                {
                    case "name":
                        allVideos = orderingDirection.ToLower() == "asc" ? allVideos.OrderBy(v => v.Name) : allVideos.OrderByDescending(v => v.Name);
                        break;
                    case "id":
                        allVideos = orderingDirection.ToLower() == "asc" ? allVideos.OrderBy(v => v.Id) : allVideos.OrderByDescending(v => v.Id);
                        break;
                    case "totaltime":
                        allVideos = orderingDirection.ToLower() == "asc" ? allVideos.OrderBy(v => v.TotalSeconds) : allVideos.OrderByDescending(v => v.TotalSeconds);
                        break;
                    default:
                        // Handle invalid sortBy parameter
                        return BadRequest($"Invalid sortBy parameter. Must be 'name', 'id', or 'totaltime'.");
                }
            }

            // Return the sorted and filtered list of videos
            return Ok(allVideos);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Video>> GetAllVideos(int pageNumber = 1, int pageSize = 5)
        {
            try
            {
                var allVideos = _videoService.GetAll();
                int totalVideos = allVideos.Count();
                int totalPages = (int)Math.Ceiling((double)totalVideos / pageSize);
                if (pageNumber < 1 || pageNumber > totalPages)
                {
                    return BadRequest("Invalid page number.");
                }

                int skipCount = (pageNumber - 1) * pageSize;

                var paginatedVideos = allVideos.Skip(skipCount).Take(pageSize).ToList();
                return Ok(new { Videos = paginatedVideos, TotalPages = totalPages, CurrentPage = pageNumber });
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }


        // GET api/<GenreController>/5
        [HttpGet("{id}")]
        public ActionResult<RequestVideo> GetVideo(int id)
        {
            try
            {
                var videoById = _videoService.GetById(id);
                if (videoById == null)
                {
                    return NotFound($"Video with ID number: {id} not found.");
                }
                else
                {
                    var video = new Video
                    {
                        Id = videoById.Id,
                        Name = videoById.Name,
                        Description = videoById.Description,
                        ImageId = videoById.ImageId,
                        GenreId = videoById.GenreId,
                        TotalSeconds = videoById.TotalSeconds,
                        StreamingUrl = videoById.StreamingUrl,
                        VideoTags = videoById.VideoTags
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
        public ActionResult Post(RequestVideo video)
        {
            string[] tags = video.NewTags.Split(',');
            var allTags = _tagService.GetAll();
            var existingTags = allTags.Where(t => tags.Contains(t.Name));


            if (video == null)
            {
                BadRequest($"Request object is null.");
            }

            var videoExists = _videoService?.GetAll()?.FirstOrDefault(g => g.Name.ToLower() == video.Name.ToLower());

            if (videoExists != null)
            {
                return Conflict($"Video with the name '{video.Name}' already exists");
            }
            
            var newVideo = new Video
            {
                Name = video.Name,
                Description = video.Description,
                GenreId = video.GenreId,
                TotalSeconds = video.TotalSeconds,
                StreamingUrl = video.StreamingUrl,
                ImageId = video.ImageId,
                VideoTags = existingTags.Select(t => new VideoTag { TagId = t.Id }).ToList()
            };

            foreach (var tag in tags)
            {
                if (!allTags.Any(t => t.Name == tag))
                {
                    var newTag = new Tag { Name = tag };
                    newVideo.VideoTags.Add(new VideoTag { Tag = newTag });
                }
            }

            _videoService.Add(newVideo);
            return Ok(newVideo);
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

                var foundVideo = _videoService.GetById(id);

                if (foundVideo == null)
                {
                    return Conflict($"Video with ID number {id} not found!");
                }
                else
                {
                    foundVideo.Name = request.Name;
                    foundVideo.Description = request.Description;
                    foundVideo.GenreId = request.GenreId;
                    foundVideo.TotalSeconds = request.TotalSeconds;
                    foundVideo.StreamingUrl = request.StreamingUrl;
                    foundVideo.ImageId = request.ImageId;
                    _videoService.Update(foundVideo);
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
                    foundVideo = _videoService.GetById(id);
                    if (foundVideo == null)
                    {
                        return NotFound($"Video: {foundVideo.Name} with ID  '{identifier}' not found!");
                    }
                    else
                    {
                        _videoService.DeleteById(foundVideo.Id);
                        return Ok($"Video {foundVideo.Name} with ID {foundVideo.Id} has been deleted!");
                    }
                }
                else
                {
                    foundVideo = _videoService.GetByName(identifier);
                    if (foundVideo == null)
                    {
                        return NotFound($"Video with ID or name '{identifier}' not found!");
                    }
                    else
                    {
                        _videoService.DeleteByName(foundVideo.Name);
                        return Ok($"Video {foundVideo.Name} has been deleted!");
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
