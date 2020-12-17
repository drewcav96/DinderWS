using DinderWS.Enums;
using System;
using System.Collections.Generic;

namespace DinderWS.Models.Experience {
    /// <summary>
    /// The Result of an Experience Detail.
    /// </summary>
    public sealed class ExperienceDetailResult : IResult {
        public bool Success { get; private set; }
        public string Message { get; set; }
        public string Error { get; set; }
        public IDictionary<string, string[]> ValidationErrors { get; set; }
        /// <summary>
        /// The Identity's Id which this experience belongs to.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// The cuisine type preference for this experience.
        /// </summary>
        public ECuisineType CuisineType { get; set; }
        /// <summary>
        /// The group size preference for this experience.
        /// </summary>
        public EGroupSize GroupSize { get; set; }
        /// <summary>
        /// The gender preference for this experience.
        /// </summary>
        public EGender GenderPreference { get; set; }
        /// <summary>
        /// The current location longitude.
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// The current location latitude.
        /// </summary>
        public double Latitude { get; set; }
        /// <summary>
        /// The timestamp when this experience was submitted.
        /// </summary>
        public DateTimeOffset Timestamp { get; set; }
        public long? MatchId { get; set; }

        /// <summary>
        /// Instantiates the result.
        /// </summary>
        /// <param name="model">The experience model to initialize this result with.</param>
        public ExperienceDetailResult(Experience model) {
            if (model == null) {
                Success = false;
                Message = "The requested experience could not be found.";
            } else {
                Success = true;
                Message = $"Experience for {model.Identity.Email}";
                Id = model.Id;
                CuisineType = model.CuisineType;
                GroupSize = model.GroupSize;
                GenderPreference = model.GenderPreference;
                Longitude = model.Longitude;
                Latitude = model.Latitude;
                Timestamp = model.Timestamp;
                MatchId = model.MatchId;
            }
        }
    }
}
