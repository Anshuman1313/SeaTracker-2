using Assiginment.Models;
using Assiginment.Services;
using System.Net.Mail;
using System.Text.Json;

public class EmailService : IEmailService
{
    private readonly SmtpClient _smtpClient;
    private readonly string _jsonFilePath = "D:\\Assiginment11\\Assiginment\\Assiginment\\Data\\contacts.json\r\n"; // Replace with actual path
    public EmailService(SmtpClient smtpClient)
    {
        _smtpClient = smtpClient;
    }

    public async Task SendApprovalEmailAsync(int userId, string email)
    {
        var mailMessage = new MailMessage
        {
            From = new MailAddress("demo@gmail.com"),
            Subject = "Approval Request",
            Body = GenerateEmailBody(userId),
            IsBodyHtml = true,
        };
        mailMessage.To.Add(email);

        await _smtpClient.SendMailAsync(mailMessage);
    }
    public async Task FirstPasswordEmailAsync(string toEmail, string subject, string body)
    {   
        var mailMessage = new MailMessage()
        {
            From = new MailAddress("rana13anshu@gmail.com", "Sea Tracker Team"),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };
        mailMessage.To.Add(toEmail);
        await _smtpClient.SendMailAsync(mailMessage);
    }
    public async Task SendWelcomeEmailAsync(string email, string firstName, int contractId)
    {

        try
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress("singhpraveen082410@gmail.com"),
                Subject = "Welcome to Our Platform!",
                Body = GenerateWelcomeEmailBody(firstName, contractId),
                IsBodyHtml = true,
            };
            mailMessage.To.Add(email);

            await _smtpClient.SendMailAsync(mailMessage);
        }
        catch (Exception)
        {

            throw;
        }
       
    }


    //public async Task UpdateStatusAsync(StatusUpdateRequest request)
    //{
        
    //    var users = await LoadUsersAsync();

        
    //    var user = users.FirstOrDefault(u => u.Id == request.UserId);
    //    if (user != null)
    //    {
    //        user.Status = request.Status;
    //        //user.Comment = request.Comment;

    //        // Save the updated list back to the JSON file
    //        await SaveUsersAsync(users);
    //    }
    //    else
    //    {
    //        throw new Exception("User not found.");
    //    }
    //}

    private string GenerateEmailBody(int userId)
    {
        return $@"
            <div style='padding: 20px;'>
                <h3>Please Accept the Request</h3>
                <textarea id='comment' placeholder='Enter your comment here' required></textarea><br/><br/>
                <button onclick='approveRequest({userId})' id='approveBtn' style='background-color: blue; color: white; padding: 10px;'>Approve</button>
                <button onclick='rejectRequest({userId})' id='rejectBtn' style='background-color: red; color: white; padding: 10px;'>Reject</button>
                <script>
                    function approveRequest(userId) {{
                        var comment = document.getElementById('comment').value;
                        if (!comment) {{
                            alert('Please enter a comment before approving.');
                            return;
                        }}
                        fetch('https://your-backend-url/api/Email/UpdateStatus', {{
                            method: 'POST',
                            headers: {{ 'Content-Type': 'application/json' }},
                            body: JSON.stringify({{ userId: userId, status: 'Approved', comment: comment }})
                        }})
                        .then(response => response.json())
                        .then(data => {{
                            document.getElementById('approveBtn').disabled = true;
                            document.getElementById('rejectBtn').disabled = true;
                        }});
                    }}
                    function rejectRequest(userId) {{
                        var comment = document.getElementById('comment').value;
                        fetch('https://your-backend-url/api/Email/UpdateStatus', {{
                            method: 'POST',
                            headers: {{ 'Content-Type': 'application/json' }},
                            body: JSON.stringify({{ userId: userId, status: 'Rejected', comment: comment }})
                        }})
                        .then(response => response.json())
                        .then(data => {{
                            document.getElementById('approveBtn').disabled = true;
                            document.getElementById('rejectBtn').disabled = true;
                        }});
                    }}
                </script>
            </div>";
    }
    private string GenerateWelcomeEmailBody(string firstName,int EmployeeId)
    {
        return $@"
        <html>
            <body>
                <h1>Welcome to Our Platform, {firstName}!</h1>
                <p>Dear {firstName},</p>
                <p>We are excited to have you on board. Explore our services and make the most out of your journey with us.</p>
                <p>If you have any questions, feel free to contact us at demo@gmail.com.</p>
                <p>Your Contract Id is {EmployeeId}</p>
                <br />
                <p>Best Regards,</p>
                <p>The Demo Team</p>
            </body>
        </html>";
    }

    private async Task<List<Employee>> LoadUsersAsync()
    {
        using var stream = new FileStream(_jsonFilePath, FileMode.Open, FileAccess.Read);
        return await JsonSerializer.DeserializeAsync<List<Employee>>(stream);
    }

    private async Task SaveUsersAsync(List<Employee> users)
    {
        using var stream = new FileStream(_jsonFilePath, FileMode.Create, FileAccess.Write);
        await JsonSerializer.SerializeAsync(stream, users);
    }
}
