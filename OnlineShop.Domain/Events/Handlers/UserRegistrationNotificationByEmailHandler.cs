using MediatR;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<UserRegistrationNotificationByEmailHandler> _logger;

        public UserRegistrationNotificationByEmailHandler(
            IEmailSender emailSender,
            ILogger<UserRegistrationNotificationByEmailHandler> _logger)
        {
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            this._logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
        }

        public async Task Handle(AccountRegistered notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start of email sending about success of Registration");
            await _emailSender.SendEmailAsync(
                notification.Account.Email,
                "Подтверждение рагистрации",
                "Вы успешно зарегистрированы",
                cancellationToken);
        }
    }
}
