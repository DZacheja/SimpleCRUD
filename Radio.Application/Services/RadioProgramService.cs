using Microsoft.Extensions.Logging;
using Radio.Application.Services.Interfaces;
using Radio.Domain.Models;
using Radio.Infrastructure.Repositories.Interfaces;

namespace Radio.Application.Services;

public class RadioProgramService : IRadioProgramService
{
	private readonly IRadioProgramRepository _radioProgramRepository;
	private readonly IGenericService<ProgramDetails> _programDetailsService;
	private readonly IGenericService<Host> _hostService;
	private readonly IGenericService<Music> _musicService;
	private readonly ILogger<RadioProgramService> _logger;

	public RadioProgramService(IRadioProgramRepository radioProgramRepository,
		IGenericService<ProgramDetails> programDetailsService,
		IGenericService<Host> hostService,
		IGenericService<Music> musicService,
		ILogger<RadioProgramService> logger)
	{
		_radioProgramRepository = radioProgramRepository;
		_programDetailsService = programDetailsService;
		_hostService = hostService;
		_musicService = musicService;
		_logger = logger;
	}

	public async Task<IEnumerable<RadioProgram>> GetAllProgramsAsync()
	{
		try
		{
			var results = await _radioProgramRepository.GetAllAsync();
			//Clear references because of circular reference - it can be fixed using DTOs
			foreach (var item in results)
			{
				item.Musics!.ForEach(x => x.RadioPrograms.Clear());
				item.ProgramDetails!.RadioProgram = new RadioProgram();
				item.Host!.Programs = new List<RadioProgram>();
			}
			return results;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "An error occurred while getting all programs");
			throw new Exception("Sorry, unable to load Programs.");
		}
	}

	public async Task<RadioProgram?> GetProgramByIdAsync(int id)
	{
		try
		{
			return await _radioProgramRepository.GetByIdAsync(id);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"An error occurred while getting program with id:{id}");
			throw new Exception("Sorry, unable to load Program.");
		}
	}

	public async Task<IEnumerable<RadioProgram>> GetProgramsByTimeAsync(DateTime time)
	{
		try
		{
			return await _radioProgramRepository.GetProgramsByTimeAsync(time);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"An error occurred while getting program by time:{time.ToString("dd-MM-yyyy hh:mm")}");
			throw new Exception("Sorry, unable to load Program.");
		}
	}

	public async Task<RadioProgram> AddProgramAsync(string Name, DateTime StartTime, int HostId, int ProgramDetailsId, int[] MusicIdList)
	{
		try
		{
			var host = await _hostService.GetByIdAsync(HostId);
			var details = await _programDetailsService.GetByIdAsync(ProgramDetailsId);
			var musicList = new List<Music>();

			foreach (var musicId in MusicIdList)
			{
				var music = await _musicService.GetByIdAsync(musicId);
				if (music != null)
				{
					musicList.Add(music);
				}
			}
			if (host == null || details == null) throw new Exception("Invalid Host or ProgramDetails ID");

			RadioProgram radioProgram = new RadioProgram
			{
				Name = Name,
				StartTime = StartTime,
				HostId = HostId,
				Host = host,
				ProgramDetailsId = ProgramDetailsId,
				ProgramDetails = details,
				Musics = musicList,
			};

			radioProgram.EndTime = radioProgram.StartTime + details.Duration;

			await _radioProgramRepository.AddAsync(radioProgram);
			_logger.LogInformation($"Program with id:{radioProgram.Id} added successfully");
			return radioProgram;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "An error occurred while adding program");
			throw new Exception("Sorry, unable to add Program.");
		}
	}

	public async Task UpdateProgramAsync(int id, string Name, DateTime StartTime, int HostId, int ProgramDetailsId)
	{
		try
		{
			var radioProgram = await _radioProgramRepository.GetByIdAsync(id);
			if (radioProgram == null) throw new Exception("Invalid Program ID");
			radioProgram.Name = Name;
			radioProgram.StartTime = StartTime;
			if (radioProgram.HostId != HostId)
			{
				var host = await _hostService.GetByIdAsync(HostId);
				if (host == null) throw new Exception("Invalid Host ID");
				radioProgram.HostId = HostId;
				radioProgram.Host = host;
			}
			if (radioProgram.ProgramDetailsId != ProgramDetailsId)
			{
				var details = await _programDetailsService.GetByIdAsync(ProgramDetailsId);
				if (details == null) throw new Exception("Invalid ProgramDetails ID");
				radioProgram.ProgramDetailsId = ProgramDetailsId;
				radioProgram.ProgramDetails = details;
				radioProgram.EndTime = radioProgram.StartTime + details.Duration;
			}
			await _radioProgramRepository.UpdateAsync(radioProgram);
			_logger.LogInformation($"Program with id:{radioProgram.Id} updated successfully");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "An error occurred while updating program");
			throw new Exception("Sorry, unable to update Program.");
		}
	}

	public async Task DeleteProgramAsync(int id)
	{
		try
		{
			await _radioProgramRepository.DeleteAsync(id);
			_logger.LogInformation($"Program with id:{id} deleted successfully");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "An error occurred while deleting program");
			throw new Exception("Sorry, unable to delete Program.");
		}
	}

	public async Task AddMusicToProgramAsync(int radioId, int musicId)
	{
		try
		{
			var radioProgram = await _radioProgramRepository.GetByIdAsync(radioId);
			if (radioProgram == null) throw new Exception("Invalid Program ID");
			if (radioProgram.Musics!.Any(m => m.Id == musicId)) throw new Exception("Music already added to Program");
			var music = await _musicService.GetByIdAsync(musicId);
			if (music == null) throw new Exception("Invalid Music ID");
			radioProgram.Musics!.Add(music);
			await _radioProgramRepository.UpdateAsync(radioProgram);
			_logger.LogInformation($"Program with id:{radioProgram.Id} updated successfully");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "An error occurred while updating program");
			throw new Exception("Sorry, unable to update Program.");
		}
	}
}