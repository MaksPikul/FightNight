using fightnight.Server.Models.Tables;
using fightnight.Server.Models.Types;
using Microsoft.AspNetCore.SignalR;

namespace fightnight.Server.Hubs
{
    public class ChatHub : Hub
    {
        public async Task ConnectionReq(UserConnection conn)
        {
            var user = Context.User;
            Console.WriteLine(user);
            //check if user is authed
            //check if user in event
            await Groups
                .AddToGroupAsync(Context.ConnectionId, conn.eventId);

            await Clients
                .Group(conn.eventId)
                .SendAsync(
                "ConnectionRes", 
                conn.username + " Has joined lobby"
                );
        }

        public Task SendMsgReq(Message newMsg)
        {
            //accept connection or event Id
            // add message to corresponding event 
            // send message to corresponding event

            //if msg empty, do return null

            return Clients
                .Group(newMsg.eventId)
                .SendAsync("SendMsgRes", newMsg);
        }

        public Task DeleteMsgReq()
        {

        }

        public Task DisconnectReq()
        {

        }
    }
}
