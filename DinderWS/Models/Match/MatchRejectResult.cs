using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DinderWS.Models.Match {
    public class MatchRejectResult : IResult {
        public bool Success { get; private set; }
        public string Message { get; set; }
        public string Error { get; set; }
        public IDictionary<string, string[]> ValidationErrors { get; set; }
        public string Id { get; set; }

        public MatchRejectResult(bool success, Experience.Experience model) {
            if (model == null) {
                Success = false;
                Message = "The requested experience could not be found.";
            } else {
                Success = success;
                if (success) {
                    Message = $"Match for Experience {model.Id} successfully rejected.";
                } else {
                    Message = $"Experience {model.Id} has no match to reject.";
                }
                Id = model.Id;
            }
        }
    }
}
