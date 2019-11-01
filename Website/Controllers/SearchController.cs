using log4net;
using Newtonsoft.Json;
using Sitecore8EdismaxSearch.Website.Filters;
using Sitecore8EdismaxSearch.Website.Models;
using System.Web.Http;
using Sitecore8EdismaxSearch.Website.Services.SearchRepository.Services;

namespace Sitecore8EdismaxSearch.Website.Controllers
{
    [BasicAuthentication]
    public class SearchController : ApiController
    {
        private static ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ISearchRepository _searchRepository;

        /// <summary>
        /// Creates a new instance of the <see cref="SearchController"/> class.
        /// </summary>
        public SearchController(ISearchRepository searchRepository)
        {
            _searchRepository = searchRepository;
        }

        /// <summary>
        /// Searches Solr with an EDismax query and returns a generic result set.
        /// </summary>        
        [HttpPost]
        public IHttpActionResult Search([FromBody] string jsonText)
        {
            _log.DebugFormat("Search(): {0}", jsonText);

            // Deserialize the Request body into a EDismaxQuery instance.
            var edisMax = JsonConvert.DeserializeObject<EDismaxQuery>(jsonText);

            // Execute the query.
            SolrProxyResultSet result = _searchRepository.Execute<SearchResultItemWeb, SearchResultItemLiveWeb>(edisMax);

            // Serialize the header of the result set into the log.
            _log.DebugFormat("Search result: {0}", JsonConvert.SerializeObject(result.Header));

            // Return as JSON.
            return Json(result);
        }
    }
}
