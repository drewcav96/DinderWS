using DinderWS.Enums;
using System.ComponentModel.DataAnnotations;

namespace DinderWS.Models.Profile {
    /// <summary>
    /// The Create View Model for the Profile entity.
    /// </summary>
    public sealed class ProfileCreateViewModel : ICreateViewModel<Profile> {
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
        [DataType(DataType.ImageUrl)]
        public string AvatarUrl { get; set; }
        /// <summary>
        /// The dietary restriction preferences for this profile.
        /// </summary>
        public EDietaryRestriction DietaryRestrictions { get; set; }
        /// <summary>
        /// The interest preferences for this profile.
        /// </summary>
        public EInterest Interests { get; set; }

        /// <summary>
        /// Creates the profile for the specified User Id.
        /// </summary>
        /// <param name="userId">The User Id this profile belongs to.</param>
        /// <returns>The new profile entity.</returns>
        public Profile Create(string userId) {
            var profile = Create();

            profile.Id = userId;
            return profile;
        }

        public Profile Create() {
            return new Profile {
                Firstname = Firstname,
                Lastname = Lastname,
                Gender = Gender,
                AvatarUrl = AvatarUrl,
                DietaryRestrictions = DietaryRestrictions,
                Interests = Interests
            };
        }
    }
}
