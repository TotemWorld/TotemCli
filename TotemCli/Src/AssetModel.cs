using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotemCli.Src
{
    internal class AssetModel
    {
        public string Name { get; set; } = null!;
        public byte[]? ByteContent { get; set; }
    }
}
