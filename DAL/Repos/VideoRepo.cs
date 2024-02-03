using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repos
{
    public class VideoRepo
    {
        private readonly RwaMoviesContext? _context;

        public VideoRepo(RwaMoviesContext? context)
        {
            _context = context;
        }

        public IEnumerable<Video>? GetAll() => _context?.Videos;

        public Video? GetById(int id) => GetAll()?.FirstOrDefault(x => x.Id == id);

        public Video? GetByName(string name) => GetAll()?.FirstOrDefault(y => y.Name == name);

        public Video? Add(Video video)
        {
            if (video == null)
            {
                throw new ArgumentNullException(nameof(video), "Video is null");
            }
            else
            {
                _context.Videos.Add(video);
                _context.SaveChanges();
                return video;
            }
        }

        public Video? Update(Video video)
        {
            if (video == null)
            {
                throw new ArgumentNullException(nameof(video), "Video is null");
            }
            else
            {
                Video? foundVideo = GetById(video.Id);
                _context.Videos.Update(foundVideo);
                _context.SaveChanges();

                var updatedVideo = GetById(foundVideo.Id);
                return updatedVideo;
            }
        }

        public void DeleteById(int id)
        {
            Video? genre = GetById(id);
            if (genre == null)
            {
                throw new ArgumentNullException(nameof(genre), "Video not found");
            }
            else
            {
                _context.Videos.Remove(genre);
                _context.SaveChanges();
            }
        }

        public void DeleteByName(string identifier)
        {
            Video? video = GetByName(identifier);
            if (video == null)
            {
                throw new ArgumentNullException(nameof(video), "Video not found");
            }
            else
            {
                _context.Videos.Remove(video);
                _context.SaveChanges();
            }
        }
    }
}
