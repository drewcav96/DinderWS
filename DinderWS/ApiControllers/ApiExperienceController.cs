using DinderWS.Data;
using DinderWS.Models.Experience;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DinderWS.ApiControllers {
    /// <summary>
    /// The API Experience Controller.
    /// </summary>
    [Route("api/Experience")]
    [ApiController]
    [Authorize]
    public class ApiExperienceController : ApiBaseController<ApiExperienceController> {
        /// <summary>
        /// Constructs the controller with dependency injection.
        /// </summary>
        /// <param name="logger">The Logger instance.</param>
        /// <param name="userManager">The User Manager service.</param>
        /// <param name="context">The Application Database Context.</param>
        public ApiExperienceController(ILogger<ApiExperienceController> logger,
                UserManager<IdentityUser> userManager,
                ApplicationDbContext context)
                    : base(logger, userManager, context) {

        }

        /// <summary>
        /// The Detail GET request action.
        /// </summary>
        /// <param name="id">The Identity Id of the Experience to return.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while completing the request.</param>
        /// <returns>
        /// <see cref="StatusCodes.Status200OK"/> object result when the experience is found.
        /// <see cref="StatusCodes.Status404NotFound"/> object result when the experience is not found.
        /// <see cref="StatusCodes.Status500InternalServerError"/> object result when there was an unexpected Exception.
        /// </returns>
        [HttpGet("Detail/{id}")]
        public async Task<IActionResult> GetDetailAsync(string id, CancellationToken cancellationToken) {
            try {
                var exp = await Context.Experiences
                    .Include(e => e.Identity)
                    .SingleOrDefaultAsync(e => e.Id == id, cancellationToken);
                var result = new ExperienceDetailResult(exp);

                if (result.Success) {
                    return StatusCode(StatusCodes.Status200OK, result);
                }
                return StatusCode(StatusCodes.Status404NotFound, result);
            } catch (Exception ex) {
                Logger.LogError(ex, "Unexpected Exception during Experience GET.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ExperienceDetailResult(null) {
                    Message = "Internal server error.",
                    Error = ex.Message
                });
            }
        }

        /// <summary>
        /// The Create POST request action.
        /// </summary>
        /// <param name="id">The User Id.</param>
        /// <param name="vm">The Experience Create view model from the POST request body.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while completing the request.</param>
        /// <returns>
        /// <see cref="StatusCodes.Status200OK"/> object result when the experience is successfully created.
        /// <see cref="StatusCodes.Status400BadRequest"/> object result when the User Id is unspecified or invalid.
        /// <see cref="StatusCodes.Status404NotFound"/> object result when the User Id does not exist.
        /// <see cref="StatusCodes.Status409Conflict"/> object result when the User Id already has an experience associated with it.
        /// <see cref="StatusCodes.Status500InternalServerError"/> object result when there was an unexpected Exception.
        /// </returns>
        [HttpPost("Create/{id}")]
        public async Task<IActionResult> PostCreateAsync(string id, ExperienceCreateViewModel vm, CancellationToken cancellationToken) {
            if (string.IsNullOrWhiteSpace(id)) {
                return StatusCode(StatusCodes.Status400BadRequest, new ExperienceCreateResult(false) {
                    Message = "The User Id is unspecified."
                });
            }
            var currentUser = await UserManager.FindByIdAsync(id);

            if (currentUser == null) {
                return StatusCode(StatusCodes.Status404NotFound, new ExperienceCreateResult(false) {
                    Message = "No User exists with specified Id."
                });
            }
            var profileExists = await Context.Profiles
                .AnyAsync(e => e.Id == id, cancellationToken);

            if (profileExists) {
                return StatusCode(StatusCodes.Status409Conflict, new ExperienceCreateResult(false) {
                    Message = "Experience already exists for the specified User Id."
                });
            }
            try {
                var exp = vm.Create(id);

                Context.Attach(exp).State = EntityState.Added;
                await Context.SaveChangesAsync(cancellationToken);
                Logger.LogInformation($"{id} created a new experience.");
                return StatusCode(StatusCodes.Status200OK, new ExperienceCreateResult(true) {
                    Message = "Experience successfully created.",
                    Id = exp.Id
                });
            } catch (Exception ex) {
                Logger.LogError(ex, "Unexpected Exception during Create POST.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ExperienceCreateResult(false) {
                    Message = "Internal server error.",
                    Error = ex.Message
                });
            }
        }
    }
}
