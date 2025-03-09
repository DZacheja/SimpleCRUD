namespace Radio.Domain.Models;

public class Host
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public List<RadioProgram> Programs { get; set; } = null!;
}