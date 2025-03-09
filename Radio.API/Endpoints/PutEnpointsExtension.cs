using Microsoft.AspNetCore.Mvc;
using Radio.API.DTO;
using Radio.Application.Services.Interfaces;
using Radio.Domain.Models;

namespace RadioAPI.Endpoints;

public static class PutEnpointsExtension
{
	/// <summary>
	/// Add put endpoints to minimal api
	/// </summary>
	public static void MapPuts(this WebApplication app)
	{
		app.MapPut("/api/radioprogram", async ([FromBody] UpdateRadioProgramRequest request, [FromServices] IRadioProgramService radioProgramService) =>
		{
			try
			{
				await radioProgramService.UpdateProgramAsync(request.id, request.Name, request.StartTime, request.HostId, request.ProgramDetailsId);
				return Results.Ok($"Program with id {request.id} updated successfully");
			}
			catch (Exception ex)
			{
				return Results.Problem(detail: ex.Message);
			}
		})
		.WithDisplayName("Update radio program")
		.WithOpenApi(operation =>
		{
			operation.Description = "Update radio program.";
			operation.Summary = "Update radio program";
			return operation;
		})
		.Produces<RadioProgram>(StatusCodes.Status201Created);

		app.MapPut("/api/radioprogram/addmusic", async ([FromBody] AddMusicToRadioProgramRequest request, [FromServices] IRadioProgramService radioProgramService) =>
		{
			try
			{
				await radioProgramService.AddMusicToProgramAsync(request.RadioProgramId, request.MusicId);
				return Results.Ok($"Program with id {request.RadioProgramId} updated successfully");
			}
			catch (Exception ex)
			{
				return Results.Problem(detail: ex.Message);
			}
		})
		.WithDisplayName("Update radio program by adding new music")
		.WithOpenApi(operation =>
		{
			operation.Description = "Update radio program by adding new music.";
			operation.Summary = "Update radio program by adding new music";
			return operation;
		})
		.Produces<RadioProgram>(StatusCodes.Status201Created);
	}
}