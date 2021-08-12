using MysMusic.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MysMusic.Core.Repositories
{
    public interface IUserRepository
    {
        Task<User> Authenticate(string username, string password);
        Task<User> Create(User user, string password);
        void Update(User user, string password = null);
        void delete(int id);
        Task<IEnumerable<User>> GetAllUserAsync();
        Task<User> GetWithUsersByIdAsync(int id);
    }
}
