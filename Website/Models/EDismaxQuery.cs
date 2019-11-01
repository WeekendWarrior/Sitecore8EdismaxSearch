using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Sitecore8EdismaxSearch.Website.Models.Extensions;
using SolrNet;

namespace Sitecore8EdismaxSearch.Website.Models
{
    /// <summary>
    /// A Solr EDismax Query definition.
    /// </summary>
    public class EDismaxQuery : EDismaxQueryBase
    {
        private List<ISolrQuery> _solrQueries;
        private ICollection<SortOrder> _solrSortOrder;

        [JsonProperty("Keyword")]
        public string Keyword { get; set; }

        [JsonProperty("ExtraParams")]
        public List<KeyValuePair<string, string>> ExtraParams { get; set; }

        [JsonProperty("Rows")]
        public int Rows { get; set; }

        [JsonProperty("Start")]
        public int Start { get; set; }

        [JsonProperty("FilterQueries")]
        public List<FilterQuery> FilterQueries { get; set; }

        [JsonProperty("MinimumMatch")]
        public string MinimumMatch { get; set; }

        [JsonProperty("IndexName")]
        public string IndexName { get; set; }

        [JsonProperty("ShouldWrapKeywordWithQuotes")]
        public bool ShouldWrapKeywordWithQuotes { get; set; }

        [JsonProperty("OrderBy")]
        public ICollection<EdismaxSortOrder> OrderBy { get; set; }

        [JsonIgnore]
        public List<ISolrQuery> SolrFilterQueries
        {
            get
            {
                // Lazy load & ensure we only convert once in the instance's lifetime.
                if (_solrQueries == null)
                {
                    // Because Newtonsoft JSONConvert throws an error when trying to do it automatically,
                    // We must manually convert the simple POCO object to a ISolrQuery type.
                    _solrQueries = FilterQueries
                        .Select(q => q.ConvertToSolrQuery())
                        .Where(q => q != null)
                        .ToList();
                }

                return _solrQueries;
            }
        }

        [JsonIgnore]
        public ICollection<SortOrder> SolrSortOrder
        {
            get
            {
                if (_solrSortOrder == null)
                {
                    // Because Newtonsoft JSONConvert throws an error when trying to do it automatically,
                    // We must manually convert the simple POCO object to a SolrNet.SortOrder type.

                    if (OrderBy != null)
                    {
                        _solrSortOrder = OrderBy
                            .Select(o => o.ConvertToSolrSortOrder())
                            .Where(o => o != null)
                            .ToList();
                    }
                    else
                    {
                        _solrSortOrder = Enumerable.Empty<SortOrder>().ToList();
                    }
                }

                return _solrSortOrder;
            }
        }
    }

}