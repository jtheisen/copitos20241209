using Microsoft.AspNetCore.Mvc;

namespace Copitos20241209.Controllers;

[ApiController]
[Route("/people")]
public class PeopleController : ControllerBase
{
	private readonly AppDb db;

	public PeopleController(AppDb db)
	{
		this.db = db;
	}

	Person? Find(Guid id)
	{
		var person = db.People
			.Where(p => p.Id == id)
			.FirstOrDefault();

		return person;
	}

	[HttpGet]
	public IEnumerable<Person> GetAll()
	{
		return db.People.ToArray();
	}

	[HttpGet("{id}")]
	public ActionResult GetById(Guid id)
	{
		var person = Find(id);

		if (person is null)
		{
			return NotFound();
		}

		return Ok(person);
	}

	[HttpPost]
	public ActionResult Post(Person person)
	{
		if (Validate(person) is String error)
		{
			return BadRequest(error);
		}

		person.Id = Guid.NewGuid();

		db.People.Add(person);
		db.SaveChanges();

		return Ok(person);
	}

	[HttpPut("{id}")]
	public ActionResult Put(Guid id, Person person)
	{
		if (Find(id) is not Person existing)
		{
			return NotFound();
		}

		if (Validate(person) is String error)
		{
			return BadRequest(error);
		}

		person.Id = id;

		// This should be at least done in a transaction.

		db.People.Remove(existing);
		db.SaveChanges();

		db.People.Add(person);
		db.SaveChanges();

		return Ok(person);
	}

	[HttpDelete("{id}")]
	public ActionResult Delete(Guid id)
	{
		if (Find(id) is not Person existing)
		{
			return NotFound();
		}

		db.Remove(existing);
		db.SaveChanges();

		return Ok(existing);
	}

	String? Validate(Person person)
	{
		if (person.Geburtsdatum == default) return "'Geburtsdatum' is required";

		if (person.Geburtsdatum > DateTime.Now) return "'Geburtsdatum' must lie in the past";

		if (person.Plz.Length != 5) return "'plz' must be 5 digits long";

		if (!person.Plz.All(Char.IsDigit)) return "'plz' must be all digits";

		if (person.Land is String land && land.Length != 2) return "'land' must be exactly 2 characters long when specified";

		return null;
	}
}
