using JWTExample.Models;
using JWTExample.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace JWTExample.Controllers
{
	[Route("[controller]")]
	public class HomeController : ControllerBase
	{
		private readonly AuthorizationService _authorizationService;
		private readonly DataService _dataService;

		public HomeController(AuthorizationService authorizationService, DataService dataService)
		{
			_authorizationService = authorizationService;
			_dataService = dataService;
		}

		[HttpPost("login")]
		public ActionResult<string> Login([FromBody] LoginModel loginModel)
		{
			var token = _authorizationService.Login(loginModel);

			if (token == null)
			{
				return BadRequest("Invalid credentials");
			}

			return Ok(token);
		}

		[HttpGet("data")]
		public ActionResult<IEnumerable<string>> GetData()
		{
			return Ok(_dataService.GetData(HttpContext));
		}
	}
}
