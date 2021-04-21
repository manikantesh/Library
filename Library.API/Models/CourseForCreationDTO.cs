using Library.API.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Models
{
	public class CourseForCreationDTO : CourseForManipulationDTO //: IValidatableObject
	{

		/*public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (Title == Description)
			{
				yield return new ValidationResult("The provided description should be different from the title.",new[] { "CourseForCreationDTO"});
			}
		} */
	}
}
