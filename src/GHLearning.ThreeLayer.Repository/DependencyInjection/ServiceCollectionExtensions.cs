﻿using System.Reflection;
using GHLearning.ThreeLayer.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddRepository(
		this IServiceCollection services,
		Action<IServiceProvider, DbContextOptionsBuilder> dbContextOptionsBuilder)
		=> services
		.AddDbContextFactory<SampleContext>(dbContextOptionsBuilder)
		.Scan(scan => scan
		.FromAssemblies(Assembly.GetExecutingAssembly())
		.AddClasses(
			filter => filter.Where(x => x.Name.EndsWith("repository", StringComparison.OrdinalIgnoreCase)),
			publicOnly: false)
		.AsImplementedInterfaces()
		.WithTransientLifetime());
}
