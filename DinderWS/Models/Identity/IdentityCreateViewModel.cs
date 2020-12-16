using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DinderWS.Models.Identity {
    /// <summary>
    /// The Create View Model for the Identity User entity.
    /// </summary>
    public sealed class IdentityCreateViewModel : ICreateViewModel<IdentityUser> {
        /// <summary>
        /// The User's Email address.
        /// </summary>
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        /// <summary>
        /// The password.
        /// </summary>
        [DataType(DataType.Password)]
        public string Password { get; set; }
        /// <summary>
        /// The confirmed password.
        /// </summary>
        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public IdentityUser Create() {
            return new IdentityUser {
                UserName = Email,
                Email = Email
            };
        }
    }
}
