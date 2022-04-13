using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Core.BindingModel
{
    public class LoginBindingModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
    }
}
