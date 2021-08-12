using MysMusic.Core.Repositories;
using System;
using System.Threading.Tasks;

namespace MysMusic.Core
{
    public interface IUnitOfWork :IDisposable
    {
        IArtistRepository Artists { get; }
        IMusicRepository Musics { get; }
        IUserRepository Users { get; }
        Task<int> CommitAsync();
    }
}
