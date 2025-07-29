using Assiginment.Models;

namespace Assiginment.Services
{
    public interface IEmailService
    {
        Task SendApprovalEmailAsync(int userId, string email);
        // Task UpdateStatusAsync(StatusUpdateRequest request);
        Task SendWelcomeEmailAsync(string email, string firstName, int contractId);

        Task FirstPasswordEmailAsync(string toEmail, string subject, string body);
    }

}
