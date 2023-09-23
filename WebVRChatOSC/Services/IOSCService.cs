namespace WebVRChatOSC.Services
{
    public interface IOSCService : IDisposable
    {
        public void Send(string path, params object[] values);
    }
}
