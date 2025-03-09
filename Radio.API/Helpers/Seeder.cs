using Bogus;
using Radio.Application.Services.Interfaces;
using Radio.Domain.Models;
using Host = Radio.Domain.Models.Host;

namespace Radio.API.Helpers;

public static class Seeder
{
	public static async Task SeedTestData(this WebApplication app, IServiceProvider serviceProvider)
	{
		var musicService = serviceProvider.GetRequiredService<IGenericService<Music>>();
		var hostService = serviceProvider.GetRequiredService<IGenericService<Host>>();
		var programDetailsService = serviceProvider.GetRequiredService<IGenericService<ProgramDetails>>();
		var radioProgramService = serviceProvider.GetRequiredService<IRadioProgramService>();

		Random random = new Random();
		int itemCount = random.Next(50,135);

		var musicFaker = new Faker<Music>()
			.RuleFor(i => i.Title, f => f.Commerce.ProductName())
			.RuleFor(i => i.Artist, f => f.Name.FullName());

		var musics = musicFaker.Generate(itemCount);
		// Add musics to database
		foreach (var music in musics)
		{
			await musicService.AddAsync(music);
		}

		itemCount = random.Next(5, 15);

		var hostFaker = new Faker<Host>()
			.RuleFor(i => i.Name, f => f.Name.FullName())
			.RuleFor(i => i.Email, f => f.Internet.Email());

		var hosts = hostFaker.Generate(itemCount);
		// Add hosts to database
		foreach (var host in hosts)
		{
			await hostService.AddAsync(host);
		}

		itemCount = random.Next(15, 35);

		var programDetailsFaker = new Faker<ProgramDetails>()
			.RuleFor(i => i.Description, f => f.Lorem.Sentence())
			.RuleFor(i => i.Duration, f => new TimeSpan(f.Random.Int(0,3), f.Random.Int(0,60), f.Random.Int(0,59)));

		var programDetails = programDetailsFaker.Generate(itemCount);
		// Add hosts to database
		foreach (var programDetail in programDetails)
		{
			await programDetailsService.AddAsync(programDetail);
		}

		itemCount = random.Next(5, 20);
		// Faker dla RadioProgram
		var radioProgramFaker = new Faker<RadioProgram>()
			.RuleFor(r => r.Name, f => f.Company.CatchPhrase())
			.RuleFor(r => r.StartTime, f => DateTime.Now + new TimeSpan(f.Random.Int(0,999)))
			.RuleFor(r => r.Host, f => f.PickRandom(hosts))
			.RuleFor(r => r.Musics, f => f.PickRandom(musics, f.Random.Int(1, 5)).ToList());

		var radioPrograms = radioProgramFaker.Generate(itemCount);
		// Add radio programs to database
		foreach (var radioProgram in radioPrograms)
		{
			radioProgram.ProgramDetails = programDetails[random.Next(0, programDetails.Count)];
			radioProgram.ProgramDetailsId = radioProgram.ProgramDetails!.Id;
			radioProgram.HostId = radioProgram.Host!.Id;
			await radioProgramService.AddProgramAsync(radioProgram.Name, radioProgram.StartTime, radioProgram.HostId, radioProgram.ProgramDetailsId, radioProgram.Musics!.Select(m => m.Id).ToArray());
			programDetails.Remove(radioProgram.ProgramDetails);
		}
	}
}