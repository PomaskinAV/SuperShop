using MediatR;
using Microsoft.Extensions.Logging;
using OnlineShop.Domain.Events.Handlers;
using OnlineShop.Domain.Events;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain;
using Moq;
using FluentAssertions;
using OnlineShop.Domain.Entities;


namespace MySuperShop.Domain.Test
{
    public class AccountTests
    {
        [Fact]
        public async void Account_registered_event_notifies_user_by_email()
        {

            var account = new Account(Guid.NewGuid(), "John", "John@john.com", "qwerty", new[] { Role.Customer });

            var loggerMock = new Mock<ILogger<UserRegistrationNotificationByEmailHandler>>();

            var emailSenderMock = new Mock<IEmailSender>();


            var logger = new Logger<UserRegistrationNotificationByEmailHandler>(new LoggerFactory());

            var handler = new UserRegistrationNotificationByEmailHandler(emailSenderMock.Object, loggerMock.Object);
            var @event = new AccountRegistered(account, DateTime.Now);


            await handler.Handle(@event, default);

            handler.Should().BeAssignableTo<INotificationHandler<AccountRegistered>>();

            emailSenderMock
                .Verify(it =>
                    it.SendEmailAsync(account.Email, It.IsAny<string>(), It.IsAny<string>(), default), Times.Once);

        }

    }
}
