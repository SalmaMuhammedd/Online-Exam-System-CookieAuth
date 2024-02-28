using System.ComponentModel.DataAnnotations;

namespace FinalExamCorrection.Models
{
    public class User
    {
        public string Email { get; set; }

		[DataType(DataType.Password)]
		public string Password { get; set; }
        public string Id { get; set; }
    }
}
