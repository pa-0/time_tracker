using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ficksworkshop.TimeTrackerAPI
{
    public class Project : IProject
    {
        public string Identifier { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }
    }
}
