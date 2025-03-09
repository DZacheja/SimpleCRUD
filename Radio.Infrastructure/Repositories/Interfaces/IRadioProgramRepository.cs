using Radio.Domain.Models;

namespace Radio.Infrastructure.Repositories.Interfaces;

public interface IRadioProgramRepository
{
	Task<IEnumerable<RadioProgram>> GetProgramsByTimeAsync(DateTime time);

	Task<IEnumerable<RadioProgram>> GetAllAsync();

	Task<RadioProgram?> GetByIdAsync(int id);

	Task AddAsync(RadioProgram entity);

	Task UpdateAsync(RadioProgram entity);

	Task DeleteAsync(int id);
}