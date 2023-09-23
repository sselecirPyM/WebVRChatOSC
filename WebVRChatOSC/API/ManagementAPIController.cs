using Microsoft.AspNetCore.Mvc;
using WebVRChatOSC.DTO;
using WebVRChatOSC.Services;

namespace WebVRChatOSC.API
{
    [ApiController]
    [Route("api/management")]
    public class ManagementAPIController : ControllerBase
    {
        [HttpGet("address")]
        public SetAddressData GetAddress()
        {
            return new SetAddressData
            {
                host = Config.Instance.oscHost,
                recv_port = Config.Instance.oscRecvPort,
                send_port = Config.Instance.oscSendPort,
            };
        }


        [HttpPost("address")]
        public void SetAddress(SetAddressData request)
        {
            Config.Instance.ChangeHost(request.host, request.send_port, request.recv_port);
        }
    }
}
