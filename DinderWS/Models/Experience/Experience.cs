using DinderWS.Enums;
using Microsoft.AspNetCore.Identity;
using System;

namespace DinderWS.Models.Experience {
    /// <summary>
    /// The Experience entity model.
    /// </summary>
    public class Experience : IModel {
        // Scalar fields
        /// <summary>
        /// The <see cref="IdentityUser"/>'s Id which this experience belongs to.
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

        // Reference fields
        /// <summary>
        /// The <see cref="IdentityUser"/> entity this profile belongs to.
        /// </summary>
        public virtual IdentityUser Identity { get; set; }
        /// <summary>
        /// The <see cref="Profile.Profile"/> entity this profile belongs to.
        /// </summary>
        public virtual Profile.Profile Profile { get; set; }
    }
}
