using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyIntelligentHomeSystem.Models
{
    public class PersonsInFaceGroup
    {
        public string personId { get; set; }
        public string name { get; set; }
        public string userData { get; set; }
        public List<string> persistedFaceIds { get; set; }

    }
}
