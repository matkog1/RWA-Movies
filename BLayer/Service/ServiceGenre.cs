﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Repos;

namespace BLayer.Service
{
    public class ServiceGenre
    {
        private readonly GenreRepo _repo;
        private RwaMoviesContext _moviesContext;

        public ServiceGenre(RwaMoviesContext moviesContext)
        {
            _moviesContext = moviesContext ?? throw new ArgumentNullException(nameof(moviesContext), "MovieContext is null");
            _repo = new GenreRepo(_moviesContext);
        }

        public async Task<IEnumerable<Genre>> GetAll() => _repo.GetAll();

        public async Task<IEnumerable<Genre> GetById(int id) => _repo.GetById(id);

        public Genre? Add(Genre genre)
        {
           if(genre == null)
           {
                throw new InvalidOperationException("Genre is null");
           }
           _repo.Add(genre);
           return genre;
        }

        public Genre? Update(Genre genre)
        {
            if (genre == null)
            {
                throw new InvalidOperationException("Genre is null");
            }

            Genre? foundGenre = GetById(genre.Id);
            
            if (foundGenre == null)
            {
                throw new InvalidOperationException("Genre not found");
            }

            _repo.Update(foundGenre);
            var updatedGenre = GetById(foundGenre.Id);
            return updatedGenre;
        }


        public void Delete (int id)
        {
            Genre? foundGenre = _repo.GetById(id);
            if (foundGenre == null)
            {
                throw new InvalidOperationException("Genre not found");
            }
            _repo.Delete(foundGenre.Id);
        }
    }
}