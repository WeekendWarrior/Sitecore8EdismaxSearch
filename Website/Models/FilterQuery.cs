using Newtonsoft.Json;

namespace Sitecore8EdismaxSearch.Website.Models
{
    /// <summary>
    /// A Solr Filter query.
    /// </summary>
    public class FilterQuery : EDismaxQueryBase
    {
        [JsonProperty("Quoted")]
        public bool Quoted { get; set; }

        [JsonProperty("FieldName")]
        public string FieldName { get; set; }

        [JsonProperty("FieldValue")]
        public string FieldValue { get; set; }

        [JsonProperty("Expression")]
        public string Expression { get; set; }

        [JsonProperty("Query")]
        public FilterQuery Query { get; set; }

        [JsonProperty("Queries")]
        public FilterQuery[] Queries { get; set; }

        [JsonProperty("Oper")]
        public string Operator { get; set; }

        [JsonProperty("Field")]
        public string Field { get; set; }
    }
}