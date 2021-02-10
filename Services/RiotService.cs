using LeagueAnalyzR.Model;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using RiotSharp;
using RiotSharp.Endpoints.Interfaces.Static;
using RiotSharp.Endpoints.MatchEndpoint;
using RiotSharp.Endpoints.MatchEndpoint.Enums;
using RiotSharp.Endpoints.StaticDataEndpoint;
using RiotSharp.Endpoints.StaticDataEndpoint.Champion;
using RiotSharp.Endpoints.SummonerEndpoint;
using RiotSharp.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeagueAnalyzR.Services
{
    public class RiotService : IRiotService
    {
        private readonly RiotApi ApiInstance;
        private readonly IStaticEndpointProvider StaticEndpointProvider;
        private ChampionListStatic ChampionList;
        public RiotService(IStaticEndpointProvider staticEndpointProvider)
        {
            ApiInstance = RiotApi.GetDevelopmentInstance("RGAPI-xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
            StaticEndpointProvider = staticEndpointProvider;
        }

        public async Task<Summoner> GetSummonerByNameAsync(Region region, string name)
        {
            return await ApiInstance.Summoner.GetSummonerByNameAsync(region, name);
        }

        public async Task<MatchList> GetHistoricalMatchListAsync(DateTime? from, DateTime? to, Region region, string name)
        {
            if (!from.HasValue && !to.HasValue)
            {
                to = DateTime.Now.Date;
                from = to.Value.Subtract(new TimeSpan(7, 0, 0, 0));
            }
            else if (!from.HasValue)
            {
                from = to.Value.Subtract(new TimeSpan(7, 0, 0, 0));
            }
            else if (!to.HasValue)
            {
                to = from.Value.Add(new TimeSpan(7, 0, 0, 0));
            }

            var rankedQueue = 420;
            var summoner = await GetSummonerByNameAsync(region, name);
            return await ApiInstance.Match.GetMatchListAsync(region, summoner.AccountId, null, new List<int> { rankedQueue }, null, from, to);
        }

        public async Task<MatchTimeline> GetHistoricalMatchTimelineAsync(Region region, long matchId)
        {
            return await ApiInstance.Match.GetMatchTimelineAsync(region, matchId);
        }

        public async Task<Match> GetHistoricalMatchAsync(Region region, long matchId)
        {
            return await ApiInstance.Match.GetMatchAsync(region, matchId);
        }

        public async Task<IEnumerable<CompleteMatch>> GetCompleteHistoricalGamesAsync(DateTime? from, DateTime? to, Region region, string name)
        {
            if (ChampionList == default(ChampionListStatic))
            {
                ChampionList = await ((StaticEndpointProvider)StaticEndpointProvider).GetEndpoint<IStaticChampionEndpoint>().GetAllAsync("10.8.1", Language.es_MX);
            }

            var matches = (await GetHistoricalMatchListAsync(from, to, region, name)).Matches;
            var completeMatches = new List<CompleteMatch>();

            foreach (var match in matches)
            {
                completeMatches.Add(new CompleteMatch
                {
                    MatchReference = match,
                    Match = await GetHistoricalMatchAsync(match.Region, match.GameId),
                    MatchTimeline = await GetHistoricalMatchTimelineAsync(match.Region, match.GameId)
                });
            }

            return completeMatches;
        }

        public async Task<ExcelPackage> GetCompleteHistoricalGamesProcessedAsync(DateTime? from, DateTime? to, Region region, string name)
        {
            var headers = new Dictionary<string, string>
            {
                ["A"] = "GameID",
                ["B"] = "Día y hora (UTC)",
                ["C"] = "Duración",
                ["D"] = "Lado",
                ["E"] = "Resultado",
                ["F"] = "Rol",
                ["G"] = "Campeón",
                ["H"] = "Campeón enemigo",
                ["I"] = "Asesinatos",
                ["J"] = "Muertes",
                ["K"] = "Asistencias",
                ["L"] = "KDA",
                ["M"] = "KP%",
                ["N"] = "KP% enemigo",
                ["O"] = "Oro total",
                ["P"] = "Oro total enemigo",
                ["Q"] = "Diferencia de oro",
                ["R"] = "Nivel",
                ["S"] = "Nivel enemigo",
                ["T"] = "Diferencia de nivel",
                ["U"] = "Farm en línea",
                ["V"] = "Farm en jg",
                ["W"] = "Farm en jg aliada",
                ["X"] = "Farm en jg enemiga",
                ["Y"] = "Farm total",
                ["Z"] = "CS/Min",
                ["AA"] = "Centinelas usados",
                ["AB"] = "Centinelas destruídos",
                ["AC"] = "Pinks comprados",
                ["AD"] = "Wards usados/Min",
                ["AE"] = "Puntos de visión",
                ["AF"] = "Oro @ 5:00",
                ["AG"] = "Oro enemigo @ 5:00",
                ["AH"] = "Dif. de Oro @ 5:00",
                ["AI"] = "Nivel @ 5:00",
                ["AJ"] = "Nivel enemigo @ 5:00",
                ["AK"] = "Dif. de Nivel @ 5:00",
                ["AL"] = "Farm @ 5:00",
                ["AM"] = "Farm enemigo @ 5:00",
                ["AN"] = "Dif. de Farm @ 5:00",
                ["AO"] = "Oro @ 8:00",
                ["AP"] = "Oro enemigo @ 8:00",
                ["AQ"] = "Dif. de Oro @ 8:00",
                ["AR"] = "Nivel @ 8:00",
                ["AS"] = "Nivel enemigo @ 8:00",
                ["AT"] = "Dif. de Nivel @ 8:00",
                ["AU"] = "Farm @ 8:00",
                ["AV"] = "Farm enemigo @ 8:00",
                ["AW"] = "Dif. de Farm @ 8:00",
                ["AX"] = "Oro @ 12:00",
                ["AY"] = "Oro enemigo @ 12:00",
                ["AZ"] = "Dif. de Oro @ 12:00",
                ["BA"] = "Nivel @ 12:00",
                ["BB"] = "Nivel enemigo @ 12:00",
                ["BC"] = "Dif. de Nivel @ 12:00",
                ["BD"] = "Farm @ 12:00",
                ["BE"] = "Farm enemigo @ 12:00",
                ["BF"] = "Dif. de Farm @ 12:00",
                ["BG"] = "Oro @ 15:00",
                ["BH"] = "Oro enemigo @ 15:00",
                ["BI"] = "Dif. de Oro @ 15:00",
                ["BJ"] = "Nivel @ 15:00",
                ["BK"] = "Nivel enemigo @ 15:00",
                ["BL"] = "Dif. de Nivel @ 15:00",
                ["BM"] = "Farm @ 15:00",
                ["BN"] = "Farm enemigo @ 15:00",
                ["BO"] = "Dif. de Farm @ 15:00",
                ["BP"] = "Oro @ 20:00",
                ["BQ"] = "Oro enemigo @ 20:00",
                ["BR"] = "Dif. de Oro @ 20:00",
                ["BS"] = "Nivel @ 20:00",
                ["BT"] = "Nivel enemigo @ 20:00",
                ["BU"] = "Dif. de Nivel @ 20:00",
                ["BV"] = "Farm @ 20:00",
                ["BW"] = "Farm enemigo @ 20:00",
                ["BX"] = "Dif. de Farm @ 20:00",
                ["BY"] = "Oro @ 25:00",
                ["BZ"] = "Oro enemigo @ 25:00",
                ["CA"] = "Dif. de Oro @ 25:00",
                ["CB"] = "Nivel @ 25:00",
                ["CC"] = "Nivel enemigo @ 25:00",
                ["CD"] = "Dif. de Nivel @ 25:00",
                ["CE"] = "Farm @ 25:00",
                ["CF"] = "Farm enemigo @ 25:00",
                ["CG"] = "Dif. de Farm @ 25:00",
                ["CH"] = "Oro @ 30:00",
                ["CI"] = "Oro enemigo @ 30:00",
                ["CJ"] = "Dif. de Oro @ 30:00",
                ["CK"] = "Nivel @ 30:00",
                ["CL"] = "Nivel enemigo @ 30:00",
                ["CM"] = "Dif. de Nivel @ 30:00",
                ["CN"] = "Farm @ 30:00",
                ["CO"] = "Farm enemigo @ 30:00",
                ["CP"] = "Dif. de Farm @ 30:00",
                ["CQ"] = "Oro @ 40:00",
                ["CR"] = "Oro enemigo @ 40:00",
                ["CS"] = "Dif. de Oro @ 40:00",
                ["CT"] = "Nivel @ 40:00",
                ["CU"] = "Nivel enemigo @ 40:00",
                ["CV"] = "Dif. de Nivel @ 40:00",
                ["CW"] = "Farm @ 40:00",
                ["CX"] = "Farm enemigo @ 40:00",
                ["CY"] = "Dif. de Farm @ 40:00",
            };
            var funcs = new Dictionary<string, Func<ProcessedMatch, object>>
            {
                ["A"] = (ProcessedMatch game) => game.GameId,
                ["B"] = (ProcessedMatch game) => game.CreatedOn,
                ["C"] = (ProcessedMatch game) => game.Duration,
                ["D"] = (ProcessedMatch game) => game.Side,
                ["E"] = (ProcessedMatch game) => game.Result,
                ["F"] = (ProcessedMatch game) => game.Role,
                ["G"] = (ProcessedMatch game) => game.Champion,
                ["H"] = (ProcessedMatch game) => game.EnemyChampion,
                ["I"] = (ProcessedMatch game) => game.Kills,
                ["J"] = (ProcessedMatch game) => game.Deaths,
                ["K"] = (ProcessedMatch game) => game.Assists,
                ["L"] = (ProcessedMatch game) => game.Kda,
                ["M"] = (ProcessedMatch game) => game.KillParticipation,
                ["N"] = (ProcessedMatch game) => game.EnemyKillParticipation,
                ["O"] = (ProcessedMatch game) => game.Gold,
                ["P"] = (ProcessedMatch game) => game.EnemyGold,
                ["Q"] = (ProcessedMatch game) => game.GoldDifference,
                ["R"] = (ProcessedMatch game) => game.Level,
                ["S"] = (ProcessedMatch game) => game.EnemyLevel,
                ["T"] = (ProcessedMatch game) => game.LevelDifference,
                ["U"] = (ProcessedMatch game) => game.MinionsFarm,
                ["V"] = (ProcessedMatch game) => game.JungleFarm,
                ["W"] = (ProcessedMatch game) => game.AllyJungleFarm,
                ["X"] = (ProcessedMatch game) => game.EnemyJungleFarm,
                ["Y"] = (ProcessedMatch game) => game.TotalFarm,
                ["Z"] = (ProcessedMatch game) => game.FarmPerMinute,
                ["AA"] = (ProcessedMatch game) => game.WardsPlaced,
                ["AB"] = (ProcessedMatch game) => game.WardsDestroyed,
                ["AC"] = (ProcessedMatch game) => game.ControlWardsBought,
                ["AD"] = (ProcessedMatch game) => game.WardsPerMinute,
                ["AE"] = (ProcessedMatch game) => game.VisionPoints,
                ["AF"] = (ProcessedMatch game) => game.GoldAtMinute5,
                ["AG"] = (ProcessedMatch game) => game.EnemyGoldAtMinute5,
                ["AH"] = (ProcessedMatch game) => game.GoldDifferenceAtMinute5,
                ["AI"] = (ProcessedMatch game) => game.LevelAtMinute5,
                ["AJ"] = (ProcessedMatch game) => game.EnemyLevelAtMinute5,
                ["AK"] = (ProcessedMatch game) => game.LevelDifferenceAtMinute5,
                ["AL"] = (ProcessedMatch game) => game.FarmAtMinute5,
                ["AM"] = (ProcessedMatch game) => game.EnemyFarmAtMinute5,
                ["AN"] = (ProcessedMatch game) => game.FarmDifferenceAtMinute5,
                ["AO"] = (ProcessedMatch game) => game.GoldAtMinute8,
                ["AP"] = (ProcessedMatch game) => game.EnemyGoldAtMinute8,
                ["AQ"] = (ProcessedMatch game) => game.GoldDifferenceAtMinute8,
                ["AR"] = (ProcessedMatch game) => game.LevelAtMinute8,
                ["AS"] = (ProcessedMatch game) => game.EnemyLevelAtMinute8,
                ["AT"] = (ProcessedMatch game) => game.LevelDifferenceAtMinute8,
                ["AU"] = (ProcessedMatch game) => game.FarmAtMinute8,
                ["AV"] = (ProcessedMatch game) => game.EnemyFarmAtMinute8,
                ["AW"] = (ProcessedMatch game) => game.FarmDifferenceAtMinute8,
                ["AX"] = (ProcessedMatch game) => game.GoldAtMinute12,
                ["AY"] = (ProcessedMatch game) => game.EnemyGoldAtMinute12,
                ["AZ"] = (ProcessedMatch game) => game.GoldDifferenceAtMinute12,
                ["BA"] = (ProcessedMatch game) => game.LevelAtMinute12,
                ["BB"] = (ProcessedMatch game) => game.EnemyLevelAtMinute12,
                ["BC"] = (ProcessedMatch game) => game.LevelDifferenceAtMinute12,
                ["BD"] = (ProcessedMatch game) => game.FarmAtMinute12,
                ["BE"] = (ProcessedMatch game) => game.EnemyFarmAtMinute12,
                ["BF"] = (ProcessedMatch game) => game.FarmDifferenceAtMinute12,
                ["BG"] = (ProcessedMatch game) => game.GoldAtMinute15,
                ["BH"] = (ProcessedMatch game) => game.EnemyGoldAtMinute15,
                ["BI"] = (ProcessedMatch game) => game.GoldDifferenceAtMinute15,
                ["BJ"] = (ProcessedMatch game) => game.LevelAtMinute15,
                ["BK"] = (ProcessedMatch game) => game.EnemyLevelAtMinute15,
                ["BL"] = (ProcessedMatch game) => game.LevelDifferenceAtMinute15,
                ["BM"] = (ProcessedMatch game) => game.FarmAtMinute15,
                ["BN"] = (ProcessedMatch game) => game.EnemyFarmAtMinute15,
                ["BO"] = (ProcessedMatch game) => game.FarmDifferenceAtMinute15,
                ["BP"] = (ProcessedMatch game) => game.GoldAtMinute20,
                ["BQ"] = (ProcessedMatch game) => game.EnemyGoldAtMinute20,
                ["BR"] = (ProcessedMatch game) => game.GoldDifferenceAtMinute20,
                ["BS"] = (ProcessedMatch game) => game.LevelAtMinute20,
                ["BT"] = (ProcessedMatch game) => game.EnemyLevelAtMinute20,
                ["BU"] = (ProcessedMatch game) => game.LevelDifferenceAtMinute20,
                ["BV"] = (ProcessedMatch game) => game.FarmAtMinute20,
                ["BW"] = (ProcessedMatch game) => game.EnemyFarmAtMinute20,
                ["BX"] = (ProcessedMatch game) => game.FarmDifferenceAtMinute20,
                ["BY"] = (ProcessedMatch game) => game.GoldAtMinute25,
                ["BZ"] = (ProcessedMatch game) => game.EnemyGoldAtMinute25,
                ["CA"] = (ProcessedMatch game) => game.GoldDifferenceAtMinute25,
                ["CB"] = (ProcessedMatch game) => game.LevelAtMinute25,
                ["CC"] = (ProcessedMatch game) => game.EnemyLevelAtMinute25,
                ["CD"] = (ProcessedMatch game) => game.LevelDifferenceAtMinute25,
                ["CE"] = (ProcessedMatch game) => game.FarmAtMinute25,
                ["CF"] = (ProcessedMatch game) => game.EnemyFarmAtMinute25,
                ["CG"] = (ProcessedMatch game) => game.FarmDifferenceAtMinute25,
                ["CH"] = (ProcessedMatch game) => game.GoldAtMinute30,
                ["CI"] = (ProcessedMatch game) => game.EnemyGoldAtMinute30,
                ["CJ"] = (ProcessedMatch game) => game.GoldDifferenceAtMinute30,
                ["CK"] = (ProcessedMatch game) => game.LevelAtMinute30,
                ["CL"] = (ProcessedMatch game) => game.EnemyLevelAtMinute30,
                ["CM"] = (ProcessedMatch game) => game.LevelDifferenceAtMinute30,
                ["CN"] = (ProcessedMatch game) => game.FarmAtMinute30,
                ["CO"] = (ProcessedMatch game) => game.EnemyFarmAtMinute30,
                ["CP"] = (ProcessedMatch game) => game.FarmDifferenceAtMinute30,
                ["CQ"] = (ProcessedMatch game) => game.GoldAtMinute40,
                ["CR"] = (ProcessedMatch game) => game.EnemyGoldAtMinute40,
                ["CS"] = (ProcessedMatch game) => game.GoldDifferenceAtMinute40,
                ["CT"] = (ProcessedMatch game) => game.LevelAtMinute40,
                ["CU"] = (ProcessedMatch game) => game.EnemyLevelAtMinute40,
                ["CV"] = (ProcessedMatch game) => game.LevelDifferenceAtMinute40,
                ["CW"] = (ProcessedMatch game) => game.FarmAtMinute40,
                ["CX"] = (ProcessedMatch game) => game.EnemyFarmAtMinute40,
                ["CY"] = (ProcessedMatch game) => game.FarmDifferenceAtMinute40,
            };
            var additionalModifiers = new Dictionary<string, Action<ProcessedMatch, ExcelRange>>
            {
                ["A"] = (game, cell) => 
                {
                    cell.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(64, 64, 255));
                    cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                    cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 212, 212));
                    cell.Hyperlink = new Uri(game.MatchUrl);
                },
                ["B"] = (game, cell) =>
                {
                    cell.Style.Numberformat.Format = "dd/mm/yyyy hh:mm:ss";
                    cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(212, 255, 253));
                },
                ["C"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(212, 255, 253)),
                ["D"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(game.Side == "Rojo" ?
                        System.Drawing.Color.FromArgb(255, 100, 100) : System.Drawing.Color.FromArgb(100, 100, 255)),
                ["E"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(game.Result == "Derrota" ?
                        System.Drawing.Color.FromArgb(255, 50, 50) : System.Drawing.Color.FromArgb(50, 255, 50)),
                ["F"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 250, 207)),
                ["G"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(207, 237, 255)),
                ["H"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(207, 237, 255)),
                ["I"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 224, 178)),
                ["J"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 224, 178)),
                ["K"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 224, 178)),
                ["L"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 204, 128)),
                ["M"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 204, 128)),
                ["N"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 204, 128)),
                ["O"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 236, 179)),
                ["P"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 236, 179)),
                ["Q"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 224, 130)),
                ["R"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(207, 216, 220)),
                ["S"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(207, 216, 220)),
                ["T"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(176, 190, 197)),
                ["U"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(209, 196, 233)),
                ["V"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(209, 196, 233)),
                ["W"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(209, 196, 233)),
                ["X"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(209, 196, 233)),
                ["Y"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(179, 157, 219)),
                ["Z"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(244, 143, 177)),
                ["AA"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(215, 204, 200)),
                ["AB"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(215, 204, 200)),
                ["AC"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(215, 204, 200)),
                ["AD"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(188, 170, 164)),
                ["AE"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(188, 170, 164)),
                ["AF"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 236, 179)),
                ["AG"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 236, 179)),
                ["AH"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 224, 130)),
                ["AI"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(207, 216, 220)),
                ["AJ"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(207, 216, 220)),
                ["AK"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(176, 190, 197)),
                ["AL"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(209, 196, 233)),
                ["AM"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(209, 196, 233)),
                ["AN"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(179, 157, 219)),
                ["AO"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 236, 179)),
                ["AP"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 236, 179)),
                ["AQ"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 224, 130)),
                ["AR"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(207, 216, 220)),
                ["AS"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(207, 216, 220)),
                ["AT"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(176, 190, 197)),
                ["AU"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(209, 196, 233)),
                ["AV"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(209, 196, 233)),
                ["AW"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(179, 157, 219)),
                ["AX"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 236, 179)),
                ["AY"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 236, 179)),
                ["AZ"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 224, 130)),
                ["BA"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(207, 216, 220)),
                ["BB"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(207, 216, 220)),
                ["BC"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(176, 190, 197)),
                ["BD"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(209, 196, 233)),
                ["BE"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(209, 196, 233)),
                ["BF"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(179, 157, 219)),
                ["BG"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 236, 179)),
                ["BH"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 236, 179)),
                ["BI"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 224, 130)),
                ["BJ"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(207, 216, 220)),
                ["BK"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(207, 216, 220)),
                ["BL"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(176, 190, 197)),
                ["BM"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(209, 196, 233)),
                ["BN"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(209, 196, 233)),
                ["BO"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(179, 157, 219)),
                ["BP"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 236, 179)),
                ["BQ"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 236, 179)),
                ["BR"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 224, 130)),
                ["BS"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(207, 216, 220)),
                ["BT"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(207, 216, 220)),
                ["BU"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(176, 190, 197)),
                ["BV"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(209, 196, 233)),
                ["BW"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(209, 196, 233)),
                ["BX"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(179, 157, 219)),
                ["BY"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 236, 179)),
                ["BZ"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 236, 179)),
                ["CA"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 224, 130)),
                ["CB"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(207, 216, 220)),
                ["CC"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(207, 216, 220)),
                ["CD"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(176, 190, 197)),
                ["CE"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(209, 196, 233)),
                ["CF"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(209, 196, 233)),
                ["CG"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(179, 157, 219)),
                ["CH"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 236, 179)),
                ["CI"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 236, 179)),
                ["CJ"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 224, 130)),
                ["CK"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(207, 216, 220)),
                ["CL"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(207, 216, 220)),
                ["CM"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(176, 190, 197)),
                ["CN"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(209, 196, 233)),
                ["CO"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(209, 196, 233)),
                ["CP"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(179, 157, 219)),
                ["CQ"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 236, 179)),
                ["CR"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 236, 179)),
                ["CS"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 224, 130)),
                ["CT"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(207, 216, 220)),
                ["CU"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(207, 216, 220)),
                ["CV"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(176, 190, 197)),
                ["CW"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(209, 196, 233)),
                ["CX"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(209, 196, 233)),
                ["CY"] = (game, cell) => cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(179, 157, 219)),
            };
            var completeGames = await GetCompleteHistoricalGamesAsync(from, to, region, name);
            var excelPackage = new ExcelPackage();
            var processedGames = Process(completeGames, region, name);
            var sheet = excelPackage.Workbook.Worksheets.Add("Estadisticas");

            // Headers
            foreach (var header in headers)
            {
                var cell = sheet.Cells[$"{header.Key}1"];
                cell.Value = header.Value;
                cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            }

            int row = 2;
            foreach (var game in processedGames)
            {
                foreach (var func in funcs)
                {
                    var cell = sheet.Cells[$"{func.Key}{row}"];
                    cell.Value = func.Value(game);
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    cell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    if (additionalModifiers.ContainsKey(func.Key))
                    {
                        additionalModifiers[func.Key](game, cell);
                    }
                }
                row++;
            }

            sheet.Cells.AutoFitColumns();

            return excelPackage;
        }

        private string GetRoleOrDefault(string lane, string role)
        {
            var roles = new Dictionary<(string, string), string>
            {
                [("MIDDLE", "SOLO")] = "MID",
                [("TOP", "SOLO")] = "TOP",
                [("JUNGLE", "NONE")] = "JUNGLA",
                [("BOTTOM", "DUO_CARRY")] = "ADC",
                [("BOTTOM", "DUO_SUPPORT")] = "SUPP",
                [("NONE", "DUO_SUPPORT")] = "SUPP",
                [("NONE", "DUO_CARRY")] = "ADC",
                [("NONE", "NONE")] = "JUNGLA",
                [("MIDDLE", "DUO_CARRY")] = "ADC",
                [("MIDDLE", "DUO_SUPPORT")] = "SUPP",
                [("TOP", "DUO_CARRY")] = "ADC",
                [("TOP", "DUO_SUPPORT")] = "SUPP",
            };

            if (roles.ContainsKey((lane, role)))
            {
                return roles[(lane, role)];
            }

            return "UNKNOWN (enemigo random)";
        }

        private IEnumerable<ProcessedMatch> Process(IEnumerable<CompleteMatch> matches, Region region, string summonerName)
        {
            
            List<ProcessedMatch> processedMatches = new List<ProcessedMatch>();

            foreach (var match in matches)
            {
                var matchUrl = $"https://matchhistory.{region}.leagueoflegends.com/es/#match-details/{getPlatform(region)}/{match.Match.GameId}";
                var myParticipantId = match.Match.ParticipantIdentities.First(pi => pi.Player.SummonerName == summonerName).ParticipantId;
                var myParticipant = match.Match.Participants.First(p => p.ParticipantId == myParticipantId);
                var mySide = match.Match.Participants.First(p => p.ParticipantId == myParticipantId).TeamId;
                var myResult = match.Match.Teams.First(t => t.TeamId == mySide).Win;
                var myRole = myParticipant.Timeline.Role;
                var myLane = myParticipant.Timeline.Lane;
                var myTeamParticipantIds = match.Match.Participants.Where(p => p.TeamId == mySide).Select(p => p.ParticipantId);
                var enemyTeamParticipantIds = match.Match.Participants.Where(p => p.TeamId != mySide).Select(p => p.ParticipantId);
                var enemyParticipant = match.Match.Participants.FirstOrDefault(p => GetRoleOrDefault(p.Timeline.Lane, p.Timeline.Role) == GetRoleOrDefault(myLane, myRole) && p.ParticipantId != myParticipantId && p.TeamId != mySide);
                if (enemyParticipant == default)
                {
                    // Getting random element if not found
                    var enemies = match.Match.Participants.Where(p => p.TeamId != mySide);
                    enemyParticipant = enemies.ElementAt(new Random().Next(enemies.Count()));
                }
                var myTimelineFrames = match.MatchTimeline.Frames.Select(f => f.ParticipantFrames.Where(f => f.Value.ParticipantId == myParticipantId).Select(f => f.Value)).SelectMany(s => s);
                var enemyTimelineFrames = match.MatchTimeline.Frames.Select(f => f.ParticipantFrames.Where(f => f.Value.ParticipantId == enemyParticipant.ParticipantId).Select(f => f.Value)).SelectMany(s => s);
                var myKP = CalculateKP(match.MatchTimeline.Frames, myTeamParticipantIds, myParticipantId);
                var enemyKP = CalculateKP(match.MatchTimeline.Frames, enemyTeamParticipantIds, enemyParticipant.ParticipantId);

                processedMatches.Add(new ProcessedMatch
                {
                    GameId = match.Match.GameId,
                    MatchUrl = matchUrl,
                    CreatedOn = match.Match.GameCreation,
                    _Duration = match.Match.GameDuration,
                    Side = mySide == 100 ? "Azul" : "Rojo",
                    Result = myResult == "Win" ? "Victoria" : "Derrota",
                    Role = GetRoleOrDefault(myLane, myRole),
                    Champion = ChampionList.Champions[ChampionList.Keys[myParticipant.ChampionId]].Name,
                    EnemyChampion = ChampionList.Champions[ChampionList.Keys[enemyParticipant.ChampionId]].Name,
                    Kills = myParticipant.Stats.Kills,
                    Deaths = myParticipant.Stats.Deaths,
                    Assists = myParticipant.Stats.Assists,
                    _Kda = (myParticipant.Stats.Deaths > 0) ? (myParticipant.Stats.Kills + myParticipant.Stats.Assists) / (double)myParticipant.Stats.Deaths : double.MaxValue,
                    _KillParticipation = myKP,
                    _EnemyKillParticipation = enemyKP,
                    Gold = myParticipant.Stats.GoldEarned,
                    EnemyGold = enemyParticipant.Stats.GoldEarned,
                    MinionsFarm = myParticipant.Stats.TotalMinionsKilled,
                    JungleFarm = myParticipant.Stats.NeutralMinionsKilled,
                    AllyJungleFarm = myParticipant.Stats.NeutralMinionsKilledJungle,
                    EnemyJungleFarm = myParticipant.Stats.NeutralMinionsKilledEnemyJungle,
                    WardsPlaced = myParticipant.Stats.WardsPlaced,
                    WardsDestroyed = myParticipant.Stats.WardsKilled,
                    ControlWardsBought = myParticipant.Stats.VisionWardsBoughtInGame,
                    VisionPoints = myParticipant.Stats.VisionScore,
                    _Level = GetLevelFromXP(myTimelineFrames.Last().XP),
                    _EnemyLevel = GetLevelFromXP(enemyTimelineFrames.Last().XP),
                    _GoldAtMinute = myTimelineFrames.Take(myTimelineFrames.Count() - 1).Select(t => t.TotalGold).ToList(),
                    _EnemyGoldAtMinute = enemyTimelineFrames.Take(enemyTimelineFrames.Count() - 1).Select(t => t.TotalGold).ToList(),
                    _LevelAtMinute = myTimelineFrames.Take(myTimelineFrames.Count() - 1).Select(t => t.XP).Select(xp => GetLevelFromXP(xp)).ToList(),
                    _EnemyLevelAtMinute = enemyTimelineFrames.Take(enemyTimelineFrames.Count() - 1).Select(t => t.XP).Select(xp => GetLevelFromXP(xp)).ToList(),
                    _MinionsAtMinute = myTimelineFrames.Take(myTimelineFrames.Count() - 1).Select(t => t.MinionsKilled).ToList(),
                    _JungleMinionsAtMinute = myTimelineFrames.Take(myTimelineFrames.Count() - 1).Select(t => t.JungleMinionsKilled).ToList(),
                    _EnemyMinionsAtMinute = enemyTimelineFrames.Take(enemyTimelineFrames.Count() - 1).Select(t => t.MinionsKilled).ToList(),
                    _EnemyJungleMinionsAtMinute = enemyTimelineFrames.Take(enemyTimelineFrames.Count() - 1).Select(t => t.JungleMinionsKilled).ToList(),
                });
            }

            return processedMatches;
        }

        private double CalculateKP(IEnumerable<MatchFrame> frames, IEnumerable<int> myTeamIds, int myId)
        {
            var teamKills = 0;
            var myParticipations = 0;

            foreach (var frame in frames)
            {
                foreach (var evnt in frame.Events)
                {
                    if (evnt.EventType == MatchEventType.ChampionKill && myTeamIds.Contains(evnt.KillerId))
                    {
                        teamKills++;
                        if (evnt.KillerId == myId || evnt.AssistingParticipantIds.Contains(myId))
                        {
                            myParticipations++;
                        }
                    }
                }
            }

            if (teamKills == 0)
            {
                return double.MaxValue;
            }

            return (myParticipations / (double)teamKills) * 100;
        }

        private string getPlatform(Region region)
        {
            switch (region)
            {
                case Region.Br: return "BR1";
                case Region.Eune: return "EUN1";
                case Region.Euw: return "EUW1";
                case Region.Jp: return "JP1";
                case Region.Kr: return "KR1";
                case Region.Lan: return "LA1";
                case Region.Las: return "LA2";
                case Region.Na: return "NA1";
                case Region.Oce: return "OC1";
                case Region.Tr: return "TR1";
                case Region.Ru: return "RU1";
            }

            throw new Exception("Invalid region");
        }

        private double GetLevelFromXP(long xp)
        {
            if (xp >= 18360)
            {
                return 18;
            }

            int[] accumulativeLevelsPerLevel = { 0, 280, 660, 1140, 1720, 2400, 3180, 4060, 5040, 6120, 7300, 8580, 9960, 11440, 13020, 14700, 16480, 18360 };

            for (int i = 0; i < accumulativeLevelsPerLevel.Length - 1; i++)
            {
                if (accumulativeLevelsPerLevel[i] <= xp && xp < accumulativeLevelsPerLevel[i + 1])
                {
                    long levelExperience = accumulativeLevelsPerLevel[i + 1] - accumulativeLevelsPerLevel[i];
                    long currentExperience = xp - accumulativeLevelsPerLevel[i];
                    return i + 1 + (currentExperience / (double)levelExperience);
                }
            }

            return -1;
        }
    }
}
