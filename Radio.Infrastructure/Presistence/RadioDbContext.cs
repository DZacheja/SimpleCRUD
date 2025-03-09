using Microsoft.EntityFrameworkCore;
using Radio.Domain.Models;

namespace Radio.Infrastructure.Presistence;

public class RadioDbContext : DbContext
{
	public DbSet<Host> Hosts { get; set; }
	public DbSet<Music> Musics { get; set; }
	public DbSet<ProgramDetails> ProgramDetails { get; set; }
	public DbSet<RadioProgram> RadioPrograms { get; set; }

	public RadioDbContext(DbContextOptions<RadioDbContext> options) : base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<RadioProgram>()
			.HasOne(rp => rp.Host)
			.WithMany(h => h.Programs)
			.HasForeignKey(rp => rp.HostId);

		modelBuilder.Entity<RadioProgram>()
				.HasOne(rp => rp.ProgramDetails)
				.WithOne(pd => pd.RadioProgram)
				.HasForeignKey<RadioProgram>(rp => rp.ProgramDetailsId)
				.OnDelete(DeleteBehavior.Cascade);

		// Entity framerowk will create a join table for this many to many relationship
		modelBuilder.Entity<RadioProgram>()
			.HasMany(rp => rp.Musics)
			.WithMany(m => m.RadioPrograms);

		modelBuilder.Entity<Host>()
			.HasMany(h => h.Programs)
			.WithOne(rp => rp.Host)
			.HasForeignKey(rp => rp.HostId);
	}
}