namespace Kiskukta.Interfaces
{
    public interface IUserContext
    {
        bool IsAuthenticated { get; }
        int UserId { get; }
    }
}
