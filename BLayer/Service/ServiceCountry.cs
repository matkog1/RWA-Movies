using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Repos;

namespace BLayer.Service
{
    public class ServiceCountry
    {
        private readonly CountryRepo _repo;
        private RwaMoviesContext _moviesContext;

        public ServiceCountry(RwaMoviesContext moviesContext)
        {
            _moviesContext = moviesContext ?? throw new ArgumentNullException(nameof(moviesContext), "MovieContext is null");
            _repo = new CountryRepo(_moviesContext);
        }

        public IEnumerable<Country>? GetAll() => _repo.GetAll();

        public Country? GetById(int id) => _repo.GetById(id);

        public Country? GetByName(string name) => _repo.GetByName(name);

        public Country? Add(Country country)
        {
            _repo.Add(country);
            return country;
        }

        public Country? Update(Country country)
        {
            if (country == null)
            {
                throw new InvalidOperationException("Country is null");
            }

            Country? foundCountry  = GetById(country.Id);

            if (foundCountry  == null)
            {
                throw new InvalidOperationException("Country not found");
            }

            _repo.Update(foundCountry);
            var updatedCountry = GetById(foundCountry.Id);
            return updatedCountry;
        }


        public void DeleteById(int id)
        {
            Country? foundCountry = _repo.GetById(id);
            if (foundCountry == null)
            {
                throw new InvalidOperationException("Country not found");
            }
            _repo.DeleteById(foundCountry.Id);
        }
        public void DeleteByName(string name)
        {
            Country? foundCountry = _repo.GetByName(name);
            if (foundCountry == null)
            {
                throw new InvalidOperationException("Country not found");
            }
            _repo.DeleteByName(foundCountry.Name);
        }
    }
}
