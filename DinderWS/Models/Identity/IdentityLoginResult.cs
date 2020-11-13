using DinderWS.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DinderWS.Models.Identity {
    public class IdentityLoginResult : IResult {
        public bool Success { get; private set; }
        public string Message { get; set; }
        public string Error { get; set; }
        public string Id { get; set; }

        public IdentityLoginResult(bool success) {
            Success = success;
        }
    }
}
