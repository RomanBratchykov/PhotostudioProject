using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotostudioProject
{
    class Photographer
    {
        public int IdPhotographer { get; set; }
        public string NameOfPhotographer { get; set; } = string.Empty;
        public int IdOfLocation { get; set; }
        public int IdOfAdmin { get; set; }
        public string EmailOfPhotographer { get; set; } = string.Empty;
        public string PhoneNumberPhotographer { get; set; } = string.Empty;
        public string PasswordPhotographer { get; set; } = string.Empty;
    }
}
