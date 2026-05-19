using Moq;
using Kiskukta.Services;
using Kiskukta.Interfaces;
using Kiskukta.Models;

[TestFixture]
public class PostServiceTests
{
    private Mock<IPostRepository> _postRepositoryMock;
    private Mock<IUserContext> _userContextMock;
    private PostService _postService;

    [SetUp]
    public void Setup()
    {
        _postRepositoryMock = new Mock<IPostRepository>();
        _userContextMock = new Mock<IUserContext>();

        _postService = new PostService(
            _postRepositoryMock.Object,
            _userContextMock.Object
        );
    }

    [Test]
    public void CreatePost_ValidInput_ShouldCreatePostWithPendingStatus()
    {
        // Arrange
        int userId = 1;
        int recipeId = 10;

        _userContextMock.Setup(u => u.IsAuthenticated).Returns(true);
        _userContextMock.Setup(u => u.UserId).Returns(userId);

        Post savedPost = null;

        _postRepositoryMock
            .Setup(r => r.Add(It.IsAny<Post>()))
            .Callback<Post>(p => savedPost = p);

        // Act
        _postService.CreatePost(
            recipeId,
            new byte[10],
            "test.jpg",
            "Nice food"
        );

        // Assert
        NUnit.Framework.Assert.IsNotNull(savedPost);
        NUnit.Framework.Assert.AreEqual(userId, savedPost.UserId);
        NUnit.Framework.Assert.AreEqual(recipeId, savedPost.RecipeId);
        NUnit.Framework.Assert.AreEqual(PostStatus.Pending, savedPost.Status);

        _postRepositoryMock.Verify(r => r.Add(It.IsAny<Post>()), Times.Once);
    }
}

