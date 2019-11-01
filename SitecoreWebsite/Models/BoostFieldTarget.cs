using System;
using System.Diagnostics;

namespace Sitecore8EdismaxSupport.Models
{
    /// <summary>
    /// Flattened / combined class of a boost field and its target item.
    /// </summary>
    [DebuggerDisplay("FieldName = {FieldName}, FieldValue = {FieldValue}, BoostValue = {BoostValue}")]
    public class BoostFieldTarget
    {
        public string FieldName { get; set; }

        public string FieldValue { get; set; }

        public float BoostValue { get; set; }

        public Guid OriginalId { get; set; }

        public string SolrKey { get; set; }

        public bool IgnoreBoostValue { get; set; }

        public override string ToString()
        {
            if (IgnoreBoostValue)
            {
                return FieldValue;
            }

            // Has name, value and boost == 1 (for 1, to not include the boost value but send the field)
            if (!string.IsNullOrWhiteSpace(FieldName)
                && !string.IsNullOrWhiteSpace(FieldValue)
                && BoostValue == 1)
            {
                return string.Format("{0}:{1}", FieldName, FieldValue);
            }

            // Has name, value and boost != 0
            if (!string.IsNullOrWhiteSpace(FieldName)
                && !string.IsNullOrWhiteSpace(FieldValue)
                && BoostValue != 0)
            {
                return string.Format("{0}:{1}^{2}", FieldName, FieldValue, BoostValue);
            }

            // No field name but a value & boost.
            // e.g.: Recency boost: recip(ms(NOW/HOUR,insightsortdate_tdt),3.16e-11,1,1)
            if (string.IsNullOrWhiteSpace(FieldName)
                && !string.IsNullOrWhiteSpace(FieldValue)
                && BoostValue > 0)
            {
                return string.Format("{0}^{1}", FieldValue, BoostValue);
            }

            // field value only.
            // e.g.: Recency boost: recip(ms(NOW/HOUR,insightsortdate_tdt),3.16e-11,1,1)
            return FieldValue;
        }
    }
}
