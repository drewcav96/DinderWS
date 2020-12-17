using System.Collections.Generic;

namespace DinderWS.Models.Identity {
    /// <summary>
    /// The Result for an Identity Login.
    /// </summary>
    public class IdentityLoginResult : IResult {
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
        /// <param name="success">Whether the result succeeded.</param>
        public IdentityLoginResult(bool success) {
            Success = success;
        }
    }
}
