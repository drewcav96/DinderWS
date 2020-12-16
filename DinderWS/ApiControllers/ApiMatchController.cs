using DinderWS.Data;
using DinderWS.Models.Match;
using DinderWS.Services;
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
    [Route("api/Match")]
    [ApiController]
    [Authorize]
    public class ApiMatchController : ApiBaseController<ApiMatchController> {
        private readonly IMatchingService _match;

        public ApiMatchController(ILogger<ApiMatchController> logger,
                UserManager<IdentityUser> userManager,
                ApplicationDbContext context,
                IMatchingService match)
                    : base(logger, userManager, context) {
            _match = match;
        }

        [HttpGet("Detail/{id}")]
        public async Task<IActionResult> GetDetailAsync(long id, CancellationToken cancellationToken) {
            try {
                var match = await Context.Matches
                    .Include(e => e.Experiences)
                        .ThenInclude(e => e.Profile)
                    .SingleOrDefaultAsync(e => e.Id == id, cancellationToken);

                if (match == null) {
                    return StatusCode(StatusCodes.Status404NotFound, new MatchDetailResult(null) {
                        Message = "The requested Experience could not be found."
                    });
                }
                var result = new MatchDetailResult(match);

                if (result.Success) {
                    return StatusCode(StatusCodes.Status200OK, result);
                }
                return StatusCode(StatusCodes.Status404NotFound, result);
            } catch (Exception ex) {
                Logger.LogError(ex, "Unexpected Exception during Match GET.");
                return StatusCode(StatusCodes.Status500InternalServerError, new MatchDetailResult(null) {
                    Message = "Internal server error.",
                    Error = ex.Message
                });
            }
        }

        [HttpPost("Reject/{id}")]
        public async Task<IActionResult> PostRejectAsync(string id, CancellationToken cancellationToken) {
            try {
                var experience = await Context.Experiences
                    .Include(e => e.Match)
                    .Include(e => e.Rejects)
                    .SingleOrDefaultAsync(e => e.Id == id, cancellationToken);

                if (experience == null) {
                    return StatusCode(StatusCodes.Status404NotFound, new MatchRejectResult(false, null));
                }
                var success = await _match.RejectMatch(experience.Id, cancellationToken);
                var result = new MatchRejectResult(success, experience);

                if (result.Success) {
                    return StatusCode(StatusCodes.Status200OK, result);
                }
                return StatusCode(StatusCodes.Status304NotModified, result);
            } catch (Exception ex) {
                Logger.LogError(ex, "Unexpected Exception during Match POST.");
                return StatusCode(StatusCodes.Status500InternalServerError, new MatchRejectResult(false, null) {
                    Message = "Internal server error.",
                    Error = ex.Message
                });
            }
        }

        [HttpGet("Find/{id}")]
        public async Task<IActionResult> GetFindAsync(string id, CancellationToken cancellationToken) {
            try {
                var experience = await Context.Experiences
                    .Include(e => e.Match)
                    .SingleOrDefaultAsync(e => e.Id == id, cancellationToken);

                if (experience == null) {
                    return StatusCode(StatusCodes.Status404NotFound, new MatchFindResult(null));
                }
                var matchId = await _match.FindOrCreateMatch(id, cancellationToken);
                var match = await Context.Matches
                    .SingleOrDefaultAsync(e => e.Id == matchId, cancellationToken);
                var result = new MatchFindResult(match);

                if (result.Success) {
                    experience.MatchId = matchId;
                    Context.Entry(experience).State = EntityState.Modified;
                    await Context.SaveChangesAsync(cancellationToken);
                    return StatusCode(StatusCodes.Status200OK, result);
                }
                return StatusCode(StatusCodes.Status304NotModified, result);
            } catch (Exception ex) {
                Logger.LogError(ex, "Unexpected Exception during Match POST.");
                return StatusCode(StatusCodes.Status500InternalServerError, new MatchFindResult(null) {
                    Message = "Internal server error.",
                    Error = ex.Message
                });
            }
        }
    }
}
