namespace WebVRChatOSC.DTO
{
    public class ButtonData
    {
        [LiteDB.BsonId(true)]
        public int id { get; set; }
        public string label { get; set; }
        public string icon { get; set; }
        public string action { get; set; }
        public string deactiveAction { get; set; }
        public string color { get; set; }
        public int category { get; set; }
        public int type { get; set; }
        public float min { get; set; }
        public float max { get; set; }
    }
}
