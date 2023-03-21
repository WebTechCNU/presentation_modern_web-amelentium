using JWTExample.Entities;
using JWTExample.Models;
using JWTExample.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTExample.Services
{
	public class AuthorizationService
	{
		private static readonly Dictionary<string, UserModel> _users;
		private readonly JWTSettings _settings;

		static AuthorizationService()
		{
			_users = new Dictionary<string, UserModel>
			{
				{ "user", new()
					{
						Id = 1,
						Login = "user",
						Password = "user",
						Role = "user"
					}
				},
				{ "admin", new()
					{
						Id = 1,
						Login = "admin",
						Password = "admin",
						Role = "admin"
					}
				}
			};
		}

		public AuthorizationService(IOptions<JWTSettings> settings)
		{
			_settings = settings.Value;
		}

		public string Login(LoginModel login)
		{
			if (_users.TryGetValue(login.Login, out var user) && user.Password == login.Password)
			{
				return CreateJWT(user);
			}

			return null;
		}

		private string CreateJWT(UserModel user)
		{
			var claims = new[] {
				new Claim(ClaimTypes.Name, user.Login),
				new Claim(ClaimTypes.Role, user.Role),
			};

			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
			var tokenDescriptor = new JwtSecurityToken(_settings.Issuer, _settings.Audience, claims,
				expires: DateTime.Now.AddMinutes(_settings.LifeTime), signingCredentials: credentials);

			return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
		}

		public bool TryValidateToken(string token, out SecurityToken validatedToken)
		{
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
			var tokenHandler = new JwtSecurityTokenHandler();
			try
			{
				tokenHandler.ValidateToken(token, new TokenValidationParameters
				{
					ValidateAudience = true,
					ValidAudience = _settings.Audience,
					ValidateIssuer = true,
					ValidIssuer = _settings.Issuer,
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = securityKey,
					ValidateLifetime = true
				}, out validatedToken);

				return true;
			}
			catch
			{
				validatedToken = null;

				return false;
			}
		}
	}
}
