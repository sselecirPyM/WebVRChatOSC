
using CoreOSC;
using LiteDB;
using WebVRChatOSC.DTO;

namespace WebVRChatOSC.Services
{
    public class MyBackgroundService : BackgroundService
    {
        private readonly ILogger<MyBackgroundService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public MyBackgroundService(ILogger<MyBackgroundService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var textService = (TextService)scope.ServiceProvider.GetService<ITextService>();
            var osc = (OSCService)scope.ServiceProvider.GetService<IOSCService>();
            osc.OnReceiveOSC += History;

            using PeriodicTimer timer = new(TimeSpan.FromSeconds(0.2));

            try
            {
                textService.DoWork();
                while (await timer.WaitForNextTickAsync(stoppingToken))
                    textService.DoWork();
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("stop");
                osc.OnReceiveOSC -= History;
            }
        }

        void History(object sender, OscMessage msg)
        {
            if (msg.Address == "/avatar/change")
            {
                using var scope = _serviceProvider.CreateScope();
                var db = scope.ServiceProvider.GetService<ILiteDatabase>();
                db.GetCollection<AvatarHistory>("avatarHistory").Insert(new AvatarHistory()
                {
                    avatarId = (string)msg.Arguments[0],
                    created = DateTime.Now,
                });
            }
        }
    }
}
