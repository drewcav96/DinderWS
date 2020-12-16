using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DinderWS.Models.Match {
    public class MatchFindResult : IResult {
        public bool Success { get; private set; }
        public string Message { get; set; }
        public string Error { get; set; }
        public IDictionary<string, string[]> ValidationErrors { get; set; }
        public long Id { get; set; }

        public MatchFindResult(Match model) {
            if (model == null) {
                Success = false;
                Message = "No match could be found or created.";
            } else {
                Success = true;
                Message = $"Match found: {model.Id}";
                Id = model.Id;
            }
        }
    }
}
