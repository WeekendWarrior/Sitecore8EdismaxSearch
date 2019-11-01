using System.Linq;
using CommonServiceLocator;
using Sitecore8EdismaxSearch.Website.Configuration;
using Sitecore8EdismaxSearch.Website.Models;
using SolrNet;
using SolrNet.Commands.Parameters;

namespace Sitecore8EdismaxSearch.Website.Services.SearchRepository.Services
{
    /// <summary>
    /// Searches Solr.
    /// </summary>
    public class SolrSearchRepository: ISearchRepository        
    {
        /// <summary>
        /// Executes the query.
        /// </summary>
        public SolrProxyResultSet Execute<W,LW>(EDismaxQuery query)
            where W : SearchResultItemWeb
            where LW : SearchResultItemLiveWeb
        {
            var queryOptions = BuildQueryOptions(query);

            // Should we use liveweb index ("sitecore_liveweb_index") ?
            if (query.IndexName.Equals(AppSettings.SolrIndex_LiveWeb_IndexName, System.StringComparison.InvariantCultureIgnoreCase))
            {
                return GetResults<LW>(query, queryOptions);
            }

            // Default to web index ("sitecore_web_index")
            return GetResults<W>(query, queryOptions);
        }

        #region infrastructure

        /// <summary>
        /// Gets the live web results.
        /// </summary>
        private static SolrProxyResultSet GetResults<T>(EDismaxQuery query, QueryOptions queryOptions)
        {
            // Determine the solr instance to query against.
            var solr = ServiceLocator.Current.GetInstance<ISolrOperations<T>>();

            // Should we wrap they keyword with quotes?
            string keyword = query.ShouldWrapKeywordWithQuotes ? string.Concat("\"", query.Keyword, "\"") : query.Keyword;

            // Issue the solr query.
            SolrQueryResults<T> results = solr.Query(keyword, queryOptions);

            // Return the result set.
            return BuildSolrProxyResultSet(results);
        }

        /// <summary>
        /// Builds the query options.
        /// </summary>
        private static QueryOptions BuildQueryOptions(EDismaxQuery query)
        {
            return new QueryOptions
            {
                Rows = query.Rows,
                StartOrCursor = new StartOrCursor.Start(query.Start),
                ExtraParams = query.ExtraParams,
                FilterQueries = query.SolrFilterQueries,
                OrderBy = query.SolrSortOrder
            };
        }

        /// <summary>
        /// Builds the result set.
        /// </summary>
        private static SolrProxyResultSet BuildSolrProxyResultSet<T>(SolrQueryResults<T> results)
        {
            string[] spellCheckCollationQuery = Enumerable.Empty<string>().ToArray();

            if (results.SpellChecking != null && results.SpellChecking.Collations != null)
            {
                spellCheckCollationQuery = results.SpellChecking.Collations.Select(c => c.CollationQuery).ToArray();
            }

            return new SolrProxyResultSet
            {
                Clusters = results.Clusters,
                Debug = results.Debug,
                FacetFields = results.FacetFields,
                FacetIntervals = results.FacetIntervals,
                FacetPivots = results.FacetPivots,
                FacetQueries = results.FacetQueries,
                FacetRanges = results.FacetRanges,
                Grouping = results.Grouping,
                Header = results.Header,
                Highlights = results.Highlights,
                MaxScore = results.MaxScore,
                NumFound = results.NumFound,
                Results = results.ToArray(),
                SimilarResults = results.SimilarResults,
                SpellChecking = results.SpellChecking,
                SpellCheckCollationQuery = spellCheckCollationQuery,
                Start = results.Start,
                Stats = results.Stats,
                Terms = results.Terms,
                TermVectorResults = results.TermVectorResults
            };
        }

        #endregion
    }    
}