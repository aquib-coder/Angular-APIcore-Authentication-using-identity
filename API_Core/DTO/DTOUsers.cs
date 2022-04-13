using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Core.DTO
{
    public class DTOUsers
    {
        public DTOUsers(string fullName,string Email,string UserName,DateTime dateModified)
        {
            FullName = fullName;
            this.Email = Email;
            this.UserName = UserName;
            DateModified = dateModified;
        }
        public string FullName{ get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public DateTime DateModified { get; set; }
        public string token { get; set; }
    }
}
