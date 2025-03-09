using Microsoft.Extensions.Logging;
using Radio.Application.Services.Interfaces;
using Radio.Infrastructure.Repositories.Interfaces;

namespace Radio.Application.Services;

public class GenericService<T> : IGenericService<T> where T : class
{
	private readonly IRepository<T> _repository;
	private readonly ILogger<GenericService<T>> _logger;

	public GenericService(IRepository<T> repository, ILogger<GenericService<T>> logger)
	{
		_repository = repository;
		_logger = logger;
	}

	public async Task<T> AddAsync(T entity)
	{
		try
		{
			await _repository.AddAsync(entity);
			_logger.LogInformation($"{typeof(T).Name} added successfully");
			return entity;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"An error occurred while while adding {typeof(T).Name}");
			throw new Exception($"Sorry, unable to add {typeof(T).Name}.");
		}
	}

	public async Task DeleteAsync(int id)
	{
		try
		{
			await _repository.DeleteAsync(id);
			_logger.LogInformation($"{typeof(T).Name} deleted successfully");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"An error occurred while while deleting {typeof(T).Name}");
			throw new Exception($"Sorry, unable to delete {typeof(T).Name}.");
		}
	}

	public async Task<IEnumerable<T>> GetAllAsync()
	{
		try
		{
			return await _repository.GetAllAsync();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"An error occurred while while geting all {typeof(T).Name}");
			throw new Exception($"Sorry, unable to get {typeof(T).Name}.");
		}
	}

	public async Task<T?> GetByIdAsync(int id)
	{
		try
		{
			return await _repository.GetByIdAsync(id);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"An error occurred while while geting {typeof(T).Name} with id {id}");
			throw new Exception($"Sorry, unable to get {typeof(T).Name}.");
		}
	}

	public async Task UpdateAsync(T entity)
	{
		try
		{
			await _repository.UpdateAsync(entity);
			_logger.LogInformation($"{typeof(T).Name} updated successfully");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"An error occurred while while updating {typeof(T).Name}");
			throw new Exception($"Sorry, unable to update {typeof(T).Name}.");
		}
	}
}