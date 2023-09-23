namespace WebVRChatOSC.Services
{
    public class TextService : ITextService
    {
        private readonly ILogger<TextService> _logger;
        private readonly IServiceProvider _serviceProvider;

        static List<string> messages = new List<string>();

        CurrentMessage previous = null;
        public CurrentMessage currentMessage { get; private set; }

        int index = 0;
        int tickCount = 100;

        public void Chat(string message, int duration)
        {
            _logger.LogInformation(message);
            currentMessage = new CurrentMessage()
            {
                message = message,
                duration = duration,
                created = DateTime.Now
            };
        }

        public void DoWork()
        {
            tickCount++;
            if (ChatMessage())
                return;

            if (messages.Count == 0)
                return;
            index++;
            using var scope = _serviceProvider.CreateScope();
            scope.ServiceProvider.GetService<IOSCService>().Send("/chatbox/input", messages[index % messages.Count], true, true);
        }

        bool ChatMessage()
        {
            if (currentMessage == null)
                return false;
            if (currentMessage.created.AddSeconds(currentMessage.duration) < DateTimeOffset.Now)
                return false;

            if (tickCount >= 50 || previous != currentMessage)
            {
                tickCount = 0;
                previous = currentMessage;
                using var scope1 = _serviceProvider.CreateScope();
                scope1.ServiceProvider.GetService<IOSCService>().Send("/chatbox/input", currentMessage.message, true, true);
            }
            return true;

        }

        public TextService(ILogger<TextService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _logger.LogInformation("text service init");
        }

        public class CurrentMessage
        {
            public DateTime created;
            public string message;
            public int duration;
        }
    }
}
