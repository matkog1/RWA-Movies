using BLayer.Service;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Drawing.Printing;

namespace WebMVC.Controllers
{
    public class VideoController : Controller
    {
        // GET: VideoController
        private ServiceVideo _serviceVideo;
        private ServiceGenre _serviceGenre;
        private ServiceImage _serviceImage;

        public VideoController(ServiceVideo serviceVideo, ServiceGenre serviceGenre, ServiceImage serviceImage)
        {
            _serviceVideo = serviceVideo;
            _serviceGenre = serviceGenre;
            _serviceImage = serviceImage;
        }

        public ActionResult Index(string searchString, int genreId, int page = 1, int pageSize = 5)
        {
            // Get all videos
            var allVideos = _serviceVideo.GetAll();

            // Filter videos by search string
            if (!string.IsNullOrEmpty(searchString))
            {
                allVideos = allVideos.Where(video => video.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase));
            }

            // Filter videos by genre
            if (genreId != 0)
            {
                allVideos = allVideos.Where(video => video.GenreId == genreId);
            }

            // Populate genre details for each video
            var videosWithDetails = allVideos.Select(video =>
            {
                var genre = _serviceGenre.GetById(video.GenreId);
                video.Genre = genre;
                return video;
            });

            // Create paginated list
            var paginatedVideos = PaginatedList<Video>.Create(videosWithDetails, page, pageSize);

            // Retrieve all genres
            var allGenres = _serviceGenre.GetAll();

            // Pass paginated list and genres to the view
            ViewBag.Genres = allGenres;
            return View(paginatedVideos);
        }

        // GET: VideoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: VideoController/Create
        public ActionResult Create()
        {
            var dbImages = _serviceImage.GetAll();
            var dbGenres = _serviceGenre.GetAll();
            ViewBag.GenreId = new SelectList(dbGenres, "Id", "Name");
            ViewBag.ImageId = new SelectList(dbImages, "Id", "Content");
            return View();
        }

        // POST: VideoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Video video)
        {
            try
            {
                int selectedGenreId = video.GenreId;
                _serviceVideo.Add(video);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: VideoController/Edit/5
        public ActionResult Edit(int id)
        {
            var dbImages = _serviceImage.GetAll();
            var dbGenres = _serviceGenre.GetAll();
            ViewBag.GenreId = new SelectList(dbGenres, "Id", "Name");
            ViewBag.ImageId = new SelectList(dbImages , "Id", "Content");
            return View(_serviceVideo.GetById(id));
        }

        // POST: VideoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Video video)
        {
            try
            {
                var foundVideo = _serviceVideo.GetById(id);
                if (foundVideo != null)
                {
                    foundVideo.Id = video.Id;
                    foundVideo.Name = video.Name;
                    foundVideo.Description = video.Description;
                    foundVideo.GenreId = video.GenreId;
                    foundVideo.Genre = video.Genre;
                    foundVideo.TotalSeconds = video.TotalSeconds;
                    foundVideo.StreamingUrl = video.StreamingUrl;
                    foundVideo.Image = video.Image;
                    foundVideo.ImageId = video.ImageId;
                    foundVideo.CreatedAt = video.CreatedAt;
                    _serviceVideo.Update(foundVideo);
                }
                return RedirectToAction(nameof(Index));

            }
            catch
            {
                return View();
            }
        }

        // GET: VideoController/Delete/5
        public ActionResult Delete(int id)
        {
            var video = _serviceVideo.GetById(id);
            var genreName = _serviceGenre.GetById(video.GenreId).Name;
            ViewBag.Genre = genreName;
            return View(video);
        }

        // POST: VideoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Video video)
        {
            try
            {
                _serviceVideo.DeleteById(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
