namespace Radio.Domain.Models;

public class Music
{
	public int Id { get; set; }
	public string Title { get; set; } = string.Empty;
	public string Artist { get; set; } = string.Empty;
	public List<RadioProgram> RadioPrograms { get; set; } = null!;
}