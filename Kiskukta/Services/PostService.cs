using Kiskukta.Services;
using Kiskukta.Interfaces;
using Kiskukta.Models;

namespace Kiskukta.Services
{
    public class PostService
    {
        private readonly IPostRepository _repo;
        private readonly IUserContext _user;

        public PostService(IPostRepository repo, IUserContext user)
        {
            _repo = repo;
            _user = user;
        }

        public void CreatePost(int recipeId, byte[] image, string fileName, string comment)
        {
            if (!_user.IsAuthenticated)
                throw new Exception("Not logged in");

            if (image == null)
                throw new Exception("Image required");

            if (image.Length > 5 * 1024 * 1024)
                throw new Exception("File too large");

            var post = new Post
            {
                UserId = _user.UserId,
                RecipeId = recipeId,
                Comment = comment,
                Status = PostStatus.Pending
            };

            _repo.Add(post);
        }
    }
}