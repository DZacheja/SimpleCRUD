namespace Radio.Domain.Models;

public class RadioProgram
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public DateTime StartTime { get; set; }
	public DateTime EndTime { get; set; }
	public int HostId { get; set; }
	public virtual Host? Host { get; set; }
	public int ProgramDetailsId { get; set; }
	public virtual ProgramDetails? ProgramDetails { get; set; }
	public virtual List<Music>? Musics { get; set; }
}