using TgLab.Domain.Interfaces.Notification;

namespace TgLab.Tests.Bet.Services.Mock
{
    public class InMemoryNotificationService : INotificationService
    {
        public Task SendMessageAsync(string message)
        {
            Console.WriteLine(message);

            return Task.CompletedTask;
        }
    }
}
