using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace TotemCli.Enums
{
    public enum ProcessType 
    {
        Interactive,
        JsonBatch
    }
    public enum Actions 
    {
        LoadAsset,
        CreateDestination,
        CreateExperience,
        CreateExplorer
    }
}
