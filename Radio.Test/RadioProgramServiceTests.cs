using Microsoft.Extensions.Logging;
using Moq;
using Radio.Application.Services;
using Radio.Application.Services.Interfaces;
using Radio.Domain.Models;
using Radio.Infrastructure.Repositories.Interfaces;

namespace Radio.Test
{
	public class RadioProgramServiceTests
	{
		private readonly Mock<IRadioProgramRepository> _radioProgramRepositoryMock;
		private readonly Mock<IGenericService<ProgramDetails>> _programDetailsServiceMock;
		private readonly Mock<IGenericService<Host>> _hostServiceMock;
		private readonly Mock<IGenericService<Music>> _musicServiceMock;
		private readonly Mock<ILogger<RadioProgramService>> _loggerMock;
		private readonly RadioProgramService _radioProgramService;

		public RadioProgramServiceTests()
		{
			_radioProgramRepositoryMock = new Mock<IRadioProgramRepository>();
			_programDetailsServiceMock = new Mock<IGenericService<ProgramDetails>>();
			_hostServiceMock = new Mock<IGenericService<Host>>();
			_musicServiceMock = new Mock<IGenericService<Music>>();
			_loggerMock = new Mock<ILogger<RadioProgramService>>();
			_radioProgramService = new RadioProgramService(
				_radioProgramRepositoryMock.Object,
				_programDetailsServiceMock.Object,
				_hostServiceMock.Object,
				_musicServiceMock.Object,
				_loggerMock.Object);
		}

		[Fact]
		public async Task GetAllProgramsAsync_ShouldReturnAllPrograms()
		{
			// Arrange
			var programs = new List<RadioProgram> { new RadioProgram { Id = 1, Name = "Program1" } };
			_radioProgramRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(programs);

			// Act
			var result = await _radioProgramService.GetAllProgramsAsync();

			// Assert
			Assert.Equal(programs, result);
		}

		[Fact]
		public async Task GetProgramByIdAsync_ShouldReturnProgram_WhenProgramExists()
		{
			// Arrange
			var program = new RadioProgram { Id = 1, Name = "Program1" };
			_radioProgramRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(program);

			// Act
			var result = await _radioProgramService.GetProgramByIdAsync(1);

			// Assert
			Assert.Equal(program, result);
		}

		[Fact]
		public async Task GetProgramsByTimeAsync_ShouldReturnPrograms_WhenProgramsExist()
		{
			// Arrange
			var time = DateTime.Now;
			var programs = new List<RadioProgram> { new RadioProgram { Id = 1, Name = "Program1" } };
			_radioProgramRepositoryMock.Setup(repo => repo.GetProgramsByTimeAsync(time)).ReturnsAsync(programs);

			// Act
			var result = await _radioProgramService.GetProgramsByTimeAsync(time);

			// Assert
			Assert.Equal(programs, result);
		}

		[Fact]
		public async Task AddProgramAsync_ShouldAddProgram_WhenValidData()
		{
			// Arrange
			var host = new Host { Id = 1, Name = "Host1" };
			var details = new ProgramDetails { Id = 1, Description = "Details1", Duration = TimeSpan.FromHours(1) };
			var music = new Music { Id = 1, Title = "Music1" };
			_hostServiceMock.Setup(service => service.GetByIdAsync(1)).ReturnsAsync(host);
			_programDetailsServiceMock.Setup(service => service.GetByIdAsync(1)).ReturnsAsync(details);
			_musicServiceMock.Setup(service => service.GetByIdAsync(1)).ReturnsAsync(music);
			_radioProgramRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<RadioProgram>())).Returns(Task.CompletedTask);

			// Act
			var result = await _radioProgramService.AddProgramAsync("Program1", DateTime.Now, 1, 1, new[] { 1 });

			// Assert
			Assert.Equal("Program1", result.Name);
			Assert.Equal(1, result.HostId);
			Assert.Equal(1, result.ProgramDetailsId);
			Assert.Single(result.Musics);
		}

		[Fact]
		public async Task UpdateProgramAsync_ShouldUpdateProgram_WhenValidData()
		{
			// Arrange
			var program = new RadioProgram { Id = 1, Name = "Program1", HostId = 1, ProgramDetailsId = 1 };
			var host = new Host { Id = 2, Name = "Host2" };
			var details = new ProgramDetails { Id = 2, Description = "Details2", Duration = TimeSpan.FromHours(2) };
			_radioProgramRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(program);
			_hostServiceMock.Setup(service => service.GetByIdAsync(2)).ReturnsAsync(host);
			_programDetailsServiceMock.Setup(service => service.GetByIdAsync(2)).ReturnsAsync(details);
			_radioProgramRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<RadioProgram>())).Returns(Task.CompletedTask);

			// Act
			await _radioProgramService.UpdateProgramAsync(1, "UpdatedProgram", DateTime.Now, 2, 2);

			// Assert
			Assert.Equal("UpdatedProgram", program.Name);
			Assert.Equal(2, program.HostId);
			Assert.Equal(2, program.ProgramDetailsId);
		}

		[Fact]
		public async Task DeleteProgramAsync_ShouldDeleteProgram_WhenValidId()
		{
			// Arrange
			_radioProgramRepositoryMock.Setup(repo => repo.DeleteAsync(1)).Returns(Task.CompletedTask);

			// Act
			await _radioProgramService.DeleteProgramAsync(1);

			// Assert
			_radioProgramRepositoryMock.Verify(repo => repo.DeleteAsync(1), Times.Once);
		}

		[Fact]
		public async Task AddMusicToProgramAsync_ShouldAddMusic_WhenValidData()
		{
			// Arrange
			var program = new RadioProgram { Id = 1, Name = "Program1", Musics = new List<Music>() };
			var music = new Music { Id = 1, Title = "Music1" };
			_radioProgramRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(program);
			_musicServiceMock.Setup(service => service.GetByIdAsync(1)).ReturnsAsync(music);
			_radioProgramRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<RadioProgram>())).Returns(Task.CompletedTask);

			// Act
			await _radioProgramService.AddMusicToProgramAsync(1, 1);

			// Assert
			Assert.Single(program.Musics);
			Assert.Equal(music, program.Musics[0]);
		}
	}
}
