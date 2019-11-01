using System.Diagnostics;

namespace Sitecore8EdismaxSupport.Models
{
    /// <summary>
    /// Represents a result item from the Proxy Solr service.
    /// </summary>
    [DebuggerDisplay("ID = {ID}; FullPath = {FullPath}")]
    public class SolrProxyResultItem
    {
        public string UniqueId { get; set; }

        public string FullPath { get; set; }

        public string Language { get; set; }

        public string Version { get; set; }

        public bool LatestVersion { get; set; }

        public string CreatedBy { get; set; }

        public string TemplateId { get; set; }

        public string TemplateName { get; set; }

        public string ParentId { get; set; }

        public string ID { get; set; }

        public virtual string[] ItemDisplayName { get; set; }
    }
}
