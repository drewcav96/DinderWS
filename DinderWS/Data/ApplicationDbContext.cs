using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DinderWS.Data {
    public sealed class ApplicationDbContext : IdentityDbContext {
        private readonly ILogger<ApplicationDbContext> _log;

        public ApplicationDbContext(ILogger<ApplicationDbContext> logger,
                DbContextOptions<ApplicationDbContext> options)
                    : base(options) {
            _log = logger;
        }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);
            _log.LogInformation($"Application Database Context connected to {Database.ProviderName}.");
        }
    }
}
