using DinderWS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DinderWS.Models.Match {
    public class Match : IModel {
        public long Id { get; set; }
        public EGroupSize GroupSize { get; set; }
        public EGender Gender { get; set; }
        public ECuisineType CuisineType { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public double AvgLongitude { get; set; }
        public double AvgLatitude { get; set; }

        public virtual ICollection<Experience.Experience> Experiences { get; set; }
        public virtual ICollection<Rejects.Reject> Rejects { get; set; }

        public bool IsFull {
            get {
                if (GroupSize == EGroupSize.Small) {
                    return Experiences.Count >= 3;
                }
                return Experiences.Count >= 6;
            }
        }

        public bool AddExperience(Experience.Experience experience) {
            if (!IsFull) {
                AvgLongitude = ((AvgLongitude * Experiences.Count) + experience.Longitude) / (Experiences.Count + 1);
                AvgLatitude = ((AvgLatitude * Experiences.Count) + experience.Latitude) / (Experiences.Count + 1);
                Experiences.Add(experience);
                experience.MatchId = Id;
                return true;
            }
            return false;
        }
    }
}
