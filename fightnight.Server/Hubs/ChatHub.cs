using fightnight.Server.Models.Tables;
using fightnight.Server.Models.Types;
using Microsoft.AspNetCore.SignalR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace fightnight.Server.Hubs
{
    public class ChatHub : Hub
    {
        public async Task ConnectionReq(UserConnection conn)
        {
            await Groups
                .AddToGroupAsync(Context.ConnectionId, conn.eventId);

            await Clients
                .Group(conn.eventId)
                .SendAsync(
                "ConnectionRes", 
                "someone Has joined lobby"
                );


        }
        public async void ForceDisconnectReq(string connId)
        {
            await Clients.Client(connId).SendAsync("DisconnectRes", "You are being disconnected by the server.");

            var connection = Context.ConnectionAborted;
            if (!connection.IsCancellationRequested)
            {
                Context.Abort(); // This will disconnect the client
            }
        }
    }
}
