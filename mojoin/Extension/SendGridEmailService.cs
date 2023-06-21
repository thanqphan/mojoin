namespace mojoin.Extension;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

public class SendGridEmailService : IEmailService
{
    private readonly SendGridSettings _sendGridSettings;

    public SendGridEmailService(IOptions<SendGridSettings> sendGridSettings)
    {
        _sendGridSettings = sendGridSettings.Value;
    }

    public async Task SendEmail(string toEmail, string subject, string body)
    {
        try
        {
            var client = new SendGridClient(_sendGridSettings.ApiKey);
            var from = new EmailAddress(_sendGridSettings.FromEmail, _sendGridSettings.FromName);
            var to = new EmailAddress(toEmail);
            var email = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent: null, htmlContent: body);
            var response = await client.SendEmailAsync(email);

            // Kiểm tra kết quả gửi email
            if (response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                // Email đã được gửi thành công
                Console.WriteLine("Email sent successfully!");
            }
            else
            {
                // Gửi email không thành công, xử lý lỗi tại đây
                Console.WriteLine($"Failed to send email. Status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            // Xử lý lỗi khi gửi email
            Console.WriteLine($"Error sending email: {ex.Message}");
        }
    }

}
