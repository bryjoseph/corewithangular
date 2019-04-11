using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.DTOs
{
    public class UserForReqisteringDTO
    {
        // placing the validation here because the ApiControll annotation in the controller performs a ModelState check for the project
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "You must specify a password between 4 and 10 characters")]
        public string Password { get; set; }
    }
}