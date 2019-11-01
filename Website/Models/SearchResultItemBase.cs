using System;
using System.Diagnostics;
using Sitecore8EdismaxSearch.Website.Models.Extensions;
using SolrNet.Attributes;

namespace Sitecore8EdismaxSearch.Website.Models
{
    /// <summary>
    /// Search result from the "sitecore_web_index" index.
    /// </summary>
    public class SearchResultItemWeb : SearchResultItemBase
    {
        public override string IndexName { get { return "sitecore_web_index"; } }
    }

    /// <summary>
    /// Search result from the "sitecore_liveweb_index" index.
    /// </summary>
    public class SearchResultItemLiveWeb : SearchResultItemBase
    {
        public override string IndexName { get { return "sitecore_liveweb_index"; } }
    }

    /// <summary>
    /// Base class for a SOLR search result item.
    /// </summary>
    [DebuggerDisplay("ID = {ID}, FullPath = {FullPath}")]
    public abstract class SearchResultItemBase
    {
        private Guid? _sitecoreId;

        public abstract string IndexName { get; }

        [SolrUniqueKey("_uniqueid")]
        public string UniqueId { get; set; }

        [SolrField("_fullpath")]
        public string FullPath { get; set; }

        [SolrField("__display_name_t")]
        public string[] ItemDisplayName { get; set; }

        [SolrField("_language")]
        public string Language { get; set; }

        [SolrField("_version")]
        public string Version { get; set; }

        [SolrField("_latestversion")]
        public bool LatestVersion { get; set; }

        [SolrField("__created_by_s")]
        public string CreatedBy { get; set; }

        [SolrField("_template")]
        public Guid? TemplateId { get; set; }

        [SolrField("_templatename")]
        public string TemplateName { get; set; }

        [SolrField("_parent")]
        public Guid? ParentId { get; set; }

        /// <summary>
        /// Gets the Sitecore ID.
        /// </summary>
        public Guid? ID => _sitecoreId ?? (_sitecoreId = this.ParseSitecoreId());
    }
}