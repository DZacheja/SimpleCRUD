using Microsoft.AspNetCore.Mvc;
using Radio.Application.Services.Interfaces;
using Radio.Domain.Models;
using Host = Radio.Domain.Models.Host;

namespace RadioAPI.Endpoints;

public static class DeleteEnpointsExtension
{
	/// <summary>
	/// Add delete endpoints to minimal api
	/// </summary>
	public static void MapDeletes(this WebApplication app)
	{
		app.MapDelete("/api/musics/{id:int}", async (int id, [FromServices] IGenericService<Music> musicService) =>
		{
			try
			{
				await musicService.DeleteAsync(id);
				return Results.NoContent();
			}
			catch (Exception ex)
			{
				return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
			}
		})
		.WithDisplayName("Delete music")
		.WithOpenApi(operation =>
		{
			operation.Description = "Delete music by given id";
			operation.Summary = "Remove Music from database";
			return operation;
		});

		app.MapDelete("/api/hosts/{id:int}", async (int id, [FromServices] IGenericService<Host> hostService) =>
		{
			try
			{
				await hostService.DeleteAsync(id);
				return Results.NoContent();
			}
			catch (Exception ex)
			{
				return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
			}
		})
		.WithDisplayName("Delete host")
		.WithOpenApi(operation =>
		{
			operation.Description = "Delete host by given id";
			operation.Summary = "Remove Host from database";
			return operation;
		});

		app.MapDelete("/api/programdetails/{id:int}", async (int id, [FromServices] IGenericService<ProgramDetails> programDetailsService) =>
		{
			try
			{
				await programDetailsService.DeleteAsync(id);
				return Results.NoContent();
			}
			catch (Exception ex)
			{
				return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
			}
		})
		.WithDisplayName("Delete program details")
		.WithOpenApi(operation =>
		{
			operation.Description = "Delete program details by given id";
			operation.Summary = "Remove program details from database";
			return operation;
		});

		app.MapDelete("/api/radioprogram/{id:int}", async (int id, [FromServices] IRadioProgramService radioProgramService) =>
		{
			try
			{
				await radioProgramService.DeleteProgramAsync(id);
				return Results.NoContent();
			}
			catch (Exception ex)
			{
				return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
			}
		})
		.WithDisplayName("Delete radio program")
		.WithOpenApi(operation =>
		{
			operation.Description = "Delete radio program by given id";
			operation.Summary = "Remove radio program from database";
			return operation;
		});
	}
}