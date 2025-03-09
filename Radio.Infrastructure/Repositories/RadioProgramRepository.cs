using Microsoft.EntityFrameworkCore;
using Radio.Domain.Models;
using Radio.Infrastructure.Presistence;
using Radio.Infrastructure.Repositories.Interfaces;

namespace Radio.Infrastructure.Repositories;
public class RadioProgramRepository : IRadioProgramRepository
{
	private readonly RadioDbContext _context;

	public RadioProgramRepository(RadioDbContext context)
	{
		_context = context;
	}

	public async Task AddAsync(RadioProgram entity)
	{
		await _context.RadioPrograms.AddAsync(entity);
		await _context.SaveChangesAsync();
	}

	public async Task DeleteAsync(int id)
	{
		var entity = await GetByIdAsync(id);
		if (entity != null)
		{
			_context.RadioPrograms.Remove(entity);
			await _context.SaveChangesAsync();
		}
	}

	public async Task<IEnumerable<RadioProgram>> GetAllAsync()
	{
		return await _context.RadioPrograms
			.Include(rp => rp.Host)
			.Include(rp => rp.ProgramDetails)
			.Include(rp => rp.Musics)
			.ToListAsync();
	}

	public async Task<RadioProgram?> GetByIdAsync(int id)
	{
		return await _context.RadioPrograms
			.Include(rp => rp.Host)
			.Include(rp => rp.ProgramDetails)
			.Include(rp => rp.Musics)
			.FirstOrDefaultAsync(rp => rp.Id == id);
	}

	public async Task<IEnumerable<RadioProgram>> GetProgramsByTimeAsync(DateTime time)
	{
		return await _context.RadioPrograms
			.Where(rp => time >= rp.StartTime && time <= rp.EndTime)
			.Include(rp => rp.Host)
			.Include(rp => rp.ProgramDetails)
			.ToListAsync();
	}

	public async Task UpdateAsync(RadioProgram entity)
	{
		_context.RadioPrograms.Update(entity);
		await _context.SaveChangesAsync();
	}
}
