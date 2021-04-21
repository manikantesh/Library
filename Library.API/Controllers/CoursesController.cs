using AutoMapper;
using Library.API.Models;
using Library.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
		[HttpPut("{courseId}")]
		public IActionResult UpdateCourseForAuthor(Guid authorId, Guid courseId, CourseForUpdateDTO courseForUpdateDTO) {
			if (_libraryRepository.AuthorExists(authorId)) {
				return NotFound();
			}

			var courseForAuthorFromRepo = _libraryRepository.GetCourse(authorId, courseId);

			if (courseForAuthorFromRepo == null) {
				var courseToAdd = _mapper.Map<Entities.Course>(courseForUpdateDTO);
				courseToAdd.Id = courseId;

				_libraryRepository.AddCourse(authorId, courseToAdd);
				_libraryRepository.Save();

				var courseToReturn = _mapper.Map<CourseDTO>(courseToAdd);

				return CreatedAtRoute("GetCourseForAuthor",new { authorId,courseToReturn.Id},courseToReturn);
			}

			//map the entity to a courseForUpdateDTO
			//apply the updated field values to that DTO
			// map the CourseForUpdateDTO back to an entitiy

			_mapper.Map(courseForUpdateDTO, courseForAuthorFromRepo);

			_libraryRepository.UpdateCourse(courseForAuthorFromRepo);
			//Learn Repository Pattern
			_libraryRepository.Save();
			return NoContent();
		}
		[HttpPatch("{courseId}")]
		public ActionResult PartiallyUpdateCourseForAuthor(Guid authorId, Guid courseId,JsonPatchDocument<CourseForUpdateDTO> patchDocument) {
			if (!_libraryRepository.AuthorExists(authorId))
			{
				return NotFound();
			}
			var coursesForAuthorFromRepo = _libraryRepository.GetCourse(authorId, courseId);

			if (coursesForAuthorFromRepo == null)
			{
				var courseDTO = new CourseForUpdateDTO();
				patchDocument.ApplyTo(courseDTO,ModelState);

				if (TryValidateModel(courseDTO))
				{
					return ValidationProblem(ModelState);
				}

				var courseToAdd = _mapper.Map<Entities.Course>(courseDTO);
				courseToAdd.Id = courseId;

				_libraryRepository.AddCourse(authorId, courseToAdd);
				_libraryRepository.Save();

				var courseToReturn = _mapper.Map<CourseDTO>(courseToAdd);

				return CreatedAtRoute("GetCourseForAuthor", new {authorId,courseId=courseToReturn.Id },courseToReturn);
			}

			var courseToPatch = _mapper.Map<CourseForUpdateDTO>(coursesForAuthorFromRepo);
			patchDocument.ApplyTo(courseToPatch,ModelState);

			if (TryValidateModel(courseToPatch)) {
				return ValidationProblem(ModelState);
			}

			_mapper.Map(courseToPatch, coursesForAuthorFromRepo);

			_libraryRepository.UpdateCourse(coursesForAuthorFromRepo);
			//Learn Repository Pattern
			_libraryRepository.Save();
			return NoContent();
		}

		public override ActionResult ValidationProblem([ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
		{
			var options = HttpContext.RequestServices
			.GetRequiredService<IOptions<ApiBehaviorOptions>>();

			return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
		}
	}
}
