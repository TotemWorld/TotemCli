using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotemCli.Models
{
    internal class Asset
    {
        public string Name { get; set; } = null!;
        public byte[]? ByteContent { get; set; }
        public string? DestinationId { get; set; }
        public string? ExperienceId { get; set; }
        public Point Point { get; set; } = null!;

    }
}
