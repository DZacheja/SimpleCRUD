namespace Radio.API.DTO;

public sealed record CreateRadioProgramRequest(string Name, DateTime StartTime, int HostId, int ProgramDetailsId, int[] MusicIdList);
public sealed record UpdateRadioProgramRequest(int id, string Name, DateTime StartTime, int HostId, int ProgramDetailsId);
public sealed record AddMusicToRadioProgramRequest(int RadioProgramId, int MusicId);