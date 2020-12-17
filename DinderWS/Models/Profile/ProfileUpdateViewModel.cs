using DinderWS.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace DinderWS.Models.Profile {
    /// <summary>
    /// The Update View Model for the Profile entity.
    /// </summary>
    public sealed class ProfileUpdateViewModel : IUpdateViewModel<Profile> {
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

        public void Update(Profile model) {
            if (model == null) {
                throw new ArgumentNullException(nameof(model));
            }
            model.Firstname = Firstname;
            model.Lastname = Lastname;
            model.Gender = Gender;
            model.AvatarUrl = AvatarUrl;
            model.DietaryRestrictions = DietaryRestrictions;
            model.Interests = Interests;
        }
    }
}
