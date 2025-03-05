using GHLearning.ThreeLayer.Repository.Entities;
using GHLearning.ThreeLayer.Service.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
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

builder.Services
	.AddCore()
	.AddRepository((sp, dbBuilder) => dbBuilder.UseMySql(
		builder.Configuration.GetConnectionString("MySql"),
		ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySql"))))
	.AddService();

builder.Services.AddSingleton(TimeProvider.System);

builder.Services.AddHealthChecks()
	.AddCheck("self", () => HealthCheckResult.Healthy(), tags: ["live"])
	.AddDbContextCheck<SampleContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "OpenAPI V1"));// swagger/
	app.UseReDoc(options => options.SpecUrl("/openapi/v1.json"));//api-docs/
	app.MapScalarApiReference();//scalar/v1
}

app.UseHttpsRedirection();

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
