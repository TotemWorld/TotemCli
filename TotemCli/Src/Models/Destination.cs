using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotemCli.Models
{
    internal class Destination
    {
        public string Name { get; set; } = null!;
        public List<Point> PolygonPoints { get; set; } = null!;
    }
}
