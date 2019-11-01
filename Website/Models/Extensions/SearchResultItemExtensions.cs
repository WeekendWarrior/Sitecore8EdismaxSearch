using System;

namespace Sitecore8EdismaxSearch.Website.Models.Extensions
{
    /// <summary>
    /// Extension methods for the SearchResultItem class.
    /// </summary>
    public static class SearchResultItemExtensions
    {
        /// <summary>
        /// Parses the Sitecore Item ID from the solr "_uniqueid" field.
        /// Example: "sitecore://web/{b51ff454-d8bc-416f-bfdf-1a95bd792469}?lang=en&ver=1&ndx=sitecore_web_index"
        /// will return "{b51ff454-d8bc-416f-bfdf-1a95bd792469}"
        /// </summary>
        public static Guid? ParseSitecoreId(this SearchResultItemBase item)
        {
            if (item == null || string.IsNullOrWhiteSpace(item.UniqueId)) return null;

            // Find the position of the start bracket {
            var start = item.UniqueId.IndexOf("{", StringComparison.Ordinal);

            if (start <= -1) return null;

            // Remove the start bracket {
            start = start + 1;

            var id = item.UniqueId.Substring(start);

            // Find the position of the end bracket.
            var end = id.IndexOf("}", StringComparison.Ordinal);

            if (end <= -1) return null;

            // get the finalized guid as a string.
            id = id.Substring(0, end);

            // Parse into a guid.
            if (Guid.TryParse(id, out var guidId) && guidId != Guid.Empty)
            {
                return guidId;
            }

            return null;
        }
    }
}