using BLayer.Service;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WebMVC.Controllers
{
    public class CountryController : Controller
    {
        private ServiceCountry _serviceCountry;

        public CountryController(ServiceCountry serviceCountry)
        {
            _serviceCountry = serviceCountry;
        }


        // GET: CountryController
        public ActionResult Index(int page = 1, int pageSize = 5)
        {
            var allCountries = _serviceCountry.GetAll();


            var paginatedVideos = PaginatedList<Country>.Create(allCountries, page, pageSize);
            return View(paginatedVideos);
        }

        // GET: CountryController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CountryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CountryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Country country)
        {
            try
            {
                _serviceCountry.Add(country);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CountryController/Edit/5
        public ActionResult Edit(int id)
        {
            return View(_serviceCountry.GetById(id));
        }

        // POST: CountryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Country country)
        {
            try
            {
                var foundCountry = _serviceCountry.GetById(id);
                if (foundCountry != null)
                {
                    foundCountry.Code = country.Code;
                    foundCountry.Name = country.Name;
                    _serviceCountry.Update(foundCountry);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return NotFound();
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: CountryController/Delete/5
        public ActionResult Delete(int id)
        {
            return View(_serviceCountry.GetById(id));
        }

        // POST: CountryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Country country)
        {
            try
            {
                _serviceCountry.DeleteById(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
