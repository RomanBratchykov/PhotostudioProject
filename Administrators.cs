using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotostudioProject
{
    class Administrators
    {
        public int IdAdmin { get; set; }
        public string NameOfAdmin { get; set; } = string.Empty;
        public int IdOfLocation { get; set; }
        public string EmailOfAdmin { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string PasswordAdmin { get; set; } = string.Empty;
    }
}
