using BLayer.Service;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebMVC.Controllers
{
    public class VideoController : Controller
    {
        // GET: VideoController
        private ServiceVideo _service;

        public VideoController(ServiceVideo service)
        {
            _service = service;
        }

        public ActionResult Index()
        {
            return View(_service.GetAll());
        }

        // GET: VideoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: VideoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VideoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Video video)
        {
            try
            {
                _service.Add(video);
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
            return View(_service.GetById(id));
        }

        // POST: VideoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Video video)
        {
            try
            {
                var dbVideo = _service.GetById(id);
                
                dbVideo.Id = video.Id;
                dbVideo.Name = video.Name;
                dbVideo.Description = video.Description;
                dbVideo.GenreId = video.GenreId;    
                dbVideo.Genre = video.Genre;
                dbVideo.TotalSeconds = video.TotalSeconds;
                dbVideo.StreamingUrl = video.StreamingUrl;
                dbVideo.Image = video.Image;
                dbVideo.ImageId = video.ImageId;
                dbVideo.CreatedAt = video.CreatedAt;

                _service.Update(dbVideo);
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
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
