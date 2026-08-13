using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Common.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendEmailConfirmationAsync(string toEmail, string confirmationLink);
        Task SendPasswordResetEmailAsync(string toEmail, string resetLink);
    }
}

