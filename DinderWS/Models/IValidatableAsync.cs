using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace DinderWS.Models {
    /// <summary>
    /// Interface for contextual asynchronous validation.
    /// </summary>
    public interface IValidatableAsync : IValidatableObject {
        /// <summary>
        /// The <see cref="TimeSpan"/> before validation is aborted.
        /// </summary>
        public static TimeSpan Timeout = TimeSpan.FromSeconds(5);

        /// <summary>
        /// Asynchronously confirms validity of the view model.
        /// </summary>
        /// <param name="validationContext">The Validation Context.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while completing the validation.</param>
        /// <returns></returns>
        Task<IEnumerable<ValidationResult>> ValidateAsync(ValidationContext validationContext, CancellationToken cancellationToken);
    }
}
