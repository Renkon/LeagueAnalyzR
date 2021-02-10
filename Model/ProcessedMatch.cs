using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeagueAnalyzR.Model
{
    public class ProcessedMatch
    {

        public long GameId { get; set; }

        public string MatchUrl { get; set; }

        public DateTime CreatedOn { get; set; }

        public TimeSpan _Duration { get; set; }

        public string Duration => $"{_Duration.Minutes:00}:{_Duration.Seconds:00}";

        public string Side { get; set; }

        public string Result { get; set; }

        public string Role { get; set; }

        public string Champion { get; set; }

        public string EnemyChampion { get; set; }

        public long Kills { get; set; }

        public long Deaths { get; set; }

        public long Assists { get; set; }

        public double _Kda { get; set; }
        
        public string Kda => _Kda != double.MaxValue ? Math.Round(_Kda, 2).ToString() : "Perfecto";

        public double _KillParticipation { get; set; }

        public string KillParticipation => _KillParticipation != double.MaxValue? Math.Round(_KillParticipation, 2).ToString() + "%" : "No %";

        public double _EnemyKillParticipation { get; set; }

        public string EnemyKillParticipation => _EnemyKillParticipation != double.MaxValue ? Math.Round(_EnemyKillParticipation, 2).ToString() + "%" : "No %";

        public long Gold { get; set; }

        public long EnemyGold { get; set; }

        public long GoldDifference => Gold - EnemyGold;

        public double _Level { get; set; }

        public string Level => Math.Round(_Level, 2).ToString();

        public double _EnemyLevel { get; set; }

        public string EnemyLevel => Math.Round(_EnemyLevel, 2).ToString();

        public double _LevelDifference => _Level - _EnemyLevel;

        public string LevelDifference => Math.Round(_LevelDifference, 2).ToString();

        public long MinionsFarm { get; set; }
        
        public long JungleFarm { get; set; }

        public long AllyJungleFarm { get; set; }

        public long EnemyJungleFarm { get; set; }

        public long TotalFarm => MinionsFarm + JungleFarm;

        public string FarmPerMinute => Math.Round(TotalFarm / _Duration.TotalMinutes, 2).ToString();

        public long WardsPlaced { get; set; }

        public long WardsDestroyed { get; set; }

        public long ControlWardsBought { get; set; }

        public string WardsPerMinute => Math.Round(WardsPlaced / _Duration.TotalMinutes, 2).ToString();

        public long VisionPoints { get; set; }

        public List<int> _GoldAtMinute { get; set; }

        public List<int> _EnemyGoldAtMinute { get; set; }

        public List<double> _LevelAtMinute { get; set; }

        public List<double> _EnemyLevelAtMinute { get; set; }

        public List<int> _MinionsAtMinute { get; set; }

        public List<int> _JungleMinionsAtMinute { get; set; }

        public List<int> _EnemyMinionsAtMinute { get; set; }

        public List<int> _EnemyJungleMinionsAtMinute { get; set; }

        /* Gold */
        public string GoldAtMinute5 => _GoldAtMinute.Count() >= 6 ? _GoldAtMinute[5].ToString() : "-";

        public string EnemyGoldAtMinute5 => _EnemyGoldAtMinute.Count() >= 6 ? _EnemyGoldAtMinute[5].ToString() : "-";

        public string GoldDifferenceAtMinute5 => _GoldAtMinute.Count() >= 6 && _EnemyGoldAtMinute.Count() >= 6 ? (_GoldAtMinute[5] - _EnemyGoldAtMinute[5]).ToString() : "-";

        public string GoldAtMinute8 => _GoldAtMinute.Count() >= 9 ? _GoldAtMinute[8].ToString() : "-";

        public string EnemyGoldAtMinute8 => _EnemyGoldAtMinute.Count() >= 9 ? _EnemyGoldAtMinute[8].ToString() : "-";

        public string GoldDifferenceAtMinute8 => _GoldAtMinute.Count() >= 9 && _EnemyGoldAtMinute.Count() >= 9 ? (_GoldAtMinute[8] - _EnemyGoldAtMinute[8]).ToString() : "-";

        public string GoldAtMinute12 => _GoldAtMinute.Count() >= 13 ? _GoldAtMinute[12].ToString() : "-";

        public string EnemyGoldAtMinute12 => _EnemyGoldAtMinute.Count() >= 13 ? _EnemyGoldAtMinute[12].ToString() : "-";

        public string GoldDifferenceAtMinute12 => _GoldAtMinute.Count() >= 13 && _EnemyGoldAtMinute.Count() >= 13 ? (_GoldAtMinute[12] - _EnemyGoldAtMinute[12]).ToString() : "-";

        public string GoldAtMinute15 => _GoldAtMinute.Count() >= 16 ? _GoldAtMinute[15].ToString() : "-";

        public string EnemyGoldAtMinute15 => _EnemyGoldAtMinute.Count() >= 16 ? _EnemyGoldAtMinute[15].ToString() : "-";

        public string GoldDifferenceAtMinute15 => _GoldAtMinute.Count() >= 16 && _EnemyGoldAtMinute.Count() >= 16 ? (_GoldAtMinute[15] - _EnemyGoldAtMinute[15]).ToString() : "-";

        public string GoldAtMinute20 => _GoldAtMinute.Count() >= 21 ? _GoldAtMinute[20].ToString() : "-";

        public string EnemyGoldAtMinute20 => _EnemyGoldAtMinute.Count() >= 21 ? _EnemyGoldAtMinute[20].ToString() : "-";

        public string GoldDifferenceAtMinute20 => _GoldAtMinute.Count() >= 21 && _EnemyGoldAtMinute.Count() >= 21 ? (_GoldAtMinute[20] - _EnemyGoldAtMinute[20]).ToString() : "-";

        public string GoldAtMinute25 => _GoldAtMinute.Count() >= 26 ? _GoldAtMinute[25].ToString() : "-";

        public string EnemyGoldAtMinute25 => _EnemyGoldAtMinute.Count() >= 26 ? _EnemyGoldAtMinute[25].ToString() : "-";

        public string GoldDifferenceAtMinute25 => _GoldAtMinute.Count() >= 26 && _EnemyGoldAtMinute.Count() >= 26 ? (_GoldAtMinute[25] - _EnemyGoldAtMinute[25]).ToString() : "-";

        public string GoldAtMinute30 => _GoldAtMinute.Count() >= 31 ? _GoldAtMinute[30].ToString() : "-";

        public string EnemyGoldAtMinute30 => _EnemyGoldAtMinute.Count() >= 31 ? _EnemyGoldAtMinute[30].ToString() : "-";

        public string GoldDifferenceAtMinute30 => _GoldAtMinute.Count() >= 31 && _EnemyGoldAtMinute.Count() >= 31 ? (_GoldAtMinute[30] - _EnemyGoldAtMinute[30]).ToString() : "-";

        public string GoldAtMinute40 => _GoldAtMinute.Count() >= 41 ? _GoldAtMinute[40].ToString() : "-";

        public string EnemyGoldAtMinute40 => _EnemyGoldAtMinute.Count() >= 41 ? _EnemyGoldAtMinute[40].ToString() : "-";

        public string GoldDifferenceAtMinute40 => _GoldAtMinute.Count() >= 41 && _EnemyGoldAtMinute.Count() >= 41 ? (_GoldAtMinute[40] - _EnemyGoldAtMinute[40]).ToString() : "-";

        /* Level */
        public string LevelAtMinute5 => _LevelAtMinute.Count() >= 6 ? Math.Round(_LevelAtMinute[5], 2).ToString() : "-";

        public string EnemyLevelAtMinute5 => _EnemyLevelAtMinute.Count() >= 6 ? Math.Round(_EnemyLevelAtMinute[5], 2).ToString() : "-";

        public string LevelDifferenceAtMinute5 => _LevelAtMinute.Count() >= 6 && _EnemyLevelAtMinute.Count() >= 6 ? Math.Round(_LevelAtMinute[5] - _EnemyLevelAtMinute[5], 2).ToString() : "-";

        public string LevelAtMinute8 => _LevelAtMinute.Count() >= 9 ? Math.Round(_LevelAtMinute[8], 2).ToString() : "-";

        public string EnemyLevelAtMinute8 => _EnemyLevelAtMinute.Count() >= 9 ? Math.Round(_EnemyLevelAtMinute[8], 2).ToString() : "-";

        public string LevelDifferenceAtMinute8 => _LevelAtMinute.Count() >= 9 && _EnemyLevelAtMinute.Count() >= 9 ? Math.Round(_LevelAtMinute[8] - _EnemyLevelAtMinute[8], 2).ToString() : "-";

        public string LevelAtMinute12 => _LevelAtMinute.Count() >= 13 ? Math.Round(_LevelAtMinute[12], 2).ToString() : "-";

        public string EnemyLevelAtMinute12 => _EnemyLevelAtMinute.Count() >= 13 ? Math.Round(_EnemyLevelAtMinute[12], 2).ToString() : "-";

        public string LevelDifferenceAtMinute12 => _LevelAtMinute.Count() >= 13 && _EnemyLevelAtMinute.Count() >= 13 ? Math.Round(_LevelAtMinute[12] - _EnemyLevelAtMinute[12], 2).ToString() : "-";

        public string LevelAtMinute15 => _LevelAtMinute.Count() >= 16 ? Math.Round(_LevelAtMinute[15], 2).ToString() : "-";

        public string EnemyLevelAtMinute15 => _EnemyLevelAtMinute.Count() >= 16 ? Math.Round(_EnemyLevelAtMinute[15], 2).ToString() : "-";

        public string LevelDifferenceAtMinute15 => _LevelAtMinute.Count() >= 16 && _EnemyLevelAtMinute.Count() >= 16 ? Math.Round(_LevelAtMinute[15] - _EnemyLevelAtMinute[15], 2).ToString() : "-";

        public string LevelAtMinute20 => _LevelAtMinute.Count() >= 21 ? Math.Round(_LevelAtMinute[20], 2).ToString() : "-";

        public string EnemyLevelAtMinute20 => _EnemyLevelAtMinute.Count() >= 21 ? Math.Round(_EnemyLevelAtMinute[20], 2).ToString() : "-";

        public string LevelDifferenceAtMinute20 => _LevelAtMinute.Count() >= 21 && _EnemyLevelAtMinute.Count() >= 21 ? Math.Round(_LevelAtMinute[20] - _EnemyLevelAtMinute[20], 2).ToString() : "-";

        public string LevelAtMinute25 => _LevelAtMinute.Count() >= 26 ? Math.Round(_LevelAtMinute[25], 2).ToString() : "-";

        public string EnemyLevelAtMinute25 => _EnemyLevelAtMinute.Count() >= 26 ? Math.Round(_EnemyLevelAtMinute[25], 2).ToString() : "-";

        public string LevelDifferenceAtMinute25 => _LevelAtMinute.Count() >= 26 && _EnemyLevelAtMinute.Count() >= 26 ? Math.Round(_LevelAtMinute[25] - _EnemyLevelAtMinute[25], 2).ToString() : "-";

        public string LevelAtMinute30 => _LevelAtMinute.Count() >= 31 ? Math.Round(_LevelAtMinute[30], 2).ToString() : "-";

        public string EnemyLevelAtMinute30 => _EnemyLevelAtMinute.Count() >= 31 ? Math.Round(_EnemyLevelAtMinute[30], 2).ToString() : "-";

        public string LevelDifferenceAtMinute30 => _LevelAtMinute.Count() >= 31 && _EnemyLevelAtMinute.Count() >= 31 ? Math.Round(_LevelAtMinute[30] - _EnemyLevelAtMinute[30], 2).ToString() : "-";

        public string LevelAtMinute40 => _LevelAtMinute.Count() >= 41 ? Math.Round(_LevelAtMinute[40], 2).ToString() : "-";

        public string EnemyLevelAtMinute40 => _EnemyLevelAtMinute.Count() >= 41 ? Math.Round(_EnemyLevelAtMinute[40], 2).ToString() : "-";

        public string LevelDifferenceAtMinute40 => _LevelAtMinute.Count() >= 41 && _EnemyLevelAtMinute.Count() >= 41 ? Math.Round(_LevelAtMinute[40] - _EnemyLevelAtMinute[40], 2).ToString() : "-";

        /* CS */

        public string FarmAtMinute5 => _LevelAtMinute.Count() >= 6 ? (_MinionsAtMinute[5] + _JungleMinionsAtMinute[5]).ToString() : "-";

        public string EnemyFarmAtMinute5 => _EnemyLevelAtMinute.Count() >= 6 ? (_EnemyMinionsAtMinute[5] + _EnemyJungleMinionsAtMinute[5]).ToString() : "-";

        public string FarmDifferenceAtMinute5 => _LevelAtMinute.Count() >= 6 && _EnemyLevelAtMinute.Count() >= 6 ? (_MinionsAtMinute[5] + _JungleMinionsAtMinute[5] - _EnemyMinionsAtMinute[5] - _EnemyJungleMinionsAtMinute[5]).ToString() : "-";

        public string FarmAtMinute8 => _LevelAtMinute.Count() >= 9 ? (_MinionsAtMinute[8] + _JungleMinionsAtMinute[8]).ToString() : "-";

        public string EnemyFarmAtMinute8 => _EnemyLevelAtMinute.Count() >= 9 ? (_EnemyMinionsAtMinute[8] + _EnemyJungleMinionsAtMinute[8]).ToString() : "-";

        public string FarmDifferenceAtMinute8 => _LevelAtMinute.Count() >= 9 && _EnemyLevelAtMinute.Count() >= 9 ? (_MinionsAtMinute[8] + _JungleMinionsAtMinute[8] - _EnemyMinionsAtMinute[8] - _EnemyJungleMinionsAtMinute[8]).ToString() : "-";

        public string FarmAtMinute12 => _LevelAtMinute.Count() >= 13 ? (_MinionsAtMinute[12] + _JungleMinionsAtMinute[12]).ToString() : "-";

        public string EnemyFarmAtMinute12 => _EnemyLevelAtMinute.Count() >= 13 ? (_EnemyMinionsAtMinute[12] + _EnemyJungleMinionsAtMinute[12]).ToString() : "-";

        public string FarmDifferenceAtMinute12 => _LevelAtMinute.Count() >= 13 && _EnemyLevelAtMinute.Count() >= 13 ? (_MinionsAtMinute[12] + _JungleMinionsAtMinute[12] - _EnemyMinionsAtMinute[12] - _EnemyJungleMinionsAtMinute[12]).ToString() : "-";

        public string FarmAtMinute15 => _LevelAtMinute.Count() >= 16 ? (_MinionsAtMinute[15] + _JungleMinionsAtMinute[15]).ToString() : "-";

        public string EnemyFarmAtMinute15 => _EnemyLevelAtMinute.Count() >= 16 ? (_EnemyMinionsAtMinute[15] + _EnemyJungleMinionsAtMinute[15]).ToString() : "-";

        public string FarmDifferenceAtMinute15 => _LevelAtMinute.Count() >= 16 && _EnemyLevelAtMinute.Count() >= 16 ? (_MinionsAtMinute[15] + _JungleMinionsAtMinute[15] - _EnemyMinionsAtMinute[15] - _EnemyJungleMinionsAtMinute[15]).ToString() : "-";

        public string FarmAtMinute20 => _LevelAtMinute.Count() >= 21 ? (_MinionsAtMinute[20] + _JungleMinionsAtMinute[20]).ToString() : "-";

        public string EnemyFarmAtMinute20 => _EnemyLevelAtMinute.Count() >= 21 ? (_EnemyMinionsAtMinute[20] + _EnemyJungleMinionsAtMinute[20]).ToString() : "-";

        public string FarmDifferenceAtMinute20 => _LevelAtMinute.Count() >= 21 && _EnemyLevelAtMinute.Count() >= 21 ? (_MinionsAtMinute[20] + _JungleMinionsAtMinute[20] - _EnemyMinionsAtMinute[20] - _EnemyJungleMinionsAtMinute[20]).ToString() : "-";

        public string FarmAtMinute25 => _LevelAtMinute.Count() >= 26 ? (_MinionsAtMinute[25] + _JungleMinionsAtMinute[25]).ToString() : "-";

        public string EnemyFarmAtMinute25 => _EnemyLevelAtMinute.Count() >= 26 ? (_EnemyMinionsAtMinute[25] + _EnemyJungleMinionsAtMinute[25]).ToString() : "-";

        public string FarmDifferenceAtMinute25 => _LevelAtMinute.Count() >= 26 && _EnemyLevelAtMinute.Count() >= 26 ? (_MinionsAtMinute[25] + _JungleMinionsAtMinute[25] - _EnemyMinionsAtMinute[25] - _EnemyJungleMinionsAtMinute[25]).ToString() : "-";

        public string FarmAtMinute30 => _LevelAtMinute.Count() >= 31 ? (_MinionsAtMinute[30] + _JungleMinionsAtMinute[30]).ToString() : "-";

        public string EnemyFarmAtMinute30 => _EnemyLevelAtMinute.Count() >= 31 ? (_EnemyMinionsAtMinute[30] + _EnemyJungleMinionsAtMinute[30]).ToString() : "-";

        public string FarmDifferenceAtMinute30 => _LevelAtMinute.Count() >= 31 && _EnemyLevelAtMinute.Count() >= 31 ? (_MinionsAtMinute[30] + _JungleMinionsAtMinute[30] - _EnemyMinionsAtMinute[30] - _EnemyJungleMinionsAtMinute[30]).ToString() : "-";

        public string FarmAtMinute40 => _LevelAtMinute.Count() >= 41 ? (_MinionsAtMinute[40] + _JungleMinionsAtMinute[40]).ToString() : "-";

        public string EnemyFarmAtMinute40 => _EnemyLevelAtMinute.Count() >= 41 ? (_EnemyMinionsAtMinute[40] + _EnemyJungleMinionsAtMinute[40]).ToString() : "-";

        public string FarmDifferenceAtMinute40 => _LevelAtMinute.Count() >= 41 && _EnemyLevelAtMinute.Count() >= 41 ? (_MinionsAtMinute[40] + _JungleMinionsAtMinute[40] - _EnemyMinionsAtMinute[40] - _EnemyJungleMinionsAtMinute[40]).ToString() : "-";
    }
}
