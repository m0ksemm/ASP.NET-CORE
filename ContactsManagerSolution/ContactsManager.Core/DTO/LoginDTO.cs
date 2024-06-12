using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ContactsManager.Core.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Can't be blank")]
        [EmailAddress(ErrorMessage = "Email should be in a prper email address format")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password can't be blank")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

    }
}
