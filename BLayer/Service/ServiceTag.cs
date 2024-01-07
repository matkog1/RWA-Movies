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
    public class ServiceTag
    {
        private readonly TagRepo _repo;
        private RwaMoviesContext _moviesContext;

        public ServiceTag(RwaMoviesContext moviesContext)
        {
            _moviesContext = moviesContext ?? throw new ArgumentNullException(nameof(moviesContext), "MovieContext is null");
            _repo = new TagRepo(_moviesContext);
        }

        public IEnumerable<Tag>? GetAll() => _repo.GetAll();

        public Tag? GetById(int id) => _repo.GetById(id);

        public Tag? Add(Tag tag)
        {
            if (tag == null)
            {
                throw new InvalidOperationException("Tag is null");
            }
            _repo.Add(tag);
            return tag;
        }

        public Tag? Update(Tag tag)
        {
            if (tag == null)
            {
                throw new InvalidOperationException("Tag is null");
            }

            Tag? foundTag = GetById(tag.Id);

            if (foundTag == null)
            {
                throw new InvalidOperationException("Tag not found");
            }

            _repo.Update(foundTag);
            var updatedTag = GetById(foundTag.Id);
            return updatedTag;
        }


        public void Delete(int id)
        {
            Tag? foundTag = _repo.GetById(id);
            if (foundTag == null)
            {
                throw new InvalidOperationException("Tag not found");
            }
            _repo.Delete(foundTag.Id);
        }
    }
}
