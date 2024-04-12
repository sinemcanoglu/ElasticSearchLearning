using ElasticSearchWebLogs.API.ElasticSearch;
using Microsoft.AspNetCore.Mvc;

namespace ElasticSearchWebLogs.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class WebLogsController : ControllerBase
    {
        private readonly ElasticSearchServices _elasticSearchClient;

        public WebLogsController(ElasticSearchServices elasticSearchClient)
        {
            _elasticSearchClient = elasticSearchClient;
        }

        [HttpGet("health")]
        public async Task<ActionResult> HealthAsync()
        {
            return await _elasticSearchClient.HealthAsync() ? Ok() : NotFound();
        }

        [HttpGet("search/byResponseAndDateRange")]
        public async Task<IActionResult> SearchLogsByResponseAndDateRangeAsync([FromQuery] int responseCode, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (startDate > endDate)           
                return BadRequest("Start date must be before end date.");
            

            if (pageSize <= 0)           
                return BadRequest("Page size must be greater than 0.");
            

            if (pageNumber <= 0)            
                return BadRequest("Page number must be greater than 0.");
            

            var data = await _elasticSearchClient.SearchLogsByResponseAndDateRangeAsync(responseCode, startDate, endDate, pageNumber, pageSize);
            if (data == null || !data.Any())
            {
                return NotFound("No logs found matching the criteria.");
            }

            return Ok(data);
        }


        // İstatistik Agregasyonu
        [HttpGet("aggregate/stats")]
        public async Task<ActionResult<Dictionary<string, double>>> AggregateFieldStatistics(string numericField)
        {
            var results = await _elasticSearchClient.AggregateFieldStatisticsAsync(numericField);
            return Ok(results);
        }
    }
}
