

using System.Threading.Tasks;

namespace Internships.Core.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail(string email, string url);
    }
}