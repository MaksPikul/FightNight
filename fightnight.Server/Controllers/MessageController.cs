using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace fightnight.Server.Controllers
{
    [Route("api/message")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddMessage()
        {
            
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetEventMessages()
        {
            return BadRequest("NOT Yet");
        }

        [HttpPatch]
        [Authorize]
        public async Task<IActionResult> EditMessage()
        {
            return BadRequest("");
        }


        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteMessage()
        {
            return BadRequest("not yet");
        }
    }
}
