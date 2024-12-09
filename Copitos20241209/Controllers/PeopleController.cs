using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Copitos20241209.Controllers
{
	public class Person
	{
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

	[ApiController]
	[Route("/people")]
	public class PeopleController : ControllerBase
	{
		static List<Person> People { get; set; } = new List<Person>();

		public PeopleController()
		{
		}

		Person? Find(Guid id)
		{
			var person = People
				.Where(p => p.Id == id)
				.FirstOrDefault();

			return person;
		}

		[HttpGet]
		public IEnumerable<Person> GetAll()
		{
			return People;
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

			People.Add(person);

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

			People.Remove(existing);
			People.Add(person);

			return Ok(person);
		}

		[HttpDelete("{id}")]
		public ActionResult Delete(Guid id)
		{
			if (Find(id) is not Person existing)
			{
				return NotFound();
			}

			People.Remove(existing);

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
}
