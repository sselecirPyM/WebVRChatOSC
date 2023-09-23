namespace WebVRChatOSC.DTO
{
    public class AvatarHistory
    {
        [LiteDB.BsonId(true)]
        public int id { get; set; }
        public string avatarId { get;set; }
        public DateTime created { get; set; }
    }
}
