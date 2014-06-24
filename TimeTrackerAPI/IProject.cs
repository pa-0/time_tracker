using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Ficksworkshop.TimeTrackerAPI
{
    public interface IProject
    {
        string Identifier { get; set; }

        string Description { get; set; }

        bool IsActive { get; set; }
    }
}
