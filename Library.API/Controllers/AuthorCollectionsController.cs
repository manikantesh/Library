using AutoMapper;
using Library.API.Helpers;
using Library.API.Models;
using Library.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Controllers
{
	[ApiController]
	[Route("api/authorcollections")]
	public class AuthorCollectionsController : Controller
	{
		private readonly ILibraryRepository _libraryRepository;
		private readonly IMapper _mapper;

		public AuthorCollectionsController(ILibraryRepository libraryRepository,IMapper mapper) {
			_libraryRepository = libraryRepository ?? throw new ArgumentNullException(nameof(libraryRepository));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		[HttpGet("({ids})",Name ="GetAuthorCollection")]
		public IActionResult GetAuthorCollection([FromRoute][ModelBinder(BinderType =typeof(ArrayModelBinder))] IEnumerable<Guid> ids) {
			if (ids == null) {
				return BadRequest();
			}
			var authorEntities = _libraryRepository.GetAuthors(ids);

			if (ids.Count() != authorEntities.Count()) {
				return NotFound();
			}

			var authorsToReturn = _mapper.Map<IEnumerable<AuthorDTO>>(authorEntities);

			return Ok(authorsToReturn);
		}

		[HttpPost]
		public ActionResult<IEnumerable<AuthorDTO>> CreateAuthorCollection(IEnumerable<AuthorForCreationDTO> authorCollection) {
			var authorEnitites = _mapper.Map<IEnumerable<Entities.Author>>(authorCollection);
			foreach (var author in authorEnitites) {
				_libraryRepository.AddAuthor(author);
			}
			_libraryRepository.Save();

			var authorCollectionToReturn = _mapper.Map<IEnumerable<AuthorDTO>>(authorEnitites);
			var idsAsString = string.Join(",",authorCollectionToReturn.Select(a=>a.Id));
			return CreatedAtRoute("GetAuthorCollection",new { ids=idsAsString},authorCollectionToReturn);
		}

		[HttpOptions]
		public IActionResult GetAuthorOptions() {
			Response.Headers.Add("Allow","GET,OPTIONS,POST");
			return Ok();
		}
	}
}
