using Newtonsoft.Json;

namespace Sitecore8EdismaxSearch.Website.Models
{
    public abstract class EDismaxQueryBase
    {
        [JsonProperty("Type")]
        public string OriginalType { get; set; }
    }
}