using DinderWS.Data;
using DinderWS.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace DinderWS.ApiControllers {
    /// <summary>
    /// The API Login Controller.
    /// </summary>
    [Route("api/Login")]
    [ApiController]
    public class ApiLoginController : ApiBaseController<ApiLoginController> {
        private readonly SignInManager<IdentityUser> _signInManager;

        /// <summary>
        /// Constructs the controller with dependency injection.
        /// </summary>
        /// <param name="signInManager">The Sign In Manager service.</param>
        /// <param name="logger">The Logger instance.</param>
        /// <param name="userManager">The User Manager service.</param>
        /// <param name="context">The Application Database Context.</param>
        public ApiLoginController(SignInManager<IdentityUser> signInManager,
                ILogger<ApiLoginController> logger,
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
        /// <see cref="StatusCodes.Status200OK"/> object result containing the email address of the currently signed in user, or <see langword="null"/> if no user is signed in.
        /// </returns>
        [HttpGet]
        public IActionResult GetLogin(CancellationToken cancellationToken) {
            if (_signInManager.IsSignedIn(User)) {
                return Ok(User.Identity.Name);
            }
            return Ok();
        }

        /// <summary>
        /// The Login POST request action.
        /// </summary>
        /// <param name="vm">The Identity Login view model from the POST request body.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while completing the request.</param>
        /// <returns>
        /// <see cref="StatusCodes.Status200OK"/> object result containing the email address of the currently signed in user when the login attempt is successful.
        /// <see cref="StatusCodes.Status401Unauthorized"/> object result containing the view model and an error message when the login attempt is unsuccessful.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> PostLoginAsync([FromBody] IdentityLoginViewModel vm, CancellationToken cancellationToken) {
            if (_signInManager.IsSignedIn(User)) {
                return Ok(User.Identity.Name);
            }
            if (ModelState.IsValid) {
                var result = await _signInManager.PasswordSignInAsync(vm.Email, vm.Password, vm.Remember, false);

                if (result.Succeeded) {
                    var currentUser = await GetCurrentUserAsync();

                    Logger.LogInformation($"Identity {User.Identity.Name} successfully authenticated.");
                    return Ok(currentUser);
                }
                vm.Error = "The Username or Password is incorrect.";
            } else {
                vm.Error = "The Username or Password is required.";
            }
            vm.Password = null;
            return Unauthorized(vm);
        }
    }
}
