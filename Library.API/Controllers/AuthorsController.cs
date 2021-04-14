using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers

{ 
	[ApiController]
	[Route("api/authors")]
	public class AuthorsController : ControllerBase
		{
		private readonly ILibraryRepository _libraryRepository;

		public AuthorsController(ILibraryRepository libraryRepository) {
			_libraryRepository = libraryRepository ?? throw new ArgumentException(nameof(libraryRepository));
		}

		[HttpGet("")]
		public IActionResult GetAuthors() {

			var authorsFromRepo = _libraryRepository.GetAuthors();
			return Ok(authorsFromRepo);
		}

		[HttpGet("{authorId}")]
		public IActionResult GetAuthor(Guid authorId)
		{
			var authorsFromRepo = _libraryRepository.GetAuthor(authorId);
			if (authorsFromRepo == null) {
				return NotFound();
			}
			return new JsonResult(authorsFromRepo);
		}
	}
}
