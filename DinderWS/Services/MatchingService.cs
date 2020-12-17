using DinderWS.Data;
using DinderWS.Enums;
using DinderWS.Models.Match;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DinderWS.Services {
    public class MatchingService : IMatchingService {
        private readonly ILogger<MatchingService> _log;
        private readonly ApplicationDbContext _db;

        public MatchingService(ILogger<MatchingService> logger,
                ApplicationDbContext context) {
            _log = logger;
            _db = context;
        }

        public async Task<long> FindOrCreateMatch(string id, CancellationToken cancellationToken = default) {
            var experience = await _db.Experiences
                .Include(e => e.Rejects)
                .SingleAsync(e => e.Id == id, cancellationToken);
            var matchQuery = _db.Matches
                .Include(e => e.Experiences)
                .AsQueryable();

            if (experience.GroupSize != EGroupSize.Any) {
                matchQuery = matchQuery
                    .Where(e => e.GroupSize == experience.GroupSize);
            }
            if (experience.GenderPreference != EGender.None) {
                matchQuery = matchQuery
                    .Where(e => e.Gender == experience.GenderPreference);
            }
            if (experience.CuisineType != ECuisineType.Any) {
                matchQuery = matchQuery
                    .Where(e => e.CuisineType == experience.CuisineType);
            }
            var matches = await matchQuery
                .OrderBy(e => e.Timestamp)
                .ToListAsync(cancellationToken);

            if (matches.Any()) {
                foreach (var match in matches) {
                    var rejected = experience.Rejects
                        .Any(e => e.MatchId == match.Id);

                    if (rejected) {
                        continue;
                    }
                    if (!match.IsFull) {
                        var success = match.AddExperience(experience);

                        if (success) {
                            _db.Entry(match).State = EntityState.Modified;
                            await _db.SaveChangesAsync(cancellationToken);
                            return match.Id;
                        }
                    }
                }
            }
            if (!experience.MatchId.HasValue) {
                var match = new Match {
                    GroupSize = experience.GroupSize,
                    Gender = experience.GenderPreference,
                    CuisineType = experience.CuisineType,
                    AvgLongitude = experience.Longitude,
                    AvgLatitude = experience.Latitude,
                    Timestamp = DateTimeOffset.UtcNow
                };

                _db.Entry(match).State = EntityState.Added;
                await _db.SaveChangesAsync(cancellationToken);
                return match.Id;
            }
            return experience.MatchId.Value;
        }

        public async Task<bool> RejectMatch(string id, CancellationToken cancellationToken = default) {
            var experience = await _db.Experiences
                .Include(e => e.Match)
                .SingleAsync(e => e.Id == id, cancellationToken);

            if (experience.MatchId.HasValue) {
                var success = experience.RejectMatch();

                if (success) {
                    _db.Entry(experience).State = EntityState.Modified;
                    await _db.SaveChangesAsync(cancellationToken);
                }
                return success;
            }
            return false;
        }
    }
}
