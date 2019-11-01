using System.Configuration;

namespace Sitecore8EdismaxSearch.Website.Configuration
{
    /// <summary>
    /// Settings stored in the .config file.
    /// </summary>
    public static class AppSettings
    {
        /// <summary>
        /// Gets the base SolrIndex Url.
        /// </summary>
        public static string SolrIndex_BaseUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["SolrIndex_BaseUrl"];
            }
        }

        /// <summary>
        /// Gets index name for the web database.
        /// </summary>
        public static string SolrIndex_Web_IndexName
        {
            get
            {
                return ConfigurationManager.AppSettings["SolrIndex_Web_IndexName"];
            }
        }


        /// <summary>
        /// Gets index name for the liveweb database.
        /// </summary>
        public static string SolrIndex_LiveWeb_IndexName
        {
            get
            {
                return ConfigurationManager.AppSettings["SolrIndex_LiveWeb_IndexName"];
            }
        }

        /// <summary>
        /// Gets the "Authentication_Username" app setting.
        /// </summary>
        public static string Authentication_Username
        {
            get
            {
                return ConfigurationManager.AppSettings["Authentication_Username"];
            }
        }


        /// <summary>
        /// Gets "Authentication_Password" app setting.
        /// </summary>
        public static string Authentication_Password
        {
            get
            {
                return ConfigurationManager.AppSettings["Authentication_Password"];
            }
        }


    }
}