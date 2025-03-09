namespace Radio.Domain.Models;

public class RadioProgram
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public DateTime StartTime { get; set; }
	public DateTime EndTime { get; set; }
	public int HostId { get; set; }
	public Host Host { get; set; } = null!;
	public int ProgramDetailsId { get; set; }
	public virtual ProgramDetails? ProgramDetails { get; set; }
	public List<Music> Musics { get; set; } = null!;
}