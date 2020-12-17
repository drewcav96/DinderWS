using DinderWS.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

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
        /// <summary>
        /// The Id of the Match this experience belongs to.
        /// </summary>
        public long? MatchId { get; set; }

        // Reference fields
        /// <summary>
        /// The <see cref="IdentityUser"/> entity this experience belongs to.
        /// </summary>
        public virtual IdentityUser Identity { get; set; }
        /// <summary>
        /// The <see cref="Profile.Profile"/> entity this experience belongs to.
        /// </summary>
        public virtual Profile.Profile Profile { get; set; }
        /// <summary>
        /// The <see cref="Match.Match"/> entity this experience belongs to.
        /// </summary>
        public virtual Match.Match Match { get; set; }
        public virtual ICollection<Rejects.Reject> Rejects { get; set; }

        public bool RejectMatch() {
            if (MatchId.HasValue) {
                Rejects.Add(new Rejects.Reject {
                    ExperienceId = Id,
                    MatchId = MatchId.Value
                });
                MatchId = null;
                return true;
            }
            return false;
        }
    }
}
