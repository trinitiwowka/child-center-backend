
using System.ComponentModel.DataAnnotations;

namespace GenericBackend.Models
{
    public class RegistrationModel
    {
        [Required(AllowEmptyStrings = false)]
        public string UserName { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Role { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Password { get; set; }
    }
}