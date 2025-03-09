using Microsoft.EntityFrameworkCore;
using Radio.Domain.Models;
using Radio.Infrastructure.Presistence;
using Radio.Infrastructure.Repositories;

namespace Radio.Test
{
	public class RadioProgramRepositoryTests
	{
		private readonly RadioDbContext _dbContext;
		private readonly RadioProgramRepository _radioProgramRepository;

		public RadioProgramRepositoryTests()
		{
			var options = new DbContextOptionsBuilder<RadioDbContext>()
				.UseInMemoryDatabase("TestDb")
				.Options;

			_dbContext = new RadioDbContext(options);
			_dbContext.Database.EnsureCreated();
			_radioProgramRepository = new RadioProgramRepository(_dbContext);
		}
		private async Task ClearRadioProgramsTable()
		{
			_dbContext.RadioPrograms.RemoveRange(_dbContext.RadioPrograms);
			await _dbContext.SaveChangesAsync();
		}

		[Fact]
		public async Task AddAsync_ShouldAddProgram()
		{
			await ClearRadioProgramsTable();
			// Arrange
			var program = new RadioProgram {Name = "Program1" };

			// Act
			await _radioProgramRepository.AddAsync(program);
			var result = await _dbContext.RadioPrograms.FirstOrDefaultAsync(rp => rp.Name == "Program1");

			// Assert
			Assert.NotNull(result);
			Assert.Equal("Program1", result.Name);
		}

		[Fact]
		public async Task DeleteAsync_ShouldDeleteProgram_WhenProgramExists()
		{
			await ClearRadioProgramsTable();
			// Arrange
			var host = new Host { Name = "Host1" };
			var programDetails = new ProgramDetails { Description = "Description1", Duration = new TimeSpan(1, 0, 0) };	
			await _dbContext.Hosts.AddAsync(host);
			await _dbContext.ProgramDetails.AddAsync(programDetails);
			var program = new RadioProgram {Name = "Program2", Host = host, ProgramDetails = programDetails };
			await _dbContext.RadioPrograms.AddAsync(program);
			await _dbContext.SaveChangesAsync();

			// Act
			await _radioProgramRepository.DeleteAsync(program.Id);
			var result = await _dbContext.RadioPrograms.FirstOrDefaultAsync(rp => rp.Id == program.Id);

			// Assert
			Assert.Null(result);
		}

		[Fact]
		public async Task GetAllAsync_ShouldReturnAllPrograms()
		{
			
			await ClearRadioProgramsTable();
			// Arrange
			var host1 = new Host { Name = "Host1" };
			var host2 = new Host { Name = "Host2" };
			var programDetails1 = new ProgramDetails { Description = "Description1", Duration = new TimeSpan(1, 0, 0) };
			var programDetails2 = new ProgramDetails { Description = "Description2", Duration = new TimeSpan(1, 0, 0) };
			await _dbContext.Hosts.AddRangeAsync(host1,host2);
			await _dbContext.ProgramDetails.AddRangeAsync(programDetails1, programDetails2);
			await _dbContext.SaveChangesAsync();
			var program3 = new RadioProgram {Name = "Program3", Host = host1, ProgramDetails = programDetails1};
			var program4 = new RadioProgram {Name = "Program4", Host = host2, ProgramDetails = programDetails2};
			await _dbContext.RadioPrograms.AddRangeAsync(program3,program4);
			await _dbContext.SaveChangesAsync();

			// Act
			var result = await _radioProgramRepository.GetAllAsync();
			var result2 = await _dbContext.RadioPrograms.ToListAsync();
			// Assert
			Assert.Equal(2, result.Count());
			Assert.Contains(result, p => p.Name == "Program3");
			Assert.Contains(result, p => p.Name == "Program4");
		}

		[Fact]
		public async Task GetByIdAsync_ShouldReturnProgram_WhenProgramExists()
		{
			await ClearRadioProgramsTable();
			// Arrange
			var host = new Host { Name = "Host" };
			var programDetails = new ProgramDetails { Description = "Description", Duration = new TimeSpan(1, 0, 0) };
			await _dbContext.Hosts.AddAsync(host);
			await _dbContext.ProgramDetails.AddAsync(programDetails);
			var program = new RadioProgram {Name = "Program33", Host = host, ProgramDetails = programDetails};
			await _dbContext.RadioPrograms.AddAsync(program);
			await _dbContext.SaveChangesAsync();

			// Act
			var result = await _radioProgramRepository.GetByIdAsync(program.Id);

			// Assert
			Assert.NotNull(result);
			Assert.Equal("Program33", result.Name);
		}

		[Fact]
		public async Task GetProgramsByTimeAsync_ShouldReturnPrograms_WhenProgramsExist()
		{
			await ClearRadioProgramsTable();
			// Arrange
			var time = DateTime.Now;
			var host1 = new Host { Name = "Host1" };
			var host2 = new Host { Name = "Host2" };
			var programDetails1 = new ProgramDetails { Description = "Description1", Duration = new TimeSpan(1, 0, 0) };
			var programDetails2 = new ProgramDetails { Description = "Description2", Duration = new TimeSpan(1, 0, 0) };
			await _dbContext.Hosts.AddRangeAsync(host1, host2);
			await _dbContext.ProgramDetails.AddRangeAsync(programDetails1, programDetails2);
			await _dbContext.SaveChangesAsync();
			var programs = new List<RadioProgram>
			{
				new RadioProgram { Id = 1, Name = "Program1", StartTime = time.AddHours(-1), EndTime = time.AddHours(1), Host = host1, ProgramDetails = programDetails1 },
				new RadioProgram { Id = 2, Name = "Program2", StartTime = time.AddHours(-2), EndTime = time.AddHours(-1), Host = host2, ProgramDetails = programDetails2 }
			};
			await _dbContext.RadioPrograms.AddRangeAsync(programs);
			await _dbContext.SaveChangesAsync();

			// Act
			var result = await _radioProgramRepository.GetProgramsByTimeAsync(time);

			// Assert
			Assert.Single(result);
			Assert.Equal("Program1", result.First().Name);
		}

		[Fact]
		public async Task UpdateAsync_ShouldUpdateProgram()
		{
			await ClearRadioProgramsTable();
			// Arrange
			var host = new Host { Name = "Host" };
			var programDetails = new ProgramDetails { Description = "Description", Duration = new TimeSpan(1, 0, 0) };
			await _dbContext.Hosts.AddAsync(host);
			var program = new RadioProgram { Id = 1, Name = "Program1", Host = host, ProgramDetails = programDetails };
			await _dbContext.RadioPrograms.AddAsync(program);
			await _dbContext.SaveChangesAsync();

			// Act
			program.Name = "UpdatedProgram";
			await _radioProgramRepository.UpdateAsync(program);
			var result = await _dbContext.RadioPrograms.FirstOrDefaultAsync(rp => rp.Id == program.Id);

			// Assert
			Assert.NotNull(result);
			Assert.Equal("UpdatedProgram", result.Name);
		}
	}
}
