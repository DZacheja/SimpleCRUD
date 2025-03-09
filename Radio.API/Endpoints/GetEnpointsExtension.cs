using Microsoft.AspNetCore.Mvc;
using Radio.Application.Services.Interfaces;
using Radio.Domain.Models;
using Host = Radio.Domain.Models.Host;

namespace RadioAPI.Endpoints;

public static class GetEnpointsExtension
{
	/// <summary>
	/// Add get endpoints to minimal api
	/// </summary>
	public static void MapGets(this WebApplication app)
	{
		app.MapGet("/api/musics", async ([FromServices] IGenericService<Music> musicService) =>
		{
			try
			{
				var music = await musicService.GetAllAsync();
				return Results.Ok(music);
			}
			catch (Exception ex)
			{
				return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
			}
		})
		.WithDisplayName("List of musics")
		.WithOpenApi(operation =>
		{
			operation.Description = "Get list of existing musics";
			operation.Summary = "Existing musics";
			return operation;
		})
		.Produces<List<Music>>(StatusCodes.Status200OK);

		app.MapGet("/api/musics/{id:int}", async (int id, [FromServices] IGenericService<Music> musicService) =>
		{
			try
			{
				var music = await musicService.GetByIdAsync(id);
				return Results.Ok(music);
			}
			catch (Exception ex)
			{
				return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
			}
		})
		.WithDisplayName("Single music record")
		.WithOpenApi(operation =>
		{
			operation.Description = "Get music by id";
			operation.Summary = "Music";
			return operation;
		})
		.Produces<Music>(StatusCodes.Status200OK);

		app.MapGet("/api/hosts", async ([FromServices] IGenericService<Radio.Domain.Models.Host> hostService) =>
		{
			try
			{
				var host = await hostService.GetAllAsync();
				return Results.Ok(host);
			}
			catch (Exception ex)
			{
				return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
			}
		})
		.WithDisplayName("List of musics")
		.WithOpenApi(operation =>
		{
			operation.Description = "Get list of existing musics";
			operation.Summary = "Existing musics";
			return operation;
		})
		.Produces<List<Music>>(StatusCodes.Status200OK);

		app.MapGet("/api/hosts/{id:int}", async (int id, [FromServices] IGenericService<Host> hostService) =>
		{
			try
			{
				var music = await hostService.GetByIdAsync(id);
				return Results.Ok(music);
			}
			catch (Exception ex)
			{
				return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
			}
		})
		.WithDisplayName("Single host record")
		.WithOpenApi(operation =>
		{
			operation.Description = "Get host by id";
			operation.Summary = "Host";
			return operation;
		})
		.Produces<Host>(StatusCodes.Status200OK);

		app.MapGet("/api/programdetails", async ([FromServices] IGenericService<ProgramDetails> programDetailsService) =>
		{
			try
			{
				var program = await programDetailsService.GetAllAsync();
				return Results.Ok(program);
			}
			catch (Exception ex)
			{
				return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
			}
		})
		.WithDisplayName("List of program details")
		.WithOpenApi(operation =>
		{
			operation.Description = "Get list of existing program details";
			operation.Summary = "Existing program details";
			return operation;
		})
		.Produces<List<Music>>(StatusCodes.Status200OK);

		app.MapGet("/api/radioprograms", async ([FromServices] IRadioProgramService radioProgramService) =>
		{
			try
			{
				var program = await radioProgramService.GetAllProgramsAsync();
				return Results.Ok(program);
			}
			catch (Exception ex)
			{
				return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
			}
		})
		.WithDisplayName("List of radio programs")
		.WithOpenApi(operation =>
		{
			operation.Description = "Get list of existing radio programs";
			operation.Summary = "Existing programs";
			return operation;
		})
		.Produces<List<RadioProgram>>(StatusCodes.Status200OK);

		app.MapGet("/api/radioprograms/{id:int}", async (int id, [FromServices] IRadioProgramService radioProgramService) =>
		{
			try
			{
				var radio = await radioProgramService.GetProgramByIdAsync(id);
				return Results.Ok(radio);
			}
			catch (Exception ex)
			{
				return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
			}
		})
		.WithDisplayName("Single radio program")
		.WithOpenApi(operation =>
		{
			operation.Description = "Get radio program by id";
			operation.Summary = "Radio Program";
			return operation;
		})
		.Produces<RadioProgram>(StatusCodes.Status200OK);
	}
}