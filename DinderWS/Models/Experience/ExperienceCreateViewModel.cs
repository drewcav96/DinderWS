using DinderWS.Enums;
using System;

namespace DinderWS.Models.Experience {
    /// <summary>
    /// The Create View Model for the Experience entity.
    /// </summary>
    public sealed class ExperienceCreateViewModel : ICreateViewModel<Experience> {
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
        /// Creates the experience for the specified User Id.
        /// </summary>
        /// <param name="userId">The User Id this profile belongs to.</param>
        /// <returns>The new experience entity.</returns>
        public Experience Create(string userId) {
            var exp = Create();

            exp.Id = userId;
            return exp;
        }

        public Experience Create() {
            return new Experience {
                CuisineType = CuisineType,
                GroupSize = GroupSize,
                GenderPreference = GenderPreference,
                Longitude = Longitude,
                Latitude = Latitude,
                Timestamp = DateTimeOffset.Now
            };
        }
    }
}
