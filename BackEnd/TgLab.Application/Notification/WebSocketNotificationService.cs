using System.Net.WebSockets;
using System.Text;
using TgLab.Domain.Interfaces.Notification;

namespace TgLab.Application.Notification
{
    public class WebSocketNotificationService : INotificationService
    {
        private readonly WebSocketServer _webSocketServer;

        public WebSocketNotificationService(WebSocketServer webSocketServer)
        {
            _webSocketServer = webSocketServer;
        }

        public async Task SendMessageAsync(string message)
        {
            foreach (var socket in _webSocketServer.GetAllSockets())
            {
                if (socket.State == WebSocketState.Open)
                {
                    var messageBytes = Encoding.UTF8.GetBytes(message);
                    var buffer = new ArraySegment<byte>(messageBytes);
                    await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
    }
}
