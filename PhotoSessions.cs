using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotostudioProject
{
    class PhotoSessions
    {
        public int IdPs { get; set; }
        public int IdClient { get; set; }
        public int IdPhotographer { get; set; }
        public DateTime DateOfSession { get; set; }

        public string Location { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string StatusOfSession { get; set; } = string.Empty;
        public int TypeOfService { get; set; }
    }
}
