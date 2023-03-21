using JWTExample.Services;
using JWTExample.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

var services = builder.Services;

services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(opt =>
	{
		opt.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateAudience = true,
			ValidAudience = config["JWT:Audience"],
			ValidateIssuer = true,
			ValidIssuer = config["JWT:Issuer"],
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"])),
			ValidateLifetime = true
		};
	});

services.Configure<JWTSettings>(config.GetSection(JWTSettings.SectionName));
services.AddScoped<AuthorizationService>();
services.AddScoped<DataService>();

services.AddControllers();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/error");
}

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
	endpoints.MapDefaultControllerRoute();
});

app.Run();
