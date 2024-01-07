using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repos
{
    public class GenreRepo
    {
        private readonly RwaMoviesContext? _context;

        public GenreRepo(RwaMoviesContext? context)
        {
            _context = context;
        }

        public IEnumerable<Genre>? GetAll() => _context?.Genres;

        public Genre? GetById(int id) => GetAll()?.FirstOrDefault(x => x.Id == id);

        public Genre? Add(Genre genre)
        {
            if (genre == null)
            {
                throw new ArgumentNullException(nameof(genre),"Genre is null");
            }
            else
            {
                _context.Genres.Add(genre);
                _context.SaveChanges();
                return genre;
            }
        }

        public Genre? Update(Genre genre)
        {
            if (genre == null)
            {
                throw new ArgumentNullException(nameof(genre), "Genre is null");
            }
            else
            {
                Genre? foundGenre = GetById(genre.Id);
                _context.Genres.Update(foundGenre);
                _context.SaveChanges();

                var updatedGenre = GetById(foundGenre.Id);
                return updatedGenre;
            }
        }

        public void Delete(int id)
        {
            Genre? genre = GetById(id);
            if (genre == null)
            {
                throw new ArgumentNullException(nameof(genre), "Genre not found");
            }
            else
            {
                _context.Genres.Remove(genre);
                _context.SaveChanges();
            }
        }
    }
}
