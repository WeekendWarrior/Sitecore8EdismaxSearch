using System.Collections.Generic;
using SolrNet;

namespace Sitecore8EdismaxSupport.Models
{
    /// <summary>
    /// Stores information about an Solr Edismax Query.
    /// </summary>
    public class EDismaxQuery
    {
        public string Keyword { get; set; }

        public List<KeyValuePair<string, string>> ExtraParams { get; set; }

        public int Rows { get; set; }

        public int Start { get; set; }

        public List<ISolrQuery> FilterQueries { get; set; }

        public string MinimumMatch { get; set; }

        public string IndexName { get; set; }

        public bool ShouldWrapKeywordWithQuotes { get; set; }

        public ICollection<SortOrder> OrderBy { get; set; }
    }
}
