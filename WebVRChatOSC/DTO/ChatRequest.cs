using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebVRChatOSC.DTO
{
    public class ChatRequest
    {
        [MaxLength(144)]
        public string content { get; set; }
        [DefaultValue(30)]
        public int duration { get; set; } = 30;
    }
}
