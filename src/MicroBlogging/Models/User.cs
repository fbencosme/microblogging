using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroBlogging.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Required")]
        [EmailAddress(ErrorMessage = "Invalid")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Required")]
        [Compare("Password", ErrorMessage = "NotMatch")]
        public string ConfirmPassword { get; set; }

        public string Picture { get; set; }
    }

    public class LoginUser
    {
        [Required(ErrorMessage = "Required")]
        [EmailAddress(ErrorMessage = "Invalid")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Password { get; set; }
    }

    public class UserDTO
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Username { get; set; }

        public string Gender { get; set; }

        public string Picture { get; set; }

        public bool Following { get; set; }
    }
}