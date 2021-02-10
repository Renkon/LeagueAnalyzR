using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LeagueAnalyzR.Model;
using LeagueAnalyzR.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RiotSharp.Endpoints.MatchEndpoint;
using RiotSharp.Endpoints.SummonerEndpoint;
using RiotSharp.Misc;

namespace LeagueAnalyzR.Controllers
{
    [ApiController]
    [Route("secondtry")]
    public class RiotController : ControllerBase
    {
        private readonly IRiotService riotService;
        private readonly ILogger<RiotController> logger;

        public RiotController(ILogger<RiotController> logger, IRiotService riotService)
        {
            this.logger = logger;
            this.riotService = riotService;
        }

        [HttpGet]
        [Route("region/{region}/summoner/{name}/historico/completo/excel")]
        public async Task<ActionResult<IEnumerable<CompleteMatch>>> GetCompleteMatchListAsExcelAsync(Region region, string name, [FromQuery(Name = "desde")] DateTime? from, [FromQuery(Name = "hasta")] DateTime? to)
        {
            var excelPackage = await riotService.GetCompleteHistoricalGamesProcessedAsync(from, to, region, name);
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Headers.Add("content-disposition", $"attachment; filename={name}.xlsx");
            await Response.Body.WriteAsync(excelPackage.GetAsByteArray());
            return Ok();
        }
    }
}
