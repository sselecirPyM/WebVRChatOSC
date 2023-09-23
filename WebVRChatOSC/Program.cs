using LiteDB;
using WebVRChatOSC.Services;

namespace WebVRChatOSC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(Directory.GetCurrentDirectory());
            //Directory.SetCurrentDirectory(Path.GetDirectoryName(Environment.ProcessPath));
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers().AddJsonOptions((u) =>
            {
                u.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            });
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSingleton<IOSCService, OSCService>();
            builder.Services.AddSingleton<ITextService, TextService>();
            builder.Services.AddSingleton<ILiteDatabase, LiteDatabase>((provider) => new LiteDatabase("data.db"));


            builder.Services.AddHostedService<MyBackgroundService>();
            Config.Load();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            app.UseSwagger();
            app.UseSwaggerUI();
            //}

            app.UseAuthorization();

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.MapControllers();

            {
                using var scope = app.Services.CreateScope();
                var osc = scope.ServiceProvider.GetService<OSCService>();
                var litedb = scope.ServiceProvider.GetService<ILiteDatabase>();
                litedb.Rebuild();
            }

            app.Run();
        }
    }
}