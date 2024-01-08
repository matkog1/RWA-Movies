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
    public class ServiceUser
    {
        private readonly UserRepo _repo;
        private RwaMoviesContext _moviesContext;

        public ServiceUser(RwaMoviesContext moviesContext)
        {
            _moviesContext = moviesContext ?? throw new ArgumentNullException(nameof(moviesContext), "MovieContext is null");
            _repo = new UserRepo(_moviesContext);
        }

        public IEnumerable<User>? GetAll() => _repo.GetAll();

        public User? GetById(int id) => _repo.GetById(id);

        public User? Add(User user)
        {
            if (user == null)
            {
                throw new InvalidOperationException("User is null");
            }
            _repo.Add(user);
            return user;
        }

        public User? Update(User user)
        {
            if (user == null)
            {
                throw new InvalidOperationException("User is null");
            }

            User? foundUser = GetById(user.Id);

            if (foundUser == null)
            {
                throw new InvalidOperationException("User not found");
            }

            _repo.Update(foundUser);
            var updatedUser = GetById(foundUser.Id);
            return updatedUser;
        }


        public void Delete(int id)
        {
            User? foundUser = _repo.GetById(id);
            if (foundUser == null)
            {
                throw new InvalidOperationException("User not found");
            }
            _repo.Delete(foundUser.Id);
        }
    }
}
