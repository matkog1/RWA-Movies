using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repos
{
    public class ImageRepo
    {
        private readonly RwaMoviesContext? _context;

        public ImageRepo(RwaMoviesContext? context)
        {
            _context = context;
        }

        public IEnumerable<Image>? GetAll() => _context?.Images;

        public Image? GetById(int id) => GetAll()?.FirstOrDefault(x => x.Id == id);

        public Image? Add(Image image)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image), "Image is null");
            }
            else
            {
                _context.Images.Add(image);
                _context.SaveChanges();
                return image;
            }
        }

        public Image? Update(Image image)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image), "Image is null");
            }
            else
            {
                Image? foundImage = GetById(image.Id);
                _context.Images.Update(foundImage);
                _context.SaveChanges();

                var updatedImage = GetById(foundImage.Id);
                return updatedImage;
            }
        }

        public void Delete(int id)
        {
            Image? image = GetById(id);
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image), "Image not found");
            }
            else
            {
                _context.Images.Remove(image);
                _context.SaveChanges();
            }
        }
    }
}
