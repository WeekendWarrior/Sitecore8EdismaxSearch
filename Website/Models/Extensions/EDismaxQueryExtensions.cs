using SolrNet;
using System.Linq;

namespace Sitecore8EdismaxSearch.Website.Models.Extensions
{
    public static class EDismaxQueryExtensions
    {
        /// <summary>
        /// Converts a EDismaxQuery.FilterQuery (JSON object) to a ISolrQuery object.
        /// </summary>
        public static ISolrQuery ConvertToSolrQuery(this FilterQuery fq)
        {
            const string SolrQuery = "SolrNet.SolrQuery";
            const string SolrNotQuery = "SolrNet.SolrNotQuery, SolrNet";
            const string SolrQueryByFieldRegex = "SolrNet.SolrQueryByFieldRegex, SolrNet";
            const string SolrHasValueQuery = "SolrNet.SolrHasValueQuery, SolrNet";

            if (fq != null)
            {
                // Is it a custom query?
                if (fq.FieldName != null && fq.FieldName.Equals(SolrQuery, System.StringComparison.CurrentCultureIgnoreCase))
                {
                    return new SolrQuery(fq.FieldValue);
                }

                bool isNotQuery = fq.OriginalType != null && fq.OriginalType.Equals(SolrNotQuery, System.StringComparison.InvariantCultureIgnoreCase);

                // If "Query" property is populated.
                if (fq.Query != null)
                {
                    // Note the recursive call to ConvertToSolrQuery().
                    if (isNotQuery)
                    {
                        return new SolrNotQuery(ConvertToSolrQuery(fq.Query));
                    }

                    // propagate the original type to the sub query to make sure the "SolrNotQuery" is respected.
                    fq.Query.OriginalType = fq.OriginalType;

                    return ConvertToSolrQuery(fq.Query);
                }

                // If there is an array of queries.
                if (fq.Queries != null && fq.Queries.Any())
                {
                    // Note the recursive call to ConvertToSolrQuery().
                    return new SolrMultipleCriteriaQuery(fq.Queries.Select(ConvertToSolrQuery), fq.Operator);
                }

                // SolrHasValueQuery
                if (fq.OriginalType.Equals(SolrHasValueQuery, System.StringComparison.InvariantCultureIgnoreCase)
                    && !string.IsNullOrWhiteSpace(fq.Field))
                {
                    return new SolrHasValueQuery(fq.Field);
                }

                // SolrQueryByFieldRegex
                if (fq.OriginalType.Equals(SolrQueryByFieldRegex, System.StringComparison.InvariantCultureIgnoreCase)
                    && !string.IsNullOrWhiteSpace(fq.Expression))
                {
                    return new SolrQueryByFieldRegex(fq.FieldName, fq.Expression);
                }

                // Build a "query by" field.

                // If we get this far and the field name is null, don't proceed / return null.
                if (string.IsNullOrWhiteSpace(fq.FieldName))
                {
                    return null;
                }

                var queryByField = new SolrQueryByField(fq.FieldName, fq.FieldValue)
                {
                    Quoted = fq.Quoted
                };

                // Was the original type a "Not" query?
                if (isNotQuery)
                {
                    return new SolrNotQuery(queryByField);
                }

                return queryByField;
            }

            return null;
        }

        /// <summary>
        /// Converts a <see cref="EdismaxSortOrder"/> (JSON object) to a <see cref="SortOrder"/> object.
        /// </summary>
        public static SortOrder ConvertToSolrSortOrder(this EdismaxSortOrder so)
        {
            if (so != null)
            {
                return new SortOrder(so.FieldName, so.Order.ConvertOrder());
            }

            return null;
        }

        /// <summary>
        /// Converts a <see cref="EdismaxOrder"/> to a <see cref="SortOrder"/>
        /// </summary>
        public static Order ConvertOrder(this EdismaxOrder o)
        {
            switch (o)
            {
                case EdismaxOrder.DESC:
                    return Order.DESC;
                default:
                    return Order.ASC;
            }
        }
    }
}