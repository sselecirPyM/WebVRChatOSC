namespace WebVRChatOSC.DTO
{
    public class ChatData
    {
        [LiteDB.BsonId(true)]
        public int id { get; set; }
        public string content { get; set; }
        public int duration { get; set; }
        public DateTime created { get; set; }
    }
}
