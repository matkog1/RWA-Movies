using BLayer.Service;
using DAL.APIRequests;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebMVC.Controllers
{
    public class GenreController : Controller
    {
        private ServiceGenre _service;

        public GenreController(ServiceGenre service)
        {
            _service = service;
        }

        // GET: GenreController
        public async Task<ActionResult> Index()
        {
            return View(_service.GetAll());
        }

        // GET: GenreController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            return View();
        }

        // GET: GenreController/Create
        public async Task<ActionResult> Create()
        {
            return View();
        }

        // POST: GenreController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Genre genre)
        {
            try
            {
                _service.Add(genre);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: GenreController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            return View(_service.GetById(id));
        }

        // POST: GenreController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Genre Genre)
        {
            try
            {
                var foundGenre = _service.GetById(id);
                if (foundGenre != null)
                {
                    foundGenre.Name = Genre.Name;
                    foundGenre.Description = Genre.Description;
                    _service.Update(foundGenre);
                }
                return RedirectToAction(nameof(Index));

            }
            catch
            {
                return View();
            }
        }

        // GET: GenreController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            return View(_service.GetById(id));
        }

        // POST: GenreController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, Genre genre)
        {
            try
            {
                _service.DeleteById(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}