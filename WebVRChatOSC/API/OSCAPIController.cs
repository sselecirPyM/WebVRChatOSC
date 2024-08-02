using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebVRChatOSC.DTO;
using WebVRChatOSC.Services;

namespace WebVRChatOSC.API
{
    [ApiController]
    [Route("api/osc")]
    public class OSCAPIController : ControllerBase
    {
        [HttpPost]
        public string OSCCall([FromQuery] string path, IOSCService osc,[FromBody] JsonElement[] objects)
        {
            List<object> objects1 = new List<object>();
            foreach (var item in objects)
            {
                if (item is JsonElement element)
                {
                    if (element.ValueKind == JsonValueKind.Number)
                    {
                        if (element.TryGetInt32(out int val))
                        {
                            objects1.Add(val);
                        }
                        else if (element.TryGetSingle(out float val1))
                        {
                            objects1.Add(val1);
                        }
                    }
                    else if (element.ValueKind == JsonValueKind.True)
                    {
                        objects1.Add(true);
                    }
                    else if (element.ValueKind == JsonValueKind.False)
                    {
                        objects1.Add(false);
                    }
                    else if (element.ValueKind == JsonValueKind.String)
                    {
                        objects1.Add(element.GetString());
                    }
                }
            }
            osc.Send(path, objects1.ToArray());
            return path;
        }
    }
}
