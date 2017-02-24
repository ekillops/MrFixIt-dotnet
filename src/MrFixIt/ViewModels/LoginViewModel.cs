using System.ComponentModel.DataAnnotations;

namespace MrFixIt.ViewModels
{
    public class LoginViewModel
    {
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
