namespace WebVRChatOSC.DTO
{
    public class AvatarData
    {
        [LiteDB.BsonId(false)]
        public string id { get; set; }
        public string name { get; set; }
        public Parameter[] parameters { get; set; }

        public class Parameter
        {
            public string name { get; set; }

            public Description input { get; set; }
            public Description output { get; set; }
        }
        public class Description
        {
            public string address { get; set; }
            public string type { get; set; }
        }
    }
}
