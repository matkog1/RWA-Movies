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
    public class ServiceImage
    {
        private readonly ImageRepo _repo;
        private RwaMoviesContext _moviesContext;

        public ServiceImage(RwaMoviesContext moviesContext)
        {
            _moviesContext = moviesContext ?? throw new ArgumentNullException(nameof(moviesContext), "MovieContext is null");
            _repo = new ImageRepo(_moviesContext);
        }

        public IEnumerable<Image>? GetAll() => _repo.GetAll();

        public Image? GetById(int id) => _repo.GetById(id);

        public Image? Add(Image image)
        {
            if (image == null)
            {
                throw new InvalidOperationException("Image is null");
            }
            _repo.Add(image);
            return image;
        }

        public Image? Update(Image image)
        {
            if (image == null)
            {
                throw new InvalidOperationException("Image is null");
            }

            Image? foundImage = GetById(image.Id);

            if (foundImage == null)
            {
                throw new InvalidOperationException("Imagge not found");
            }

            _repo.Update(foundImage);
            var updatedImage = GetById(foundImage.Id);
            return updatedImage;
        }


        public void Delete(int id)
        {
            Image? foundImage = _repo.GetById(id);
            if (foundImage == null)
            {
                throw new InvalidOperationException("Image not found");
            }
            _repo.Delete(foundImage.Id);
        }
    }
}
