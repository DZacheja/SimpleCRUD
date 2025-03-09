using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using Radio.API.Helpers;
using Radio.Application.Services;
using Radio.Application.Services.Interfaces;
using Radio.Domain.Models;
using Radio.Infrastructure.Presistence;
using Radio.Infrastructure.Repositories;
using Radio.Infrastructure.Repositories.Interfaces;
using RadioAPI.Endpoints;
using Host = Radio.Domain.Models.Host;

var logger = LogManager.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();
var builder = WebApplication.CreateBuilder(args);
// Add NLog
builder.Logging.ClearProviders();
builder.Host.UseNLog();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Register Database
builder.Services.AddDbContext<RadioDbContext>(opt =>
	opt.UseInMemoryDatabase("TestRadioDb"));
builder.Services.AddHttpClient();

// Register Repositories
builder.Services.AddScoped<IRadioProgramRepository, RadioProgramRepository>();
builder.Services.AddScoped<IRepository<Host>, Repository<Host>>();
builder.Services.AddScoped<IRepository<Music>, Repository<Music>>();
builder.Services.AddScoped<IRepository<ProgramDetails>, Repository<ProgramDetails>>();

// Register Services
builder.Services.AddScoped<IRadioProgramService, RadioProgramService>();
builder.Services.AddScoped<IGenericService<Host>, GenericService<Host>>();
builder.Services.AddScoped<IGenericService<ProgramDetails>, GenericService<ProgramDetails>>();
builder.Services.AddScoped<IGenericService<Music>, GenericService<Music>>();

builder.Services.ConfigureHttpJsonOptions(options =>
{
	options.SerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
	using (var scope = app.Services.CreateScope())
	{
		var services = scope.ServiceProvider;
		await app.SeedTestData(services);
	}
}

app.UseHttpsRedirection();

app.MapGets();
app.MapPuts();
app.MapPosts();
app.MapDeletes();

app.Run();