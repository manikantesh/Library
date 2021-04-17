using AutoMapper;
using Library.API.Models;
using Library.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Controllers
{
	[Route("api/authors/{authorId}/courses")]
	[ApiController]
	public class CoursesController : ControllerBase
	{
		private readonly ILibraryRepository _libraryRepository;
		private readonly IMapper _mapper;

		public CoursesController(ILibraryRepository libraryRepository, IMapper mapper)
		{
			_libraryRepository = libraryRepository ?? throw new ArgumentException(nameof(libraryRepository));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		[HttpGet]
		public ActionResult<IEnumerable<CourseDTO>> GetCoursesForAuthor(Guid authorId) {
			if (!_libraryRepository.AuthorExists(authorId)) {
				return NotFound();
			}
			var coursesForAuthorFromRepo = _libraryRepository.GetCourses(authorId);
			return Ok(_mapper.Map<IEnumerable<CourseDTO>>(coursesForAuthorFromRepo));	
		}

		[HttpGet("{courseId}",Name = "GetCoursesForAuthor")]
		public ActionResult<CourseDTO> GetCoursesForAuthor(Guid authorId,Guid courseId)
		{
			if (!_libraryRepository.AuthorExists(authorId))
			{
				return NotFound();
			}
			var coursesForAuthorFromRepo = _libraryRepository.GetCourse(authorId,courseId);

			if (coursesForAuthorFromRepo == null) {
				return NotFound();
			}

			return Ok(_mapper.Map<CourseDTO>(coursesForAuthorFromRepo));
		}

		[HttpPost]
		public ActionResult<CourseDTO> CreateCourseForAuthor(Guid authorId,CourseForCreationDTO courseForCreationDTO) {
			if (!_libraryRepository.AuthorExists(authorId)) {
				return NotFound();
			}

			var courseEntity = _mapper.Map<Entities.Course>(courseForCreationDTO);
			_libraryRepository.AddCourse(authorId, courseEntity);
			_libraryRepository.Save();

			var courseToReturn = _mapper.Map<CourseDTO>(courseEntity);

			return CreatedAtRoute("GetCoursesForAuthor",new { authorId=authorId,courseId = courseToReturn.Id},courseToReturn);
		}
	}
}
