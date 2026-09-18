using Microsoft.AspNetCore.Mvc;

namespace BigCommerceApi.Client.Web.Controllers
{
	[Route("ping")]
	public sealed class PingController : ControllerBase
	{
		[HttpGet]
		public IActionResult Ping()
		{
			return Ok("Pong");
		}
	}
}
