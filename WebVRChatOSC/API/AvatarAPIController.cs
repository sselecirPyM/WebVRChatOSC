using LiteDB;
using Microsoft.AspNetCore.Mvc;
using WebVRChatOSC.DTO;

namespace WebVRChatOSC.API
{
    [ApiController]
    [Route("api/avatar")]
    public class AvatarAPIController : ControllerBase
    {
        [HttpGet("avatars")]
        public List<AvatarData> GetAvatars(ILiteDatabase db)
        {
            string path = Environment.ExpandEnvironmentVariables("%appdata%/../LocalLow/VRChat/VRChat/OSC/");

            List<AvatarData> avatarDatas = new List<AvatarData>();
            var filenames = Directory.EnumerateFiles(path, "*.json", new EnumerationOptions() { RecurseSubdirectories = true });
            foreach (var filename in filenames)
            {
                using var stream = System.IO.File.OpenRead(filename);
                avatarDatas.Add(System.Text.Json.JsonSerializer.Deserialize<AvatarData>(stream));
            }
            AvatarCollection(db).Upsert(avatarDatas);
            return avatarDatas;
        }

        [HttpPost("scan")]
        public int Scan(ILiteDatabase db)
        {
            return GetAvatars(db).Count;
        }

        [HttpPost("clear")]
        public int ClearAvatars(ILiteDatabase db)
        {
            return AvatarCollection(db).DeleteAll();
        }

        [HttpGet("last")]
        public AvatarData GetLast(ILiteDatabase db)
        {
            var last = db.GetCollection<AvatarHistory>("avatarHistory").Query().OrderByDescending(u => u.id).FirstOrDefault();
            if (last == null)
            {
                return null;
            }
            var avatarCollection = AvatarCollection(db);
            AvatarData data = avatarCollection.FindById(last.avatarId);
            data = avatarCollection.FindById(last.avatarId);
            if (data == null)
            {
                string path = Environment.ExpandEnvironmentVariables("%appdata%/../LocalLow/VRChat/VRChat/OSC/");
                var filenames = Directory.EnumerateFiles(path, $"{last.avatarId}.json", new EnumerationOptions() { RecurseSubdirectories = true });
                foreach (var filename in filenames)
                {
                    using var stream = System.IO.File.OpenRead(filename);
                    avatarCollection.Upsert(System.Text.Json.JsonSerializer.Deserialize<AvatarData>(stream));
                }
                data = avatarCollection.FindById(last.avatarId);
            }

            return data;
        }

        static ILiteCollection<AvatarData> AvatarCollection(ILiteDatabase db)
        {
            return db.GetCollection<AvatarData>("avatars");
        }
    }
}
