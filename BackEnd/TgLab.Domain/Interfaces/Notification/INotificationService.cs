namespace TgLab.Domain.Interfaces.Notification
{
    public interface INotificationService
    {
        Task SendMessageAsync(string message);
    }
}
