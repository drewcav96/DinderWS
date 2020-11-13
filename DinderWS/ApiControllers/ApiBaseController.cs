using DinderWS.Data;
using DinderWS.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System.Threading.Tasks;

namespace DinderWS.ApiControllers {
    /// <summary>
    /// Base abstract class for all API Controllers.
    /// </summary>
    /// <typeparam name="TControllerBase">The inheriting API Controller type.</typeparam>
    public abstract class ApiBaseController<TControllerBase> : ControllerBase
            where TControllerBase : ControllerBase {
        protected readonly UserManager<IdentityUser> UserManager;

        /// <summary>
        /// The Logger instance.
        /// </summary>
        protected readonly ILogger<TControllerBase> Logger;
        /// <summary>
        /// The Application Database Context.
        /// </summary>
        protected readonly ApplicationDbContext Context;

        /// <summary>
        /// The Display name of this Controller instance.
        /// </summary>
        public static string ControllerName {
            get {
                var name = typeof(TControllerBase).Name;
                var indexOf = name.LastIndexOf(nameof(Controller));

                if (indexOf > 0) {
                    return name.Substring(0, indexOf);
                }
                return name;
            }
        }

        /// <summary>
        /// The URI of the current action to return to.
        /// </summary>
        public string RefererUrl {
            get => Request.Headers[HeaderNames.Referer].ToString();
        }

        /// <summary>
        /// Constructs the controller with dependency injection.
        /// </summary>
        /// <param name="logger">The Logger instance.</param>
        /// <param name="userManager">The User Manager service.</param>
        /// <param name="context">The Application Database Context.</param>
        public ApiBaseController(ILogger<TControllerBase> logger,
                UserManager<IdentityUser> userManager,
                ApplicationDbContext context) {
            Logger = logger;
            UserManager = userManager;
            Context = context;
        }

        /// <summary>
        /// Asynchronously gets the current User Identity.
        /// </summary>
        /// <returns>The currently signed in User, or <see langword="null"/> if no User is signed in.</returns>
        protected async Task<IdentityUser> GetCurrentUserAsync() {
            return await UserManager.GetUserAsync(User);
        }
    }
}
