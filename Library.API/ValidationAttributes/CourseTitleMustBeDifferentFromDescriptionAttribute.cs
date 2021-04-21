﻿using Library.API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.ValidationAttributes
{
	public class CourseTitleMustBeDifferentFromDescriptionAttribute:ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var course = (CourseForManipulationDTO)validationContext.ObjectInstance;

			if (course.Title == course.Description)
			{
				return new ValidationResult(ErrorMessage, new[] { nameof(CourseForManipulationDTO) });
			}

			return ValidationResult.Success;
		}
	}
}
