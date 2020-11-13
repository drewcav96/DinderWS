using DinderWS.Data;
using DinderWS.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DinderWS.ApiControllers {
    /// <summary>
    /// The API Identity Controller.
    /// </summary>
    [Route("api/Identity")]
    [ApiController]
    [Authorize]
    public class ApiIdentityController : ApiBaseController<ApiIdentityController> {
        private readonly SignInManager<IdentityUser> _signInManager;

        /// <summary>
        /// Constructs the controller with dependency injection.
        /// </summary>
        /// <param name="signInManager">The Sign In Manager service.</param>
        /// <param name="logger">The Logger instance.</param>
        /// <param name="userManager">The User Manager service.</param>
        /// <param name="context">The Application Database Context.</param>
        public ApiIdentityController(SignInManager<IdentityUser> signInManager,
                ILogger<ApiIdentityController> logger,
                UserManager<IdentityUser> userManager,
                ApplicationDbContext context)
                    : base(logger, userManager, context) {
            _signInManager = signInManager;
        }

        /// <summary>
        /// The Login GET request action.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while completing the request.</param>
        /// <returns>
        /// <see cref="StatusCodes.Status200OK"/> object result.
        /// </returns>
        [HttpGet("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLoginAsync(CancellationToken cancellationToken) {
            // handle user already signed in
            if (_signInManager.IsSignedIn(User)) {
                try {
                    var currentUser = await GetCurrentUserAsync();

                    await _signInManager.RefreshSignInAsync(currentUser);
                    return StatusCode(StatusCodes.Status200OK, new IdentityLoginResult(true) {
                        Message = "Already signed in.",
                        Id = currentUser.Id
                    });
                } catch (Exception ex) {
                    Logger.LogError(ex, "Unexpected Exception during Login GET.");
                    return StatusCode(StatusCodes.Status500InternalServerError, new IdentityLoginResult(false) {
                        Message = "Internal server error.",
                        Error = ex.Message
                    });
                }
            }
            // no current user signed in
            return StatusCode(StatusCodes.Status200OK, new IdentityLoginResult(true));
        }

        /// <summary>
        /// The Login POST request action.
        /// </summary>
        /// <param name="vm">The Identity Login view model from the POST request body.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while completing the request.</param>
        /// <returns>
        /// <see cref="StatusCodes.Status200OK"/> object result when the login attempt is successful.
        /// <see cref="StatusCodes.Status401Unauthorized"/> object result when the login attempt is unsuccessful.
        /// </returns>
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> PostLoginAsync([FromBody] IdentityLoginViewModel vm, CancellationToken cancellationToken) {
            // handle user already signed in
            if (_signInManager.IsSignedIn(User)) {
                try {
                    var currentUser = await GetCurrentUserAsync();

                    // attempted to sign in as someone other than themself
                    if (currentUser.NormalizedEmail != vm.Email.Normalize()) {
                        return StatusCode(StatusCodes.Status401Unauthorized, new IdentityLoginResult(false) {
                            Message = $"You are already signed in as {currentUser.Email}.",
                            Id = currentUser.Id
                        });
                    }
                    // attempted to sign in as themself
                    await _signInManager.RefreshSignInAsync(currentUser);
                    return StatusCode(StatusCodes.Status200OK, new IdentityLoginResult(true) {
                        Message = "Already signed in.",
                        Id = currentUser.Id
                    });
                } catch (Exception ex) {
                    Logger.LogError(ex, "Unexpected Exception during Login POST.");
                    return StatusCode(StatusCodes.Status500InternalServerError, new IdentityLoginResult(false) {
                        Message = "Internal server error.",
                        Error = ex.Message
                    });
                }
            }
            // attempt to sign in
            if (ModelState.IsValid) {
                try {
                    var result = await _signInManager.PasswordSignInAsync(vm.Email.Trim(), vm.Password, vm.PersistLogin, false);

                    if (result.Succeeded) {
                        // DC 11/12/20: Here we have to lookup the user from the UserManager since they are not yet
                        //              embedded into the HttpContext request header.
                        var currentUser = await UserManager.FindByEmailAsync(vm.Email.Trim());

                        Logger.LogInformation($"Identity {currentUser.Email} successfully authenticated.");
                        return StatusCode(StatusCodes.Status200OK, new IdentityLoginResult(true) {
                            Message = $"Signed in as {currentUser.Email}.",
                            Id = currentUser.Id
                        });
                    }
                    return StatusCode(StatusCodes.Status401Unauthorized, new IdentityLoginResult(false) {
                        Message = "The Username or Password is incorrect."
                    });
                } catch (Exception ex) {
                    Logger.LogError(ex, "Unexpected Exception during Login POST.");
                    return StatusCode(StatusCodes.Status500InternalServerError, new IdentityLoginResult(false) {
                        Message = "Internal server error.",
                        Error = ex.Message
                    });
                }
            }
            return StatusCode(StatusCodes.Status400BadRequest, new IdentityLoginResult(false) {
                Message = "The Username and Password fields are required."
            });
        }

        /// <summary>
        /// The Logout POST request action.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while completing the request.</param>
        /// <returns>
        /// <see cref="StatusCodes.Status200OK"/> object result when the login attempt is successful.
        /// </returns>
        [HttpPost("Logout")]
        [AllowAnonymous]
        public async Task<IActionResult> PostLogoutAsync(CancellationToken cancellationToken) {
            if (_signInManager.IsSignedIn(User)) {
                try {
                    var currentUser = await GetCurrentUserAsync();

                    Logger.LogInformation($"Identity {currentUser.Email} signing out.");
                    await _signInManager.SignOutAsync();
                    return StatusCode(StatusCodes.Status200OK, new IdentityLoginResult(true) {
                        Message = $"{currentUser.Email} signed out."
                    });
                } catch (Exception ex) {
                    Logger.LogError(ex, "Unexpected Exception during Login POST.");
                    return StatusCode(StatusCodes.Status500InternalServerError, new IdentityLoginResult(false) {
                        Message = "Internal server error.",
                        Error = ex.Message
                    });
                }
            }
            return StatusCode(StatusCodes.Status200OK, new IdentityLoginResult(true) {
                Message = $"No user to sign out."
            });
        }
    }
}
