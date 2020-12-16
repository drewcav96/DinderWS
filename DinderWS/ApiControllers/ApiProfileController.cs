using DinderWS.Data;
using DinderWS.Models.Profile;
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
    /// The API Profile Controller.
    /// </summary>
    [Route("api/Profile")]
    [ApiController]
    [Authorize]
    public class ApiProfileController : ApiBaseController<ApiProfileController> {
        /// <summary>
        /// Constructs the controller with dependency injection.
        /// </summary>
        /// <param name="logger">The Logger instance.</param>
        /// <param name="userManager">The User Manager service.</param>
        /// <param name="context">The Application Database Context.</param>
        public ApiProfileController(ILogger<ApiProfileController> logger,
                UserManager<IdentityUser> userManager,
                ApplicationDbContext context)
                    : base(logger, userManager, context) {

        }

        /// <summary>
        /// The Detail GET request action.
        /// </summary>
        /// <param name="id">The Identity Id of the Profile to return.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while completing the request.</param>
        /// <returns>
        /// <see cref="StatusCodes.Status200OK"/> object result when the profile is found.
        /// <see cref="StatusCodes.Status404NotFound"/> object result when the profile is not found.
        /// <see cref="StatusCodes.Status500InternalServerError"/> object result when there was an unexpected Exception.
        /// </returns>
        [HttpGet("Detail/{id}")]
        public async Task<IActionResult> GetDetailAsync(string id, CancellationToken cancellationToken) {
            try {
                var profile = await Context.Profiles
                    .Include(e => e.Identity)
                    .SingleOrDefaultAsync(e => e.Id == id, cancellationToken);
                var result = new ProfileDetailResult(profile);

                if (result.Success) {
                    return StatusCode(StatusCodes.Status200OK, result);
                }
                return StatusCode(StatusCodes.Status404NotFound, result);
            } catch (Exception ex) {
                Logger.LogError(ex, "Unexpected Exception during Profile GET.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ProfileDetailResult(null) {
                    Message = "Internal server error.",
                    Error = ex.Message
                });
            }
        }
        /// <summary>
        /// updates the rating of a different profile based on the rating of the user giving the rating
        /// </summary>
        /// id is the id of the profile being rated
        /// myrating is the rating of the user giving the rating
        /// yourrating is the rating of the user recieving the rating
        /// givenrating is the rating given
        /// <returns></returns>
        public async Task<IActionResult> RateProfileAsync(string id, int GivenRating, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ProfileUpdateResult(false)
                {
                    Message = "The User Id is unspecified."
                });
            }
            var currentUser = await UserManager.FindByIdAsync(id);

            if (currentUser == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ProfileUpdateResult(false)
                {
                    Message = "No User exists with specified Id."
                });
            }
            var profileExists = await Context.Profiles
                .AnyAsync(e => e.Id == id, cancellationToken);

            if (!profileExists)
            {
                return StatusCode(StatusCodes.Status409Conflict, new ProfileUpdateResult(false)
                {
                    Message = "No profile exists for the specified User Id."
                });
            }
            try
            {
                var profile = await Context.Profiles
                    .SingleAsync(e => e.Id == id, cancellationToken);
                int NewRating = profile.Rating + GivenRating;
                if (NewRating >= 19)
                {
                    profile.Rating = 10;
                }
                else
                {
                    profile.Rating = NewRating / 2;
                }
                Logger.LogInformation($"{id}'s rating was updated.");
                return StatusCode(StatusCodes.Status200OK, new ProfileUpdateResult(true)
                {
                    Message = "Profile successfully updated.",
                    Id = profile.Id
                });
            }

            catch (Exception ex)
            {
                Logger.LogError(ex, "Unexpected Exception during Rating Set.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ProfileDetailResult(null)
                {
                    Message = "Internal server error.",
                    Error = ex.Message
                });
            }
        }

        /// <summary>
        /// The Create POST request action.
        /// </summary>
        /// <param name="id">The User Id.</param>
        /// <param name="vm">The Profile Create view model from the POST request body.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while completing the request.</param>
        /// <returns>
        /// <see cref="StatusCodes.Status200OK"/> object result when the profile is successfully created.
        /// <see cref="StatusCodes.Status400BadRequest"/> object result when the User Id is unspecified or invalid.
        /// <see cref="StatusCodes.Status404NotFound"/> object result when the User Id does not exist.
        /// <see cref="StatusCodes.Status409Conflict"/> object result when the User Id already has a profile associated with it.
        /// <see cref="StatusCodes.Status500InternalServerError"/> object result when there was an unexpected Exception.
        /// </returns>
        [HttpPost("Create/{id}")]
        public async Task<IActionResult> PostCreateAsync(string id, ProfileCreateViewModel vm, CancellationToken cancellationToken) {
            if (string.IsNullOrWhiteSpace(id)) {
                return StatusCode(StatusCodes.Status400BadRequest, new ProfileCreateResult(false) {
                    Message = "The User Id is unspecified."
                });
            }
            var currentUser = await UserManager.FindByIdAsync(id);

            if (currentUser == null) {
                return StatusCode(StatusCodes.Status404NotFound, new ProfileCreateResult(false) {
                    Message = "No User exists with specified Id."
                });
            }
            var profileExists = await Context.Profiles
                .AnyAsync(e => e.Id == id, cancellationToken);

            if (profileExists) {
                return StatusCode(StatusCodes.Status409Conflict, new ProfileCreateResult(false) {
                    Message = "Profile already exists for the specified User Id."
                });
            }
            try {
                var profile = vm.Create(id);

                Context.Attach(profile).State = EntityState.Added;
                await Context.SaveChangesAsync(cancellationToken);
                Logger.LogInformation($"{id} created a new profile.");
                return StatusCode(StatusCodes.Status200OK, new ProfileCreateResult(true) {
                    Message = "Profile successfully created.",
                    Id = profile.Id
                });
            } catch (Exception ex) {
                Logger.LogError(ex, "Unexpected Exception during Create POST.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ProfileCreateResult(false) {
                    Message = "Internal server error.",
                    Error = ex.Message
                });
            }
        }

        /// <summary>
        /// The Update POST request action.
        /// </summary>
        /// <param name="id">The User Id.</param>
        /// <param name="vm">The Profile Update view model from the POST request body.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while completing the request.</param>
        /// <returns>
        /// <see cref="StatusCodes.Status200OK"/> object result when the profile is successfully updated.
        /// <see cref="StatusCodes.Status400BadRequest"/> object result when the User Id is unspecified or invalid.
        /// <see cref="StatusCodes.Status404NotFound"/> object result when the User Id does not exist.
        /// <see cref="StatusCodes.Status409Conflict"/> object result when the User Id does not have a profile associated with it.
        /// <see cref="StatusCodes.Status500InternalServerError"/> object result when there was an unexpected Exception.
        /// </returns>
        [HttpPost("Update/{id}")]
        public async Task<IActionResult> PostUpdateAsync(string id, ProfileUpdateViewModel vm, CancellationToken cancellationToken) {
            if (string.IsNullOrWhiteSpace(id)) {
                return StatusCode(StatusCodes.Status400BadRequest, new ProfileUpdateResult(false) {
                    Message = "The User Id is unspecified."
                });
            }
            var currentUser = await UserManager.FindByIdAsync(id);

            if (currentUser == null) {
                return StatusCode(StatusCodes.Status404NotFound, new ProfileUpdateResult(false) {
                    Message = "No User exists with specified Id."
                });
            }
            var profileExists = await Context.Profiles
                .AnyAsync(e => e.Id == id, cancellationToken);

            if (!profileExists) {
                return StatusCode(StatusCodes.Status409Conflict, new ProfileUpdateResult(false) {
                    Message = "No profile exists for the specified User Id."
                });
            }
            try {
                var profile = await Context.Profiles
                    .SingleAsync(e => e.Id == id, cancellationToken);

                vm.Update(profile);
                Context.Entry(profile).State = EntityState.Modified;
                await Context.SaveChangesAsync(cancellationToken);
                Logger.LogInformation($"{id} updated their profile.");
                return StatusCode(StatusCodes.Status200OK, new ProfileUpdateResult(true) {
                    Message = "Profile successfully updated.",
                    Id = profile.Id
                });
            } catch (Exception ex) {
                Logger.LogError(ex, "Unexpected Exception during Update POST.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ProfileUpdateResult(false) {
                    Message = "Internal server error.",
                    Error = ex.Message
                });
            }
        }
    }
}
