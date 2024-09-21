﻿namespace fightnight.Server.Models.Tables
{
    public class Message
    {
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string eventId { get; set; } = string.Empty;
        public string userId { get; set; } = string.Empty;
        public string username { get; set; } = string.Empty;
        //public string userPicture {  get; set; } = string.Empty;
        public string message { get; set; } = string.Empty;
        public DateTime timeStamp { get; set; } = DateTime.Now;
        public bool IsEdited { get; set; } = false;
        public bool IsLoading { get; set; } = false;
    }
}
