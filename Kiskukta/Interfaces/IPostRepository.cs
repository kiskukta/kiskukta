using Kiskukta.Models;

namespace Kiskukta.Interfaces
{
    public interface IPostRepository
    {
        void Add(Post post);
        Post GetById(int id);
        void Update(Post post);
    }
}