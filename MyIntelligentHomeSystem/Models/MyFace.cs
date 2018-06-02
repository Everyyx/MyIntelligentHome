using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyIntelligentHomeSystem.Models
{
    public class MyDetectFace
    {
        public string faceId { get; set; }

        public faceRectangle faceRectangle { get; set; }
    }

    public class MyIdentifyFace
    {
        public string faceId { get; set; }
        public List<candidates> candidates { get; set; }
    }
    public class faceRectangle
    {
        public int top { get; set; }
        public int left { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class candidates
    {
        public string personId { get; set; }
        public double confidence { get; set; }
    }
}
