namespace Radio.Domain.Models;

public class ProgramDetails
{
	public int Id { get; set; }
	public string Description { get; set; } = string.Empty;
	public TimeSpan Duration { get; set; }
	public virtual RadioProgram? RadioProgram { get; set; }
}