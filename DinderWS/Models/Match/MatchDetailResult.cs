using DinderWS.Enums;
using DinderWS.Models.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DinderWS.Models.Match {
    public class MatchDetailResult : IResult {
        public bool Success { get; private set; }
        public string Message { get; set; }
        public string Error { get; set; }
        public IDictionary<string, string[]> ValidationErrors { get; set; }
        public long Id { get; set; }
        public EGroupSize GroupSize { get; set; }
        public EGender Gender { get; set; }
        public ECuisineType CuisineType { get; set; }
        public double AvgLongitude { get; set; }
        public double AvgLatitude { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public IEnumerable<ProfileDetailResult> Profiles { get; set; }

        public MatchDetailResult(Match model) {
            if (model == null) {
                Success = false;
                Message = "The requested match could not be found.";
            } else {
                Success = true;
                Message = $"Match for Id {model.Id}";
                Id = model.Id;
                GroupSize = model.GroupSize;
                Gender = model.Gender;
                CuisineType = model.CuisineType;
                AvgLongitude = model.AvgLongitude;
                AvgLatitude = model.AvgLatitude;
                Timestamp = model.Timestamp;
                Profiles = model.Experiences
                    .Select(m => new ProfileDetailResult(m.Profile));
            }
        }
    }
}
