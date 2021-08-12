using MysMusic.Core;
using MysMusic.Core.Models;
using MysMusic.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.Services.Services
{
    public class UserService:IUserService
    {
        private readonly IUnitOfWork _unitOfwork;
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfwork = unitOfWork;
        }

        public async  Task<User> Authenticate(string username, string password)
        {
            return await _unitOfwork.Users.Authenticate(username, password);
        }

        public async Task<User> Create(User user, string password)
        {
            await _unitOfwork.Users.Create(user, password);
            await _unitOfwork.CommitAsync();
            return user;
        }

        public void Delete(int id)
        {
            _unitOfwork.Users.delete(id);
            _unitOfwork.CommitAsync();
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _unitOfwork.Users.GetAllUserAsync();
        }

        public async Task<User> GetById(int id)
        {
            return await _unitOfwork.Users.GetWithUsersByIdAsync(id);
        }

        public void Update(User user, string password = null)
        {
            _unitOfwork.Users.Update(user, password);
            _unitOfwork.CommitAsync();
        }
    }
}
