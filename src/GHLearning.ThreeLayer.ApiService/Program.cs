using GHLearning.ThreeLayer.Repositories.Entities;
using GHLearning.ThreeLayer.Services.User;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using System.Net.Mime;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
	.AddRouting(options => options.LowercaseUrls = true)
	.AddControllers(options =>
	{
		options.Filters.Add(new ProducesAttribute(MediaTypeNames.Application.Json));
		options.Filters.Add(new ConsumesAttribute(MediaTypeNames.Application.Json));
	})
	.AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen(c =>
{
	// 設定 Swagger 支援 Cookie 身份驗證
	c.AddSecurityDefinition("AspNetCore.Cookies", new OpenApiSecurityScheme
	{
		Type = SecuritySchemeType.ApiKey,    // 使用 API Key 方式，因為 Cookie 會被當作 API Key 傳遞
		Name = ".AspNetCore.Cookies",                 // 設定 Cookie 名稱，與你設置的 Cookie 名稱一致
		In = ParameterLocation.Cookie,       // 指定 Cookie 傳遞的方式
		Description = "Use your AspNetCore.Cookies to authenticate"  // 描述
	});

	// 設定安全要求，告訴 Swagger 需要這個 Cookie 來進行身份驗證
	c.AddSecurityRequirement(new OpenApiSecurityRequirement
		{
			{
				new OpenApiSecurityScheme
				{
					Reference = new OpenApiReference
					{
						Type = ReferenceType.SecurityScheme,
						Id = "AspNetCore.Cookies"
					}
				},
				Array.Empty<string>()
			}
		});
});

builder.Services
	.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie();

builder.Services
	.AddCore()
	.AddRepositories((sp, dbBuilder) => dbBuilder.UseMySql(
		builder.Configuration.GetConnectionString("MySql"),
		ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySql"))))
	.AddServices();

builder.Services.AddSingleton(TimeProvider.System);

builder.Services.AddHealthChecks()
	.AddCheck("self", () => HealthCheckResult.Healthy(), tags: ["live"])
	.AddDbContextCheck<SampleContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	app.UseSwagger();
	app.UseSwaggerUI();// swagger/
	app.UseReDoc();//api-docs/
	app.MapScalarApiReference();//scalar/v1
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseHealthChecks("/live", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
	Predicate = check => check.Tags.Contains("live"),
	ResultStatusCodes =
	{
		[HealthStatus.Healthy] = StatusCodes.Status200OK,
		[HealthStatus.Degraded] = StatusCodes.Status200OK,
		[HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
	}
});
app.UseHealthChecks("/healthz", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
	Predicate = _ => true,
	ResultStatusCodes =
	{
		[HealthStatus.Healthy] = StatusCodes.Status200OK,
		[HealthStatus.Degraded] = StatusCodes.Status200OK,
		[HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
	}
});

var factory = app.Services.GetRequiredService<IDbContextFactory<SampleContext>>();
using var context = await factory.CreateDbContextAsync().ConfigureAwait(false);
context.Database.Migrate();

using (var scope = app.Services.CreateScope())
{
	var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
	var password = "1qaz2wsx";

	if (await mediator.Send(new UserGetRequest("gordon")).ConfigureAwait(false) is null)
	{
		using var cts = new CancellationTokenSource();
		await mediator.Send(new UserAddRequest("gordon", password, "Gordon Hung"), cts.Token).ConfigureAwait(false);
	}

	if (await mediator.Send(new UserGetRequest("andy")).ConfigureAwait(false) is null)
	{
		using var cts = new CancellationTokenSource();
		await mediator.Send(new UserAddRequest("andy", password, "Andy Lin"), cts.Token).ConfigureAwait(false);
	}
}

app.Run();
