
namespace Claims.Auditing;

/// <summary>
/// Queues claim and cover audit events for background processing.
/// </summary>
public class Auditer : IAuditer
{
    private readonly AuditQueue _auditQueue;

    public Auditer(AuditQueue auditQueue)
    {
        _auditQueue = auditQueue;
    }

    public async Task AuditClaimAsync(string id, string httpRequestType)
    {
        var auditEvent = new AuditEvent("Claim", id, httpRequestType, DateTime.UtcNow);

        await _auditQueue.EnqueueAsync(auditEvent);
    }
     
    public async Task AuditCoverAsync(string id, string httpRequestType)
    {
        var auditEvent = new AuditEvent("Cover", id, httpRequestType, DateTime.UtcNow);

        await _auditQueue.EnqueueAsync(auditEvent);
    }
}
