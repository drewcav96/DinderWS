using DinderWS.Enums;
using System.Collections.Generic;

namespace DinderWS.Models.Profile {
    /// <summary>
    /// The Result of Profile Detail. 
    /// </summary>
    public sealed class ProfileDetailResult : IResult {
        public bool Success { get; private set; }
        public string Message { get; set; }
        public string Error { get; set; }
        public IDictionary<string, string[]> ValidationErrors { get; set; }
        /// <summary>
        /// The Identity's Id which this profile belongs to.
        /// </summary>
        public string Id { get; private set; }
        /// <summary>
        /// The given name for this profile.
        /// </summary>
        public string Firstname { get; private set; }
        /// <summary>
        /// The surname for this profile.
        /// </summary>
        public string Lastname { get; private set; }
        /// <summary>
        /// This profile's gender.
        /// </summary>
        public EGender Gender { get; private set; }
        /// <summary>
        /// The URL of the avatar for this profile.
        public string AvatarUrl { get; private set; }
        /// <summary>
        /// The dietary restriction preferences for this profile.
        /// </summary>
        public EDietaryRestriction DietaryRestrictions { get; private set; }
        /// <summary>
        /// The interest preferences for this profile.
        /// </summary>
        public EInterest Interests { get; private set; }
        public int Rating { get; private set; }

        /// <summary>
        /// Instantiates the result.
        /// </summary>
        /// <param name="model">The profile model to initialize this result with.</param>
        public ProfileDetailResult(Profile model) {
            if (model == null) {
                Success = false;
                Message = "The requested profile could not be found.";
            } else {
                Success = true;
                Message = $"Profile for {model.Firstname} {model.Lastname}.";
                Id = model.Id;
                Firstname = model.Firstname;
                Lastname = model.Lastname;
                Gender = model.Gender;
                AvatarUrl = model.AvatarUrl;
                DietaryRestrictions = model.DietaryRestrictions;
                Interests = model.Interests;
                Rating = model.Rating;
            }
        }
    }
}
