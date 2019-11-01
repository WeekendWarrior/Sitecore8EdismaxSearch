using Newtonsoft.Json;

namespace Sitecore8EdismaxSearch.Website.Models
{
    public class EdismaxSortOrder
    {
        [JsonProperty("FieldName")]
        public string FieldName { get; set; }

        [JsonProperty("Order")]
        public EdismaxOrder Order { get; set; }
    }

    public enum EdismaxOrder
    {
        ASC = 0,
        DESC = 1
    }
}