using Microsoft.AspNetCore.Mvc;
using System.Net.Sockets;
using System.Net;
using System.Reflection;

namespace GHLearning.ThreeLayer.ApiService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InfoController : ControllerBase
{
	[HttpGet]
	public async Task<object> GetAsync(
		[FromServices] IWebHostEnvironment hostingEnvironment,
		[FromServices] IConfiguration configuration)
	{
		var hostName = Dns.GetHostName();
		var hostEntry = await Dns.GetHostEntryAsync(hostName).ConfigureAwait(false);
		var hostIp = Array.Find(hostEntry.AddressList,
			x => x.AddressFamily == AddressFamily.InterNetwork);

		return new
		{
			Environment.MachineName,
			HostName = hostName,
			HostIp = hostIp?.ToString() ?? string.Empty,
			Environment = hostingEnvironment.EnvironmentName,
			OsVersion = $"{Environment.OSVersion}",
			Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString(),
			ProcessCount = Environment.ProcessorCount,
			ConnectionStrings = configuration.GetSection("ConnectionStrings").GetChildren()
		};
	}
}
