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
    public class ServiceVideo
    {
        private readonly VideoRepo _repo;
        private RwaMoviesContext _moviesContext;

        public ServiceVideo(RwaMoviesContext moviesContext)
        {
            _moviesContext = moviesContext ?? throw new ArgumentNullException(nameof(moviesContext), "MovieContext is null");
            _repo = new VideoRepo(_moviesContext);
        }

        public IEnumerable<Video>? GetAll() => _repo.GetAll();

        public Video? GetById(int id) => _repo.GetById(id);
        public Video? GetByName(string name) => _repo.GetByName(name);

        public Video? Add(Video video)
        {
            if (video == null)
            {
                throw new InvalidOperationException("Video is null");
            }
            _repo.Add(video);
            return video;
        }

        public Video? Update(Video video)
        {
            if (video == null)
            {
                throw new InvalidOperationException("Video is null");
            }

            Video? foundVideo = GetById(video.Id);

            if (foundVideo == null)
            {
                throw new InvalidOperationException("Video not found");
            }

            _repo.Update(foundVideo);
            var updatedVideo = GetById(foundVideo.Id);
            return updatedVideo;
        }


        public void DeleteById(int id)
        {
            Video? foundVideo = _repo.GetById(id);
            if (foundVideo == null)
            {
                throw new InvalidOperationException("Video not found");
            }
            _repo.DeleteById(foundVideo.Id);
        }
        public void DeleteByName(string name)
        {
            Video? foundVideo = _repo.GetByName(name);
            if (foundVideo == null)
            {
                throw new InvalidOperationException("Video not found");
            }
            _repo.DeleteByName(foundVideo.Name);
        }
    }
}
