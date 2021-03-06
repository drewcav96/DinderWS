﻿using System.ComponentModel.DataAnnotations;

namespace DinderWS.Models.Identity {
    /// <summary>
    /// The Identity Login view model.
    /// </summary>
    public class IdentityLoginViewModel : IViewModel {
        /// <summary>
        /// The Identity Email address.
        /// </summary>
        [Required]
        public string Email { get; set; }
        /// <summary>
        /// The Identity unhashed password.
        /// </summary>
        [Required]
        public string Password { get; set; }
        /// <summary>
        /// Whether this login should persist through multiple sessions.
        /// </summary>
        [Required]
        public bool PersistLogin { get; set; }
    }
}
