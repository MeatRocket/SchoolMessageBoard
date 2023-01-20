using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.SignalR;
using System.Net;

namespace AdminPortal.Hubs
{
    public class CommentHub : Hub, IConnectionFactory
    {
        public const string CommentUrl = "/DynamicPostView";
        public async Task SendMessage(string user, string message) => await Clients.All.SendAsync("ReceiveMessage", user, message);

        public async Task Broadcast(string username, string message)
        {
            await Clients.All.SendAsync("Broadcast", username, message);
        }

        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"{Context.ConnectionId} connected");
            return base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception e)
        {
            Console.WriteLine($"Disconnected {e?.Message} {Context.ConnectionId}");
            await base.OnDisconnectedAsync(e);
        }

        public ValueTask<ConnectionContext> ConnectAsync(EndPoint endpoint, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
