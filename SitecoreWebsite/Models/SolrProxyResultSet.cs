using Newtonsoft.Json;
using SolrNet.Impl;
using System.Collections.Generic;

namespace Sitecore8EdismaxSupport.Models
{
    /// <summary>
    /// The top-level result set from the solr proxy service.
    /// </summary>
    public class SolrProxyResultSet<T> where T : SolrProxyResultItem
    {
        [JsonProperty("Results")]
        public T[] Results { get; set; }

        [JsonProperty("NumFound")]
        public int NumFound { get; set; }

        [JsonProperty("Start")]
        public int Start { get; set; }

        [JsonProperty("Header")]
        public HeaderResult Header { get; set; }

        [JsonProperty("SpellChecking")]
        public SpellCheckResults SpellChecking { get; set; }

        [JsonProperty("SpellCheckCollationQuery")]
        public IEnumerable<string> SpellCheckCollationQuery { get; set; }

        [JsonProperty("Clusters")]
        public object Clusters { get; set; }

        [JsonProperty("Debug")]
        public object Debug { get; set; }

        [JsonProperty("FacetFields")]
        public object FacetFields { get; set; }

        [JsonProperty("FacetIntervals")]
        public object FacetIntervals { get; set; }

        [JsonProperty("FacetPivots")]
        public object FacetPivots { get; set; }

        [JsonProperty("FacetQueries")]
        public object FacetQueries { get; set; }

        [JsonProperty("FacetRanges")]
        public object FacetRanges { get; set; }

        [JsonProperty("Grouping")]
        public object Grouping { get; set; }

        [JsonProperty("Highlights")]
        public object Highlights { get; set; }

        [JsonProperty("MaxScore")]
        public object MaxScore { get; set; }

        [JsonProperty("Similar")]
        public object Similar { get; set; }

        [JsonProperty("Stats")]
        public object Stats { get; set; }

        [JsonProperty("Terms")]
        public object[] Terms { get; set; }

        [JsonProperty("TermVectorResults")]
        public object TermVectorResults { get; set; }

        public class Params
        {
            [JsonProperty("q")]
            public string Query { get; set; }

            [JsonProperty("defType")]
            public string DefinitionType { get; set; }

            [JsonProperty("qf")]
            public string QF { get; set; }

            [JsonProperty("rows")]
            public string Rows { get; set; }

            [JsonProperty("version")]
            public string Version { get; set; }

            [JsonProperty("wt")]
            public string WT { get; set; }
        }

        public class HeaderResult
        {
            [JsonProperty("Status")]
            public int Status { get; set; }

            [JsonProperty("QTime")]
            public int QueryTime { get; set; }

            [JsonProperty("Params")]
            public Params Parameters { get; set; }
        }
    }
}
