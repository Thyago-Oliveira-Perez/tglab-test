using System.Net.WebSockets;

namespace TgLab.Application.Notification
{
    public class WebSocketServer
    {
        private readonly List<WebSocket> _sockets = new List<WebSocket>();

        public async Task AddSocketAsync(WebSocket socket)
        {
            _sockets.Add(socket);

            await Receive(socket);
        }

        public IEnumerable<WebSocket> GetAllSockets()
        {
            return _sockets;
        }

        private async Task Receive(WebSocket socket)
        {
            var buffer = new byte[1024 * 4];

            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    _sockets.Remove(socket);
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed", CancellationToken.None);
                }
            }
        }
    }
}