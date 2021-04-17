using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Models
{
	public class AuthorDTO
	{

		public Guid Id { get; set; }
		public string Name { get; set; }
		public int Age { get; set; }
		[Required]
		[MaxLength(50)]
		public string MainCategory { get; set; }

	}
}
