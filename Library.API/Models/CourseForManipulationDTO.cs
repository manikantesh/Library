using Library.API.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Models
{
	[CourseTitleMustBeDifferentFromDescription(ErrorMessage = "Title must be different than Description")]
	public abstract class CourseForManipulationDTO
	{
		[Required(ErrorMessage = "You should fill out a title")]
		[MaxLength(100, ErrorMessage = "No more than 100 characters")]
		public string Title { get; set; }
		[MaxLength(1500, ErrorMessage = "No more than 1500 characters")]
		public virtual string Description { get; set; }
	}
}
