using BLayer.Service;
using Utils;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PublicMVC.Models;
using Utils;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PublicMVC.Controllers
{
    public class UserController : Controller
    {
        // GET: UserController
        private ServiceUser _serviceUser;
        private ServiceCountry _serviceCountry;

        public UserController(ServiceUser serviceuser, ServiceCountry serviceCountry)
        {
            _serviceUser = serviceuser;
            _serviceCountry = serviceCountry;
        }

        public ActionResult Index()
        {
            return View("Login");
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        public ActionResult Login(UserLogin userLogin)
        {
            try
            {
                var dbUser = _serviceUser.GetByName(userLogin.Username);
                if (dbUser != null)
                {
                    string hashedPassword = SecurityUtils.HashPassword(userLogin.Password, Convert.FromBase64String(dbUser.PwdSalt));
                    if (hashedPassword == dbUser.PwdHash)
                    {
                        ViewBag.UserLogged = userLogin.Username;
                        return RedirectToAction("Index", "Video", new { username = userLogin.Username });
                    }
                }
                ViewBag.LoginMessage = "Login unsuccessful. Please try again.";
                return View(userLogin);
            }
            catch (Exception)
            {
                return View();
            }
        }


        // GET: UserController/Create
        public ActionResult Create()
        {
            var dbCountries = _serviceCountry.GetAll();
            ViewBag.CountryOfResidenceId = new SelectList(dbCountries, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            try
            {
                (byte[] salt, string saltString) = SecurityUtils.GenerateSalt();
                string hashedPassword = SecurityUtils.HashPassword(user.PwdHash, salt);

                user.PwdHash = hashedPassword;
                user.PwdSalt = saltString;
                int selectedCountryId = user.CountryOfResidenceId;
                var userExists = _serviceUser?.GetAll()?.FirstOrDefault(g => g.Username.ToLower() == user.Username.ToLower());

                if (userExists != null)
                {
                    return View(user);
                }
                else
                {
                    _serviceUser.Add(user);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception)
            {
                return View();
            }
        }

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            var dbCountryId = _serviceCountry.GetAll();
            ViewBag.CountryOfResidenceId = new SelectList(dbCountryId, "Id", "Name");
            return View(_serviceUser.GetById(id));
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, User user)
        {
            try
            {
                var foundUser = _serviceUser.GetById(id);
                if (foundUser != null)
                {
                    foundUser.Username = user.Username;
                    foundUser.FirstName = user.FirstName;
                    foundUser.LastName = user.LastName;
                    foundUser.PwdHash = user.PwdHash;
                    foundUser.PwdSalt = user.PwdSalt;
                    foundUser.Email = user.Email;
                    foundUser.Phone = user.Phone;
                    foundUser.IsConfirmed = user.IsConfirmed;
                    foundUser.CountryOfResidenceId = user.CountryOfResidenceId;
                    //ne prikazuje password ako vec postoji, rijesiti bug
                    if (!string.IsNullOrEmpty(user.PwdHash))
                    {
                        (byte[] salt, string saltString) = SecurityUtils.GenerateSalt();
                        string hashedPassword = SecurityUtils.HashPassword(user.PwdHash, salt);
                        foundUser.PwdHash = hashedPassword;
                        foundUser.PwdSalt = saltString;
                    }
                    _serviceUser.Update(foundUser);
                }
                return RedirectToAction(nameof(Index));

            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View(_serviceUser.GetById(id));
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, User user)
        {
            try
            {
                _serviceUser.DeleteById(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
