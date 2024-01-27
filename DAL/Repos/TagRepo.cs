using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repos
{
    public class TagRepo
    {
        private readonly RwaMoviesContext? _context;

        public TagRepo(RwaMoviesContext? context)
        {
            _context = context;
        }

        public IEnumerable<Tag>? GetAll() => _context?.Tags;

        public Tag? GetById(int id) => GetAll()?.FirstOrDefault(x => x.Id == id);

        public Tag? GetByName(string name) => GetAll()?.FirstOrDefault(y => y.Name == name);

        public Tag? Add(Tag tag)
        {
            if (tag == null)
            {
                throw new ArgumentNullException(nameof(tag), "Tag is null");
            }
            else
            {
                _context.Tags.Add(tag);
                _context.SaveChanges();
                return tag;
            }
        }

        public Tag? Update(Tag tag)
        {
            if (tag == null)
            {
                throw new ArgumentNullException(nameof(tag), "Tag is null");
            }
            else
            {
                Tag? foundTag = GetById(tag.Id);
                _context.Tags.Update(foundTag);
                _context.SaveChanges();

                var updatedTag = GetById(foundTag.Id);
                return updatedTag;
            }
        }

        public void DeleteById(int id)
        {
            Tag? tag = GetById(id);
            if (tag == null)
            {
                throw new ArgumentNullException(nameof(tag), "Genre not found");
            }
            else
            {
                _context.Tags.Remove(tag);
                _context.SaveChanges();
            }
        }

        public void DeleteByName(string identifier)
        {
            Tag? tag = GetByName(identifier);
            if (tag == null)
            {
                throw new ArgumentNullException(nameof(tag), "Genre not found");
            }
            else
            {
                _context.Tags.Remove(tag);
                _context.SaveChanges();
            }
        }
    }
}
