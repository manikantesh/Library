using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Library.API.Helpers;
using Library.API.Models;
using Library.API.ResourceParameters;
using Library.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers

{
	[ApiController]
	[Route("api/authors")]
	public class AuthorsController : ControllerBase
	{
		private readonly ILibraryRepository _libraryRepository;
		private readonly IMapper _mapper;

		public AuthorsController(ILibraryRepository libraryRepository, IMapper mapper) {
			_libraryRepository = libraryRepository ?? throw new ArgumentException(nameof(libraryRepository));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		[HttpGet()]
		[HttpHead]
		public ActionResult<IEnumerable<AuthorDTO>> GetAuthors(AuthorsResourceParameters authorsResourceParameters) {

			var authorsFromRepo = _libraryRepository.GetAuthors(authorsResourceParameters);
			return Ok(_mapper.Map<IEnumerable<AuthorDTO>>(authorsFromRepo));
		}

		[HttpGet("{authorId}", Name = "GetAuthor")]
		public IActionResult GetAuthor(Guid authorId)
		{
			var authorsFromRepo = _libraryRepository.GetAuthor(authorId);
			if (authorsFromRepo == null) {
				return NotFound();
			}
			return Ok(_mapper.Map<AuthorDTO>(authorsFromRepo));
		}

		[HttpPost]
		public ActionResult<AuthorDTO> CreateAuthor(AuthorForCreationDTO author) {

			var authorEntity = _mapper.Map<Entities.Author>(author);
			_libraryRepository.AddAuthor(authorEntity);
			_libraryRepository.Save();

			var authorToReturn = _mapper.Map<AuthorDTO>(authorEntity);
			return CreatedAtRoute("GetAuthor", new { authorId = authorToReturn.Id }, authorToReturn);
		}
 	}
}
