using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repos
{
    public class VideoTagRepo
    {
        private readonly RwaMoviesContext? _context;

        public VideoTagRepo(RwaMoviesContext? context)
        {
            _context = context;
        }

        public IEnumerable<VideoTag>? GetAll() => _context?.VideoTags;

        public VideoTag? GetById(int id) => GetAll()?.FirstOrDefault(x => x.Id == id);


        public VideoTag? Add(VideoTag videoTag)
        {
            if (videoTag == null)
            {
                throw new ArgumentNullException(nameof(videoTag), "VideoTag is null");
            }
            else
            {
                _context.VideoTags.Add(videoTag);
                _context.SaveChanges();
                return videoTag;
            }
        }

        public VideoTag? Update(VideoTag videoTag)
        {
            if (videoTag == null)
            {
                throw new ArgumentNullException(nameof(videoTag), "VideoTag is null");
            }
            else
            {
                VideoTag? foundVideoTag = GetById(videoTag.Id);
                _context.VideoTags.Update(foundVideoTag);
                _context.SaveChanges();

                var updatedVideo = GetById(foundVideoTag.Id);
                return updatedVideo;
            }
        }

        public void DeleteById(int id)
        {
            VideoTag? tag = GetById(id);
            if (tag == null)
            {
                throw new ArgumentNullException(nameof(tag), "VideoTag not found");
            }
            else
            {
                _context.VideoTags.Remove(tag);
                _context.SaveChanges();
            }
        }
    }
}
