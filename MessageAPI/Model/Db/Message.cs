﻿
namespace MessageAPI.Model.Db
{
    public class Message
    {
        public int Id { get; set; }

        public bool Unread { get; set; } = true;
        public string EmailFrom { get; set; }
        public string EmailTo { get; set; }
        public string Text { get; set; }
        public virtual User User { get; set; }
    }
}
