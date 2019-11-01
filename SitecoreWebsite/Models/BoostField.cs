using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sitecore8EdismaxSupport.Models
{
    /// <summary>
    /// Represents a boost field. /sitecore/templates/Project/Common/Content Type/Boosts
    /// </summary>
    [DebuggerDisplay("SolrKey = {SolrKey}")]
    public class BoostField
    {
        public IEnumerable<Guid> TargetIds { get; set; }

        public string BoostValue { get; set; }

        public float BoostValueFloat
        {
            get
            {
                float boostVal = 0;

                if (!string.IsNullOrWhiteSpace(BoostValue))
                {
                    float.TryParse(BoostValue, out boostVal);
                }

                return boostVal;
            }
        }

        public string SolrKey { get; set; }

        public override string ToString()
        {
            if (BoostValueFloat != 0 && BoostValueFloat != 1)
            {
                return string.Format("{0}^{1}", SolrKey, BoostValueFloat);
            }

            // No Boost
            return SolrKey;
        }
    }
}
