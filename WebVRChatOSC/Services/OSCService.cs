using CoreOSC;

namespace WebVRChatOSC.Services
{
    public class OSCService : IOSCService
    {
        UDPSender messageSender;
        UDPListener messageListener;

        public event EventHandler<OscMessage> OnReceiveOSC;

        public OSCService(IServiceProvider provider)
        {
            var config = Config.Instance;
            config.HostChanged += Config_HostChanged;
            messageListener = new UDPListener(config.oscRecvPort, HandleOSC);
            messageSender = new UDPSender(config.oscHost, config.oscSendPort);
        }

        void HandleOSC(OscPacket oscPacket)
        {
            if (oscPacket is OscMessage msg)
            {
                OnReceiveOSC?.Invoke(this, msg);
                ProcessOSC(msg);
            }
            else if (oscPacket is OscBundle bundle)
            {
                foreach (var msg1 in bundle.Messages)
                {
                    OnReceiveOSC?.Invoke(this, msg1);
                    ProcessOSC(msg1);
                }
            }
        }

        void ProcessOSC(OscMessage msg)
        {
            Console.Write(msg.Address);
            for (int i = 0; i < msg.Arguments.Count; i++)
            {
                if (i <= msg.Arguments.Count - 1)
                    Console.Write(", ");
                Console.Write(msg.Arguments[i]);
            }
            Console.WriteLine();
        }

        void Config_HostChanged(object sender, EventArgs e)
        {
            var config = Config.Instance;
            lock (config)
            {
                messageSender?.Close();
                messageSender = new UDPSender(config.oscSendPort);
                messageListener?.Close();
                messageListener = new UDPListener(config.oscRecvPort, HandleOSC);
            }
        }

        public void Send(string path, params object[] value)
        {
            var config = Config.Instance;
            lock (config)
            {
                messageSender.Send(new OscMessage(path, value));
            }
        }

        public void Dispose()
        {
            var config = Config.Instance;
            config.HostChanged -= Config_HostChanged;
            messageSender?.Close();
            messageListener?.Close();
        }
    }
}
