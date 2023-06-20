using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TotemCli.Models
{
    public class Experience
    {
        public string ExperienceName { get; set; } = null!;
        public string? Description { get; set; } = null!;
        public string[] Assets { get; set; } = null!;
        public DateTime OpeningDateTime { get; set; } 
        public DateTime EndDateTime { get; set; }
        public string[] InDestinations { get; set; } = null!;
        [JsonConverter(typeof(StringEnumConverter))]
        public ExperienceType ExperienceType { get; set; }
    }

    public enum ExperienceType
    {
        [EnumMember(Value = "destinationOpening")]
        DestinationOpening,
        [EnumMember(Value = "discoverAndCollect")]
        DiscoverAndCollect,
        [EnumMember(Value = "scavengerHunt")]
        ScavengerHunt
    }
}
