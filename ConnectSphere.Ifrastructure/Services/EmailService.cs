using ConnectSphere.Application.Common.Interfaces.Services;
using ConnectSphere.Application.Common.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace ConnectSphere.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<SmtpSettings> smtpSettings, ILogger<EmailService> logger)
        {
            _smtpSettings = smtpSettings.Value;
            _logger = logger;
        }

        public async Task SendEmailConfirmationAsync(string toEmail, string confirmationLink)
        {
            var subject = "Confirm Your Email - ConnectSphere";

            var body = $"""
                <html>
                <body style="font-family: Arial, sans-serif; line-height: 1.6; color: #333;">
                    <div style="max-width: 600px; margin: 0 auto; padding: 20px;">
                        <h2 style="color: #2c3e50;">Welcome to ConnectSphere!</h2>
                        <p>Thank you for registering. Please confirm your email address by clicking the link below:</p>
                        <p style="margin: 25px 0;">
                            <a href="{confirmationLink}"
                               style="background-color: #3498db; color: #ffffff; padding: 12px 24px;
                                      text-decoration: none; border-radius: 5px; display: inline-block;">
                                Confirm Email
                            </a>
                        </p>
                        <p>If the button doesn't work, copy and paste this link into your browser:</p>
                        <p style="word-break: break-all; color: #7f8c8d;">{confirmationLink}</p>
                        <p>If you didn't create an account, you can safely ignore this email.</p>
                        <hr style="border: none; border-top: 1px solid #eee; margin: 20px 0;" />
                        <p style="font-size: 12px; color: #999;">ConnectSphere Team</p>
                    </div>
                </body>
                </html>
                """;

            await SendEmailAsync(toEmail, subject, body);
        }

        public async Task SendPasswordResetEmailAsync(string toEmail, string resetLink)
        {
            var subject = "Reset Your Password - ConnectSphere";
            var body = $"""
                <html>
                <body style="font-family: Arial, sans-serif; line-height: 1.6; color: #333;">
                    <div style="max-width: 600px; margin: 0 auto; padding: 20px;">
                        <h2 style="color: #2c3e50;">Reset Your Password</h2>
                        <p>We received a request to reset your password. Click the button below to proceed:</p>
                        <p style="margin: 25px 0;">
                            <a href="{resetLink}"
                               style="background-color: #e74c3c; color: #ffffff; padding: 12px 24px;
                                      text-decoration: none; border-radius: 5px; display: inline-block;">
                                Reset Password
                            </a>
                        </p>
                        <p>If the button doesn't work, copy and paste this link into your browser:</p>
                        <p style="word-break: break-all; color: #7f8c8d;">{resetLink}</p>
                        <p>This link will expire in <strong>24 hours</strong>.</p>
                        <p>If you didn't request a password reset, you can safely ignore this email.</p>
                        <hr style="border: none; border-top: 1px solid #eee; margin: 20px 0;" />
                        <p style="font-size: 12px; color: #999;">ConnectSphere Team</p>
                    </div>
                </body>
                </html>
                """;

            await SendEmailAsync(toEmail, subject, body);
        }

        private async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
        {
            using var message = new MailMessage();
            message.From = new MailAddress(_smtpSettings.From);
            message.To.Add(new MailAddress(toEmail));
            message.Subject = subject;
            message.Body = htmlBody;
            message.IsBodyHtml = true;

            using var client = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port);
            client.Credentials = new NetworkCredential(_smtpSettings.UserName, _smtpSettings.Password);
            client.EnableSsl = true;

            try
            {
                await client.SendMailAsync(message);

                _logger.LogInformation("Confirmation email sent to {Email}", toEmail);
            }
            catch (SmtpException ex)
            {
                _logger.LogError(ex, "Failed to send confirmation email to {Email}", toEmail);
                throw;
            }
        }
    }
}
