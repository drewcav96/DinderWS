using System.Collections.Generic;

namespace DinderWS.Models.Profile {
    /// <summary>
    /// The Result for a Profile creation.
    /// </summary>
    public sealed class ProfileCreateResult : IResult {
        public bool Success { get; private set; }
        public string Message { get; set; }
        public string Error { get; set; }
        public IDictionary<string, string[]> ValidationErrors { get; set; }
        /// <summary>
        /// The Identity Id this profile belongs to.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Instantiates the result.
        /// </summary>
        /// <param name="success">Whether the result succeeded.</param>
        public ProfileCreateResult(bool success) {
            Success = success;
        }
    }
}
