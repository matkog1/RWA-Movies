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
    public class ServiceVideoTag
    {
        private readonly VideoTagRepo _repo;
        private RwaMoviesContext _moviesContext;

        public ServiceVideoTag(RwaMoviesContext moviesContext)
        {
            _moviesContext = moviesContext ?? throw new ArgumentNullException(nameof(moviesContext), "MovieContext is null");
            _repo = new VideoTagRepo(_moviesContext);
        }

        public IEnumerable<VideoTag>? GetAll() => _repo.GetAll();

        public VideoTag? GetById(int id) => _repo.GetById(id);

        public VideoTag? Add(VideoTag videoTag)
        {
            if (videoTag == null)
            {
                throw new InvalidOperationException("Video is null");
            }
            _repo.Add(videoTag);
            return videoTag;
        }

        public VideoTag? Update(VideoTag videoTag)
        {
            if (videoTag == null)
            {
                throw new InvalidOperationException("Video is null");
            }

            VideoTag? foundVideoTag = GetById(videoTag.Id);

            if (foundVideoTag == null)
            {
                throw new InvalidOperationException("Video not found");
            }

            _repo.Update(foundVideoTag);
            var updatedVideoTag = GetById(foundVideoTag.Id);
            return updatedVideoTag;
        }


        public void DeleteById(int id)
        {
            VideoTag? foundVideoTag = _repo.GetById(id);
            if (foundVideoTag == null)
            {
                throw new InvalidOperationException("Video not found");
            }
            _repo.DeleteById(foundVideoTag.Id);
        }

    }
}
