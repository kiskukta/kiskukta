using NUnit.Framework;
using Moq;
using Kiskukta.Services;
using Kiskukta.Interfaces;
using Kiskukta.Models;
using System;

[TestFixture]
public class PostModerationTests
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
    public void ApprovePost_NonAdminUser_ShouldThrowUnauthorizedException()
    {
        // Arrange
        int postId = 5;

        var post = new Post
        {
            Id = postId,
            Status = PostStatus.Pending
        };

        _postRepositoryMock.Setup(r => r.GetById(postId)).Returns(post);
        _userContextMock.Setup(u => u.IsAdmin).Returns(false);

        // Act & Assert
        NUnit.Framework.Assert.Throws<UnauthorizedAccessException>(() =>
            _postService.ApprovePost(postId)
        );

        _postRepositoryMock.Verify(r => r.Update(It.IsAny<Post>()), Times.Never);
    }
}
