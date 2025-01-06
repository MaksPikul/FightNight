using fightnight.Server.Interfaces;
using fightnight.Server.Models.Tables;
using fightnight.Server.Models.Types;
using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace fightnight.Server.Hubs
{
    public class ChatHub(
        ICacheService cacheService
    ) : Hub
    {
        private readonly ICacheService _cacheService = cacheService;



        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var eventId = httpContext.Request.Query["eventId"];
            var userId = httpContext.Request.Query["userId"];

            string connectionKey = this.buildKey(eventId, userId);
            _cacheService.AddToCacheAsync(connectionKey, Context.ConnectionId);
            
            await Groups
                .AddToGroupAsync(Context.ConnectionId, eventId);

            await Clients
                .Group(eventId)
                .SendAsync(
                "ConnectionRes",
                "someone Has joined lobby"
                );

            base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {

            var httpContext = Context.GetHttpContext();
            var eventId = httpContext.Request.Query["eventId"];
            var userId = httpContext.Request.Query["userId"];

            string connectionKey = this.buildKey(eventId, userId);
            string connectionId = await _cacheService.GetFromCacheAsync(connectionKey);
            
            _cacheService
                .RemoveFromCacheAsync(connectionKey);
            Groups
                .RemoveFromGroupAsync(connectionId, eventId);

            var connection = Context.ConnectionAborted;
            if (!connection.IsCancellationRequested)
            {
                Context.Abort(); // This will disconnect the client
            }

            base.OnDisconnectedAsync(exception);
        }

        public async void ForceDisconnectReq(string eventId)
        {
            string connectionId = Context.ConnectionId;

            Groups
                .RemoveFromGroupAsync(connectionId, eventId);

            var connection = Context.ConnectionAborted;
            if (!connection.IsCancellationRequested)
            {
                Context.Abort(); // This will disconnect the client
            }

            await Clients
                .Client(connectionId)
                .SendAsync("DisconnectRes", "You Have Been Removed From Event.");
        }

        private string buildKey(string eventId, string userId)
        {
            string connectionKey = $"chat/event:{eventId}/user:{userId}";
            return connectionKey;
        }
    }
}
