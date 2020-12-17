using System.Collections.Generic;

namespace DinderWS.Models.Identity {
    /// <summary>
    /// The Result for an Identity Creation.
    /// </summary>
    public sealed class IdentityCreateResult : IResult {
        public bool Success { get; private set; }
        public string Message { get; set; }
        public string Error { get; set; }
        public IDictionary<string, string[]> ValidationErrors { get; set; }
        /// <summary>
        /// The User Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Instantiates the result.
        /// </summary>
        /// <param name="success">Whether the result succeded.</param>
        public IdentityCreateResult(bool success) {
            Success = success;
        }
    }
}
