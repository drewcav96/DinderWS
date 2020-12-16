using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DinderWS.Models.Rejects {
    public class Reject : IModel {
        public string ExperienceId { get; set; }
        public long MatchId { get; set; }
        public DateTimeOffset Timestamp { get; set; }

        public virtual Experience.Experience Experience { get; set; }
        public virtual Match.Match Match { get; set; }
    }
}
