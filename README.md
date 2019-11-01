# Sitecore8 eDisMax Search

This project is a simple .Net Framework WebAPI2 proxy website that can be setup to add Solr eDisMax query support in Sitecore 8 (or below).

> Note that Sitecore 9 contains native Edismax Search support via the Query<T>() method. So if you are on Sitecore 9, this project is useless to you - instead, just read my blog series. But if you are on Sitecore 8 and cannot upgrade to version 9 for one reason or another, then you are in the right place.

For more information on working with eDisMax itecore support For more on this, see my blog: [Introduction to Sitecore Solr eDisMax](https://stevenstriga.wordpress.com/2019/03/28/edismax-series-post-1-introduction-to-sitecore-solr-edismax/)

There are two projects in the solution "Website" and "Sitecore8EdismaxSupport"

## "Website" Project
This is the eDisMax proxy website. The `/api/search/search` HTTPPOST endpoint accepts a JSON payload generated by the sitecore website containg the Solr eDisMax query and will pass along the response from Solr in the response also as JSON. It contains no business logic, and I strongly recommend keeping it that way - keep all your business logic in the Sitecore site code.

### Why does this site have classes and models that mirror SolrNet? Why just not use SolrNet nuget?
Sitecore 8 ships with a version of SolrNet that is not available on nuget (0.4.0.2002). It was easier to rebuild some of these simple classes than to accept the risk of using a unsupported Solr.net version in Sitecore.

## "Sitecore8EdismaxSupport" Project
This is a class library project that contains the code to connect to the proxy website.

It contains two important methods:

1. `BuildEdismaxQueryParams(...)`
  This method accepts the eDisMax parameters to build an instance of EDismaxQuery object for you.
2. `CallProxySolrService(EDismaxQuery searchQuery)`
  This method will execute and send the eDisMax query to the proxy website, and then parse the results into a `SolrProxyResultSet` for you.

I recommend that you copy this project directly into your solution in a Helix foundation layer (recommended) so you can modify as you wish.
