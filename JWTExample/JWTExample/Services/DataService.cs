using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace JWTExample.Services
{
	public class DataService
	{
		private static readonly Dictionary<string, string> _data;
		private readonly AuthorizationService _authorizationService;

		static DataService()
		{
			_data = new Dictionary<string, string>()
			{
				{ "all", "Data for all users." },
				{ "auth", "Data for authorized users." },
				{ "admin", "Data for users with admin role." },
			};
		}

		public DataService(AuthorizationService authorizationService)
		{
			_authorizationService = authorizationService;
		}

		public IEnumerable<string> GetData(HttpContext context = null)
		{
			var data = new List<string>() { _data["all"] };

			if (context != null)
			{
				var token = context.Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", string.Empty);

				if (token != null && _authorizationService.TryValidateToken(token, out _))
				{
					data.Add(_data["auth"]);

					if (_data.TryGetValue(context.User.Claims.FirstOrDefault(x =>
						x.Type == ClaimTypes.Role)?.Value, out var roleSpecificData))
					{
						data.Add(roleSpecificData);
					}

				}
			}

			return data;
		}
	}
}
