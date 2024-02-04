using BLayer.Service;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebMVC.Controllers
{
    public class VideoController : Controller
    {
        // GET: VideoController
        private ServiceVideo _serviceVideo;
        private ServiceGenre _serviceGenre;

        public VideoController(ServiceVideo serviceVideo, ServiceGenre serviceGenre)
        {
            _serviceVideo = serviceVideo;
            _serviceGenre = serviceGenre;
        }

        public ActionResult Index()
        {
            return View(_serviceVideo.GetAll());
        }

        // GET: VideoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: VideoController/Create
        public ActionResult Create()
        {
            var dbGenres = _serviceGenre.GetAll();
            ViewBag.GenreId = new SelectList(dbGenres, "Id", "Name");
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
            var dbGenres = _serviceGenre.GetAll();
            ViewBag.GenreId = new SelectList(dbGenres, "Id", "Name");
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
            return View();
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
