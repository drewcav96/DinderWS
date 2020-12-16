using DinderWS.Enums;
using Microsoft.AspNetCore.Identity;

namespace DinderWS.Models.Profile {
    /// <summary>
    /// The Profile entity model.
    /// </summary>
    public class Profile : IModel {
        // Scalar fields
        /// <summary>
        /// The <see cref="IdentityUser"/>'s Id which this profile belongs to.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// The given name for this profile.
        /// </summary>
        public string Firstname { get; set; }
        /// <summary>
        /// The surname for this profile.
        /// </summary>
        public string Lastname { get; set; }
        /// <summary>
        /// This profile's gender.
        /// </summary>
        public EGender Gender { get; set; }
        /// <summary>
        /// The URL of the avatar for this profile.
        /// </summary>
        public string AvatarUrl { get; set; }
        /// <summary>
        /// The dietary restriction preferences for this profile.
        /// </summary>
        public EDietaryRestriction DietaryRestrictions { get; set; }
        /// <summary>
        /// The interest preferences for this profile.
        /// </summary>
        public EInterest Interests { get; set; }

        public int Rating { get; set; }
        // Reference fields
        /// <summary>
        /// The <see cref="IdentityUser"/> entity this profile belongs to.
        /// </summary>

        public virtual IdentityUser Identity { get; set; }
        /// <summary>
        /// The <see cref="Experience.Experience"/> that belongs to this profile, if any.
        /// </summary>
        public virtual Experience.Experience Experience { get; set; }
    }
}
