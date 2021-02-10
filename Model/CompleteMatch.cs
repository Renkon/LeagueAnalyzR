using RiotSharp.Endpoints.MatchEndpoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeagueAnalyzR.Model
{
    public class CompleteMatch
    {
        public MatchReference MatchReference { get; set; }

        public Match Match { get; set; }

        public MatchTimeline MatchTimeline { get; set; }
    }
}
