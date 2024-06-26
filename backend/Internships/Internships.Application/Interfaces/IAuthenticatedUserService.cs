namespace Internships.Core.Interfaces
{
    public interface IAuthenticatedUserService
    {
        string UserId { get; }
        public string Email { get; }
        public string Name { get; }
        public string Surname { get; }
    }
}