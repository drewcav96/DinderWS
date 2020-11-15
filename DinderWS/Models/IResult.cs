using System.Collections.Generic;

namespace DinderWS.Identity {
    /// <summary>
    /// Interface for the Result.
    /// </summary>
    public interface IResult {
        /// <summary>
        /// Whether the operation succeeded.
        /// </summary>
        bool Success { get; }
        /// <summary>
        /// The result message.
        /// </summary>
        string Message { get; }
        /// <summary>
        /// The result error, if any.
        /// </summary>
        string Error { get; }
        /// <summary>
        /// The validation errors, if any.
        /// </summary>
        IDictionary<string, string[]> ValidationErrors { get; set; }
    }
}
