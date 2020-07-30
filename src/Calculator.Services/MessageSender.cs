using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Services
{
    public class MessageSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string messageBody)
        {
            // TODO: implement email service logic here
            return Task.FromResult(0);
        }
    }
}
