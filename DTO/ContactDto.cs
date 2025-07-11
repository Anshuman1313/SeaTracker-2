using System.ComponentModel.DataAnnotations;

namespace Assiginment.DTO
{
    public class ContactDto
    {
        [Required(ErrorMessage = "FirstName is required.")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required.")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email format.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Mobile Number is required.")]
        public string? MobileNumber { get; set; }
        [Required(ErrorMessage = "Gender is required.")]
        public string? Gender { get; set; }
    }
}
