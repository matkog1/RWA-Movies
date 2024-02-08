using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repos
{
    public class CountryRepo
    {
        private readonly RwaMoviesContext? _context;

        public CountryRepo(RwaMoviesContext? context)
        {
            _context = context;
        }

        public IEnumerable<Country>? GetAll() => _context?.Countries;

        public Country? GetById(int id) => GetAll()?.FirstOrDefault(x => x.Id == id);

        public Country? GetByName(string name) => GetAll()?.FirstOrDefault(y => y.Name == name);

        public Country? Add(Country country)
        {
            if (country == null)
            {
                throw new ArgumentNullException(nameof(country), "Country is null");
            }
            else
            {
                _context.Countries.Add(country);
                _context.SaveChanges();
                return country;
            }
        }

        public Country? Update(Country country)
        {
            if (country == null)
            {
                throw new ArgumentNullException(nameof(country), "Country is null");
            }
            else
            {
                Country? foundCountry = GetById(country.Id);
                _context.Countries.Update(foundCountry);
                _context.SaveChanges();

                var updatedCountry = GetById(foundCountry.Id);
                return updatedCountry;
            }
        }

        public void DeleteById(int id)
        {
            Country? country = GetById(id);
            if (country == null)
            {
                throw new ArgumentNullException(nameof(country), "Country not found");
            }
            else
            {
                _context.Countries.Remove(country);
                _context.SaveChanges();
            }
        }

        public void DeleteByName(string identifier)
        {
            Country? country = GetByName(identifier);
            if (country == null)
            {
                throw new ArgumentNullException(nameof(country), "Country not found");
            }
            else
            {
                _context.Countries.Remove(country);
                _context.SaveChanges();
            }
        }
    }
}
