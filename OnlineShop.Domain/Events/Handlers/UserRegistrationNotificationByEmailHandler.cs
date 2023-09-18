using MediatR;
using OnlineShop.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Domain.Events.Handlers
{
    public class UserRegistrationNotificationByEmailHandler : INotificationHandler<AccountRegistered>
    {
        private readonly IEmailSender _emailSender;

        public UserRegistrationNotificationByEmailHandler (IEmailSender emailSender)
        {
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        }
        public async Task Handle (AccountRegistered notification, CancellationToken cancellationToken)
        {
            await _emailSender.SendEmailAsync(notification.Account.Email, "Подтверждение регистрации", $"Вы успешно зарегистрированы", cancellationToken);
        }
    }
}
