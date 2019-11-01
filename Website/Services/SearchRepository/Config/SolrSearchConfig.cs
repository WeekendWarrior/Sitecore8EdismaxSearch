using Sitecore8EdismaxSearch.Website.Models;
using SolrNet;
using System;
using Sitecore8EdismaxSearch.Website.Configuration;

namespace Sitecore8EdismaxSearch.Website.Services.SearchRepository.Config
{
    /// <summary>
    /// Configures Solr Containers application wide.
    /// </summary>
    public static class SolrSearchConfig
    {
        /// <summary>
        /// Configures Solr
        /// </summary>
        public static void Configure(Action configurationCallback)
        {
            configurationCallback.Invoke();
        }

        /// <summary>
        /// Registers Solr containers application wide.
        /// </summary>
        public static void Container()
        {
            string webIndexUrl = BuildSolrUrl(AppSettings.SolrIndex_Web_IndexName);
            string liveWebIndexUrl = BuildSolrUrl(AppSettings.SolrIndex_LiveWeb_IndexName);

            // Generic Results - Web & LiveWeb
            Startup.Init<SearchResultItemWeb>(webIndexUrl);
            Startup.Init<SearchResultItemLiveWeb>(liveWebIndexUrl);

            // TODO: Add any custom SOLR search containers below.
        }

        #region infrastructure

        /// <summary>
        /// Builds a solr url.
        /// </summary>
        private static string BuildSolrUrl(string indexName)
        {
            if (string.IsNullOrWhiteSpace(indexName))
            {
                throw new ArgumentNullException(nameof(indexName));
            }

            return BuildSolrUrl(AppSettings.SolrIndex_BaseUrl, indexName);
        }

        /// <summary>
        /// Builds a solr url.
        /// </summary>
        private static string BuildSolrUrl(string baseUrl, string indexName)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new ArgumentNullException(nameof(baseUrl));
            }

            if (string.IsNullOrWhiteSpace(indexName))
            {
                throw new ArgumentNullException(nameof(indexName));
            }

            // Add a trailing slash to the base url if it is missing.
            baseUrl = !AppSettings.SolrIndex_BaseUrl.EndsWith("/")
                ? string.Concat(AppSettings.SolrIndex_BaseUrl, "/")
                : AppSettings.SolrIndex_BaseUrl;

            return string.Concat(baseUrl, indexName);
        }

        #endregion
    }
}