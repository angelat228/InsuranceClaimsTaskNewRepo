using Microsoft.EntityFrameworkCore;

namespace Claims.Auditing
{
    /// <summary>
    /// Provides database access for claim and cover audit records.
    /// </summary>
    public class AuditContext : DbContext
    {
        public AuditContext(DbContextOptions<AuditContext> options) : base(options)
        {
        }
        public DbSet<ClaimAudit> ClaimAudits { get; set; }
        public DbSet<CoverAudit> CoverAudits { get; set; }
    }
}
