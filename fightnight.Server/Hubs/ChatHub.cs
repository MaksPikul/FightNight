using fightnight.Server.Models.Tables;
using fightnight.Server.Models.Types;
using Microsoft.AspNetCore.SignalR;

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
    

        public async Task SendMsgReq(Message newMsg)
        {
            await Clients.Group(newMsg.eventId).SendAsync("SendMsgRes", newMsg);
        }

        public Task DeleteMsgReq()
        {
            return null;
        }

        public Task DisconnectReq()
        {
            return null;
        }
    }
}
