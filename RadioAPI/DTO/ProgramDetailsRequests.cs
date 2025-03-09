namespace Radio.API.DTO;

public sealed record CreateProgramDetailsRequest(string Description, TimeSpan Duration);