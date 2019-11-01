using Newtonsoft.Json;
using Sitecore8EdismaxSupport.Models;
using SolrNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Sitecore8EdismaxSupport
{
    public static class EdismaxQueryBuilder
    {
        /// <summary>
        /// Use this method in your sitecore website to build the query for the the Edismax WebAPI site.
        /// </summary>
        public static EDismaxQuery BuildEdismaxQueryParams
        (
            int pageNumber,
            int pageSize,
            List<ISolrQuery> filterQueries,
            string boostItemId,
            List<string> queryFields = null,
            string minimumMatch = null,
            string keyword = null,
            ICollection<SortOrder> orderBy = null,
            IEnumerable<BoostFieldTarget> boostFields = null,
            IEnumerable<BoostFieldTarget> boostFunctions = null,
            IEnumerable<BoostField> phraseBoostFields = null,
            IEnumerable<BoostField> phraseFields2 = null,
            IEnumerable<BoostFieldTarget> boosts = null,
            int? phraseSlop = null,
            bool spellcheck = false,
            bool spellcheckCollate = true,
            string spellcheckDictionary = "spellcheckcomputed",
            string indexName = "sitecore_web_index")
        {
            var edismaxQuery = new EDismaxQuery
            {
                FilterQueries = filterQueries ?? Enumerable.Empty<ISolrQuery>().ToList(),
                IndexName = indexName,
                Start = pageNumber * pageSize,
                Rows = pageSize,
                ExtraParams = new List<KeyValuePair<string, string>>()
            };

            if (queryFields != null && queryFields.Any())
            {
                var queryFieldsString = string.Join(" ", queryFields);
                edismaxQuery.ExtraParams.Add(new KeyValuePair<string, string>("qf", queryFieldsString));
            }

            // ExtraParams: use edismax query
            edismaxQuery.ExtraParams.Add(new KeyValuePair<string, string>("defType", "edismax"));

            // ExtraParams: bq
            var boostQueryString = string.Empty;
            var boostQuerySitecoreString = string.Empty;

            // ExtraParams: bq = "Boost Query": passes as parameter
            if (boostFields != null && boostFields.Any())
            {
                boostQueryString = string.Join(" ", boostFields.Select(b => b.ToString()));
            }

            // ExtraParams: bq = "Boost Query": Finalize the string.
            var finalBoostQueryString = string.Concat(boostQueryString, " ", boostQuerySitecoreString).TrimDuplicateWhitespace();
            if (!string.IsNullOrWhiteSpace(finalBoostQueryString))
            {
                edismaxQuery.ExtraParams.Add(new KeyValuePair<string, string>("bq", finalBoostQueryString));
            }

            // ExtraParams: bf = "boost function"
            if (boostFunctions != null && boostFunctions.Any())
            {
                var boostFunctionString = string.Join(" ", boostFunctions.Select(b => b.ToString()));
                edismaxQuery.ExtraParams.Add(new KeyValuePair<string, string>("bf", boostFunctionString));
            }

            // ExtraParams: "boost"
            var boostFieldTargets = boosts.ToList();
            if (boosts != null && boostFieldTargets.Any())
            {
                var boostsString = string.Join(" ", boostFieldTargets.Select(b => b.ToString()));
                edismaxQuery.ExtraParams.Add(new KeyValuePair<string, string>("boost", boostsString));
            }

            // ExtraParams: Minimum Match
            if (!string.IsNullOrEmpty(minimumMatch))
            {
                edismaxQuery.ExtraParams.Add(new KeyValuePair<string, string>("mm", minimumMatch));
            }
            
            // Search keyword
            edismaxQuery.Keyword = !string.IsNullOrWhiteSpace(keyword) ? keyword : "*:*";

            // Phrase fields
            var phraseFieldsString = phraseBoostFields
                .Where(p => p.BoostValueFloat > 0)
                .Select(p => p.ToString())
                .ToArray();

            var phraseFieldsDelimitedString = string.Join(" ", phraseFieldsString);
            if (!string.IsNullOrWhiteSpace(phraseFieldsDelimitedString))
            {
                edismaxQuery.ExtraParams.Add(new KeyValuePair<string, string>("pf", phraseFieldsDelimitedString));
            }

            // pf2
            var phraseFields2String = phraseFields2
                .Where(p => p.BoostValueFloat > 0)
                .Select(p => p.ToString())
                .ToArray();

            var phraseFields2DelimitedString = string.Join(" ", phraseFields2String);
            if (!string.IsNullOrWhiteSpace(phraseFields2DelimitedString))
            {
                edismaxQuery.ExtraParams.Add(new KeyValuePair<string, string>("pf2", phraseFields2DelimitedString));
            }

            // Phrase Slop
            if (phraseSlop.HasValue)
            {
                edismaxQuery.ExtraParams.Add(new KeyValuePair<string, string>("ps", phraseSlop.Value.ToString()));
            }

            // Order By
            edismaxQuery.OrderBy = orderBy ?? Enumerable.Empty<SortOrder>().ToList();

            // spellcheck
            if (spellcheck)
            {
                edismaxQuery.ExtraParams.Add(new KeyValuePair<string, string>("spellcheck", "true"));

                if (spellcheckCollate)
                {
                    edismaxQuery.ExtraParams.Add(new KeyValuePair<string, string>("spellcheck.collate", "true"));
                }

                if (!string.IsNullOrWhiteSpace(spellcheckDictionary))
                {
                    edismaxQuery.ExtraParams.Add(new KeyValuePair<string, string>("spellcheck.dictionary", spellcheckDictionary));
                }
            }

            return edismaxQuery;
        }

        /// <summary>
        /// Calls the proxy Solr service with the specified query.
        /// </summary>
        public static SolrProxyResultSet<T> CallProxySolrService<T>(EDismaxQuery searchQuery)
             where T : SolrProxyResultItem
        {
            // If the parameter is null, return empty result object.
            if (searchQuery == null)
            {
                return new SolrProxyResultSet<T>
                {
                    Results = Enumerable.Empty<T>().ToArray()
                };
            }

            // TODO: change these variables to load values from Sitecore Patch file.
            var url = "http://sitecore8edismaxsearch/";
            var username = "proxysearch";
            var password = "StZ][7Qk$Vx";
            var svcCredentials = System.Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password));

            // Create a request. 
            var request = WebRequest.Create(url);
            request.ContentType = "application/json";
            request.Method = "POST";
            request.Headers.Add("Authorization", "Basic " + svcCredentials);

            // Convert parameter to JSON.
            var searchQueryJson = SerializeToJson(searchQuery);

            // In the JSON, replace slashes with quote slash, e.g. \ --> \"
            searchQueryJson = searchQueryJson.Replace("\"", "\\\"");

            //  In the JSON, wrap the entire string in quotes.
            searchQueryJson = string.Concat("\"", searchQueryJson, "\"");

            // Set the JSON as the request body.
            var encoding = new UTF8Encoding();
            var bytes = encoding.GetBytes(searchQueryJson);
            request.ContentLength = bytes.Length;

            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(bytes, 0, bytes.Length);
            }

            // Get the response.  
            string responseFromServer;
            using (var response = request.GetResponse())
            {
                // Get the stream containing content returned by the server.  
                var dataStream = response.GetResponseStream();

                if (dataStream == null)
                {
                    throw new InvalidOperationException();
                }

                // Open the stream using a StreamReader for easy access.  
                using (var reader = new StreamReader(dataStream))
                {
                    // Read the content.  
                    responseFromServer = reader.ReadToEnd();
                }
            }

            // Deserialize the JSON and return strongly-typed object.
            return JsonConvert.DeserializeObject<SolrProxyResultSet<T>>(responseFromServer);
        }

        /// <summary>
        /// Serializes the EDismaxQuery to JSON.
        /// </summary>
        private static string SerializeToJson(EDismaxQuery searchQuery)
        {
            // If the parameter is null, return empty JSON object.
            if (searchQuery == null)
            {
                return "{}";
            }

            // Convert to Json, making sure to include the type name of each object (property appears as "$type").
            var searchQueryJson = JsonConvert.SerializeObject(searchQuery, Formatting.None, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            });

            // Replace reserved property name "$type" with "Type" so we can read it as a regular property when de-serializing.
            searchQueryJson = searchQueryJson.Replace("\"$type\":", "\"Type\":");

            return searchQueryJson;
        }

        /// <summary>
        /// Trims duplicate whitespace.
        /// </summary>
        private static string TrimDuplicateWhitespace(this string text)
        {
            const string singleSpace = " ";
            const string doubleSpace = "  ";

            if (string.IsNullOrWhiteSpace(text)) return string.Empty;

            // Remove any duplicate whitespace.
            while (text.Contains(doubleSpace))
            {
                text = text.Replace(doubleSpace, singleSpace);
            }

            return text.Trim();
        }
    }
}
