using System.Text.Json;

namespace WebVRChatOSC.Services
{
    public class Config
    {
        public static Config Instance { get; set; }
        public string oscHost { get; set; }
        public int oscRecvPort { get; set; }
        public int oscSendPort { get; set; }

        public static void Load()
        {
            Config.Instance = JsonSerializer.Deserialize<Config>(File.ReadAllText("config.json"));
        }

        public static void Save()
        {
            File.WriteAllText("config.json", JsonSerializer.Serialize(Config.Instance));
        }

        public void ChangeHost(string host, int send, int recv)
        {
            this.oscHost = host;
            this.oscSendPort = send;
            this.oscRecvPort = recv;
            Config.Save();
            HostChanged?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler<EventArgs> HostChanged;
    }
}
