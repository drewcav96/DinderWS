using DinderWS.Identity;
using System.Collections.Generic;

namespace DinderWS.Models.Experience {
    /// <summary>
    /// The Result for an Experience creation.
    /// </summary>
    public sealed class ExperienceCreateResult : IResult {
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
        public ExperienceCreateResult(bool success) {
            Success = success;
        }
    }
}
