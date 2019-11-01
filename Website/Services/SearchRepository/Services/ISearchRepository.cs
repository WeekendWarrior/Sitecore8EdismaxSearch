using Sitecore8EdismaxSearch.Website.Models;

namespace Sitecore8EdismaxSearch.Website.Services.SearchRepository.Services
{
    /// <summary>
    /// Searches a Sitecore Solr content index.
    /// </summary>
    public interface ISearchRepository
    {
        /// <summary>
        /// Executes EDismaxQuery on solr.
        /// </summary>
        SolrProxyResultSet Execute<W, LW>(EDismaxQuery query)
            where W : SearchResultItemWeb
            where LW : SearchResultItemLiveWeb;
    }
}
;