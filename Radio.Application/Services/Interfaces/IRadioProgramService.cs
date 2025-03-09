using Radio.Domain.Models;

namespace Radio.Application.Services.Interfaces;

public interface IRadioProgramService
{
	Task<IEnumerable<RadioProgram>> GetAllProgramsAsync();

	Task<RadioProgram?> GetProgramByIdAsync(int id);

	Task<IEnumerable<RadioProgram>> GetProgramsByTimeAsync(DateTime time);

	Task<RadioProgram> AddProgramAsync(string Name, DateTime StartTime, int HostId, int ProgramDetailsId, int[] MusicIdList);

	Task UpdateProgramAsync(int id, string Name, DateTime StartTime, int HostId, int ProgramDetailsId);

	Task AddMusicToProgramAsync(int radioId, int musicId);

	Task DeleteProgramAsync(int id);
}