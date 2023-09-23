using Microsoft.AspNetCore.Mvc;
using WebVRChatOSC.DTO;
using WebVRChatOSC.Services;

namespace WebVRChatOSC.API
{
    [ApiController]
    [Route("api/chat")]
    public class ChatAPIController : ControllerBase
    {
        [HttpPost("message")]
        public void Message(ITextService textService, ChatRequest request)
        {
            textService.Chat(request.content, request.duration);
        }

        //[HttpGet("history")]
        //public IEnumerable<ChatData> History()
        //{

        //    return Array.Empty<ChatData>();
        //}
    }
}
