using Nest;

namespace ElasticSearchWebLogs.API.ElasticSearch
{
    public class ElasticSearchServices(ElasticClient client)
    {
        private readonly ElasticClient _client = client;

        public async Task<bool> HealthAsync()
        {
            var response = await _client.Cluster.HealthAsync();
            return response.IsValid;
        }

        /// <summary>
        /// POST kibana_sample_data_logs/_search
        ///{
        ///  "query": {
        ///    "query_string": {
        ///      "query": "error 404"
        ///    }
        ///  }
        ///}
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WebLog>> BasicSearchAsync(string searchQuery)
        {
            var response = await _client.SearchAsync<WebLog>(s => s
                .Query(q => q
                    .QueryString(qs => qs
                        .Query(searchQuery)
                    )
                )
            );

            return response.Documents;
        }

        /// <summary>
        /// POST kibana_sample_data_logs/_search
        ///{
        ///  "from": 0,
        ///  "size": 10,
        ///  "query": {
        ///    "bool": {
        ///      "must": [
        ///        {
        ///          "query_string": {
        ///            "query": "Linux x86_64;",
        ///            "default_field": "message"
        ///          }
        ///}
        ///      ]
        ///    }
        ///  }
        ///}
        /// </summary>
        /// <param name="query"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WebLog>> SearchLogsByMessageAsync(string query, int pageNumber, int pageSize)
        {
            var response = await _client.SearchAsync<WebLog>(s => s
                .Query(q => q
                    .Bool(b => b
                        .Must(m => m
                            .QueryString(qs => qs
                                .Query(query)
                                .DefaultField(f => f.Message)
                            )
                        )
                    )
                )
                .From((pageNumber - 1) * pageSize)
                .Size(pageSize)
            );

            if (response.IsValid && response.Documents.Any())
            {
                return response.Documents;
            }

            return Enumerable.Empty<WebLog>();
        }

        /// <summary>
        /// POST kibana_sample_data_logs/_search
        ///{
        ///  "from": 0,
        ///  "size": 10,
        ///  "query": {
        ///    "match": {
        ///      "message": {
        ///        "query": "223.87.60.27"
        ///      }
        ///    }
        ///  }
        ///}
        /// </summary>
        /// <param name="query"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WebLog>> SearchLogsByMessageWithPagingAsync(string query, int pageNumber, int pageSize)
        {
            var response = await _client.SearchAsync<WebLog>(s => s
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.Message)
                        .Query(query)
                    )
                )
                .From((pageNumber - 1) * pageSize)
                .Size(pageSize)
            );

            return response.Documents;
        }


        public async Task<IEnumerable<WebLog>> SearchLogsByIpAsync(string ip, int size = 10)
        {
            var response = await _client.SearchAsync<WebLog>(s => s
                .Query(q => q
                    .Term(t => t
                        .Field(f => f.Ip.Suffix("keyword"))
                        .Value(ip)
                    )
                )
                .Size(size)
            );

            return response.Documents;
        }

        public async Task<IEnumerable<WebLog>> SearchLogsByDateRangeAsync(DateTime startDate, DateTime endDate, int size = 10)
        {
            var response = await _client.SearchAsync<WebLog>(s => s
                .Query(q => q
                    .DateRange(d => d
                        .Field(f => f.Timestamp)
                        .GreaterThanOrEquals(startDate)
                        .LessThanOrEquals(endDate)
                    )
                )
                .Size(size)
            );

            return response.Documents;
        }


        public async Task<IEnumerable<WebLog>> SearchLogsByResponseAndDateRangeAsync(int responseCode, DateTime startDate, DateTime endDate, int pageNumber, int pageSize)
        {
            var response = await _client.SearchAsync<WebLog>(s => s
                .Query(q => q
                    .Bool(b => b
                        .Must(
                            mu => mu.Term(t => t.Field(f => f.Response.Suffix("keyword")).Value(responseCode)),
                            mu => mu.DateRange(dr => dr
                                .Field(f => f.Timestamp)
                                .GreaterThanOrEquals(startDate)
                                .LessThanOrEquals(endDate)
                            )
                        )
                    )
                )
                .From((pageNumber - 1) * pageSize)
                .Size(pageSize)
            );

            return response.Documents;
        }




        /// <summary>
        ///  POST kibana_sample_data_logs/_search
        ///         {
        ///           "size": 0,
        ///           "aggs": {
        ///             "agg_stats": {
        ///               "stats": {
        ///                 "field": "bytes"
        ///               }
        ///             }
        ///           }
        ///         }
        /// </summary>
        /// <param name="numericField"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, double>> AggregateFieldStatisticsAsync(string numericField)
        {
            var response = await _client.SearchAsync<WebLog>(s => s
                .Size(0) // Döküman döndürmeye gerek yok.
                .Aggregations(a => a
                    .Stats("agg_stats", st => st.Field(numericField))
                )
            );

            var stats = response.Aggregations.Stats("agg_stats");
            var result = new Dictionary<string, double>   {
                    { "min", stats.Min.GetValueOrDefault() },
                    { "max", stats.Max.GetValueOrDefault() },
                    { "avg", stats.Average.GetValueOrDefault() },
                    { "sum", stats.Sum },
                    { "count", stats.Count }
                    };

            return result;

            /* 
                "aggregations": {
                  "agg_stats": {
                    "count": 14074,
                    "min": 0,
                    "max": 19986,
                    "avg": 5664.749822367487,
                    "sum": 79725689
                  }
                }             
            */
        }


    }
}
