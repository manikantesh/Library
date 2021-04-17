using Library.API.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Models
{
	[CourseTitleMustBeDifferentFromDescription(ErrorMessage ="Title must be different than Description")]
	public class CourseForCreationDTO //: IValidatableObject
	{
		[Required(ErrorMessage = "You should fill out a title")]
		[MaxLength(100,ErrorMessage = "No more than 100 characters")]
		public string Title { get; set; }
		[MaxLength(1500,ErrorMessage = "No more than 1500 characters")]
		public string Description { get; set; }

		/*public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (Title == Description)
			{
				yield return new ValidationResult("The provided description should be different from the title.",new[] { "CourseForCreationDTO"});
			}
		} */
	}
}
