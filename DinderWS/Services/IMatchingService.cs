using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DinderWS.Services {
    public interface IMatchingService {
        Task<long> FindOrCreateMatch(string id, CancellationToken cancellationToken);
        Task<bool> RejectMatch(string id, CancellationToken cancellationToken);
    }
}
