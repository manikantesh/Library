using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Profiles
{
	public class CoursesProfile:Profile
	{
		public CoursesProfile() {
			CreateMap<Entities.Course, Models.CourseDTO>();
			CreateMap<Models.CourseForCreationDTO, Entities.Course>();
			CreateMap<Models.CourseForUpdateDTO, Entities.Course>();
			CreateMap<Entities.Course, Models.CourseForUpdateDTO>();
		}
	}
}
