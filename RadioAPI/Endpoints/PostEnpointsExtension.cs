using Microsoft.AspNetCore.Mvc;
using Radio.API.DTO;
using Radio.Application.Services.Interfaces;
using Radio.Domain.Models;
using Host = Radio.Domain.Models.Host;

namespace RadioAPI.Endpoints;

public static class PostEnpointsExtension
{
	/// <summary>
	/// Add post endpoints to minimal api
	/// </summary>
	public static void MapPosts(this WebApplication app)
	{
		app.MapPost("/api/musics", async ([FromBody] CreateMusicRequest request, [FromServices] IGenericService<Music> musicService) =>
		{
			try
			{
				var newMusic = await musicService.AddAsync(new Music{Artist = request.Artist, Title = request.Title});
				return Results.Created($"/api/musics/{newMusic.Id}", newMusic);
			}
			catch (Exception ex)
			{
				return Results.Problem(detail: ex.Message);
			}
		})
		.WithDisplayName("Add New Music")
		.WithOpenApi(operation =>
		{
			operation.Description = "Creates a new music.";
			operation.Summary = "Add Music";
			return operation;
		})
		.Produces<Music>(StatusCodes.Status201Created);

		app.MapPost("/api/hosts", async ([FromBody] CreateHostRequest request, [FromServices] IGenericService<Host> hostService) =>
		{
			try
			{
				var newHost = await hostService.AddAsync(new Host{Name = request.Name, Email = request.Email});
				return Results.Created($"/api/hosts/{newHost.Id}", newHost);
			}
			catch (Exception ex)
			{
				return Results.Problem(detail: ex.Message);
			}
		})
		.WithDisplayName("Add New Host")
		.WithOpenApi(operation =>
		{
			operation.Description = "Creates a new host.";
			operation.Summary = "Add Host";
			return operation;
		})
		.Produces<Music>(StatusCodes.Status201Created);

		app.MapPost("/api/programdetails", async ([FromBody] CreateProgramDetailsRequest request, [FromServices] IGenericService<ProgramDetails> programDetailsService) =>
		{
			try
			{
				var newProgramDetails = await programDetailsService.AddAsync(new ProgramDetails{Description = request.Description, Duration = request.Duration});
				return Results.Created($"/api/programdetails/{newProgramDetails.Id}", newProgramDetails);
			}
			catch (Exception ex)
			{
				return Results.Problem(detail: ex.Message);
			}
		})
		.WithDisplayName("Add New Program Details")
		.WithOpenApi(operation =>
		{
			operation.Description = "Creates a new program details.";
			operation.Summary = "Add program details";
			return operation;
		})
		.Produces<ProgramDetails>(StatusCodes.Status201Created);

		app.MapPost("/api/radioprogram", async (
			[FromBody] CreateRadioProgramRequest request,
			[FromServices] IRadioProgramService radioProgramService) =>
		{
			try
			{
				var newRadioProgram = await radioProgramService.AddProgramAsync(request.Name, request.StartTime, request.HostId, request.ProgramDetailsId, request.MusicIdList);
				return Results.Created($"/api/radioprogram/{newRadioProgram.Id}", newRadioProgram);
			}
			catch (Exception ex)
			{
				return Results.Problem(detail: ex.Message);
			}
		})
		.WithDisplayName("Add New Radio Program")
		.WithOpenApi(operation =>
		{
			operation.Description = "Creates a new radio program.";
			operation.Summary = "Add radio program";
			return operation;
		})
		.Produces<ProgramDetails>(StatusCodes.Status201Created);
	}
}