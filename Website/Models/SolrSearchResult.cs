using SolrNet;
using SolrNet.Impl;
using System.Collections.Generic;

namespace Sitecore8EdismaxSearch.Website.Models
{
    /// <summary>
    /// The top-level result set from the solr proxy service.
    /// </summary>
    public class SolrProxyResultSet
    {
        public int NumFound { get; set; }

        public int Start { get; set; }

        public IEnumerable<string> SpellCheckCollationQuery { get; set; }

        public SpellCheckResults SpellChecking { get; set; }

        public ClusterResults Clusters { get; set; }

        public DebugResults Debug { get; set; }

        public IDictionary<string, ICollection<KeyValuePair<string, int>>> FacetFields { get; set; }

        public IDictionary<string, ICollection<KeyValuePair<string, int>>> FacetIntervals { get; set; }

        public IDictionary<string, IList<Pivot>> FacetPivots { get; set; }

        public IDictionary<string, int> FacetQueries { get; set; }

        public IDictionary<string, RangeFacetingResult> FacetRanges { get; set; }

        public object Grouping { get; set; }

        public ResponseHeader Header { get; set; }

        public IDictionary<string, HighlightedSnippets> Highlights { get; set; }

        public double? MaxScore { get; set; }

        public object SimilarResults { get; set; }

        public IDictionary<string, StatsResult> Stats { get; set; }

        public TermsResults Terms { get; set; }

        public ICollection<TermVectorDocumentResult> TermVectorResults { get; set; }

        public object Results { get; set; }
    }
}