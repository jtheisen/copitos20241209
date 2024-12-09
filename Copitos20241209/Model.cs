using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Copitos20241209;

public class Person
{
	[Key]
	public Guid Id { get; set; }

	public String? Anrede { get; set; }

	[Required]
	public required String Vorname { get; set; }

	[Required]
	public required String Nachname { get; set; }

	public DateTime Geburtsdatum { get; set; }

	public String? Adresse { get; set; }

	[Required]
	public required String Plz { get; set; }

	public String? Ort { get; set; }

	public String? Land { get; set; }
}

public class AppDb : DbContext
{
	public DbSet<Person> People { get; set; }

    public AppDb()
    {
        
    }

    public AppDb(DbContextOptions options)
		: base(options)
    {
    }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
	}
}