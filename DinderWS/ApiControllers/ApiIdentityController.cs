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
    [AllowAnonymous]
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
        /// <see cref="StatusCodes.Status500InternalServerError"/> object result when there was an unexpected Exception.
        /// </returns>
        [HttpGet("Login")]
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
        /// <see cref="StatusCodes.Status500InternalServerError"/> object result when there was an unexpected Exception.
        /// </returns>
        [HttpPost("Login")]
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
        /// <see cref="StatusCodes.Status500InternalServerError"/> object result when there was an unexpected Exception.
        /// </returns>
        [HttpPost("Logout")]
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

        /// <summary>
        /// The Create POST request action.
        /// </summary>
        /// <param name="vm">The Identity Create view model from the POST request body.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while completing the request.</param>
        /// <returns>
        /// <see cref="StatusCodes.Status201Created"/> object result when the User is created.
        /// <see cref="StatusCodes.Status403Forbidden"/> object result when the User is already logged in.
        /// <see cref="StatusCodes.Status409Conflict"/> object result when the User already exists.
        /// <see cref="StatusCodes.Status422UnprocessableEntity"/> object result when the view model contains validation errors.
        /// <see cref="StatusCodes.Status500InternalServerError"/> object result when there was an unexpected Exception.
        /// </returns>
        [HttpPost("Create")]
        public async Task<IActionResult> PostCreateAsync(IdentityCreateViewModel vm, CancellationToken cancellationToken) {
            // don't create a new user if we're already signed in
            if (_signInManager.IsSignedIn(User)) {
                var currentUser = await GetCurrentUserAsync();

                return StatusCode(StatusCodes.Status403Forbidden, new IdentityCreateResult(false) {
                    Message = $"{currentUser.Email} is already signed in!"
                });
            }
            // create the new user
            if (ModelState.IsValid) {
                try {
                    var existingUser = UserManager.FindByEmailAsync(vm.Email);

                    if (existingUser != null) {
                        return StatusCode(StatusCodes.Status409Conflict, new IdentityCreateResult(false) {
                            Message = $"{vm.Email} is already registered!"
                        });
                    }
                    var newUser = vm.Create();
                    var result = await UserManager.CreateAsync(newUser, vm.Password);

                    // reload the new user from the context so all of its properties are populated
                    newUser = await UserManager.FindByEmailAsync(newUser.Email);
                    await _signInManager.SignInAsync(newUser, false);
                    return StatusCode(StatusCodes.Status201Created, new IdentityCreateResult(true) {
                        Message = "New User successfully created.",
                        Id = newUser.Id
                    });
                } catch (Exception ex) {
                    Logger.LogError(ex, "Unexpected Exception during Create POST.");
                    return StatusCode(StatusCodes.Status500InternalServerError, new IdentityCreateResult(false) {
                        Message = "Internal server error.",
                        Error = ex.Message
                    });
                }
            }
            return StatusCode(StatusCodes.Status422UnprocessableEntity, new IdentityCreateResult(false) {
                Message = "The view model failed validation checks.",
                ValidationErrors = ModelStateErrors.Errors
            });
        }
    }
}
