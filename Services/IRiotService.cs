using LeagueAnalyzR.Model;
using OfficeOpenXml;
using RiotSharp.Endpoints.MatchEndpoint;
using RiotSharp.Endpoints.SummonerEndpoint;
using RiotSharp.Misc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeagueAnalyzR.Services
{
    public interface IRiotService
    {
        Task<Summoner> GetSummonerByNameAsync(Region region, string name);

        Task<MatchList> GetHistoricalMatchListAsync(DateTime? from, DateTime? to, Region region, string name);

        Task<MatchTimeline> GetHistoricalMatchTimelineAsync(Region region, long matchId);

        Task<Match> GetHistoricalMatchAsync(Region region, long matchId);

        Task<IEnumerable<CompleteMatch>> GetCompleteHistoricalGamesAsync(DateTime? from, DateTime? to, Region region, string name);

        Task<ExcelPackage> GetCompleteHistoricalGamesProcessedAsync(DateTime? from, DateTime? to, Region region, string name);
    }
}
