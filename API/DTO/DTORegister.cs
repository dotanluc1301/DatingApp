using System.ComponentModel.DataAnnotations;

namespace API.DTO
{
    public class DTORegister
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}