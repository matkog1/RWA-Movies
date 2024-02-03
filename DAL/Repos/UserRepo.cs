using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repos
{
    public class UserRepo
    {
        private readonly RwaMoviesContext? _context;

        public UserRepo(RwaMoviesContext? context)
        {
            _context = context;
        }

        public IEnumerable<User>? GetAll() => _context?.Users;

        public User? GetById(int id) => GetAll()?.FirstOrDefault(x => x.Id == id);

        public User? GetByName(string userName) => GetAll()?.FirstOrDefault(y => y.Username == userName);

        public User? Add(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User is null");
            }
            else
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return user;
            }
        }

        public User? Update(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User is null");
            }
            else
            {
                User? foundUser = GetById(user.Id);
                _context.Users.Update(foundUser);
                _context.SaveChanges();

                var updatedUser = GetById(foundUser.Id);
                return updatedUser;
            }
        }

        public void DeleteById(int id)
        {
            User? user = GetById(id);
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User not found");
            }
            else
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        public void DeleteByName(string identifier)
        {
            User? user = GetByName(identifier);
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User not found");
            }
            else
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }
    }
}
