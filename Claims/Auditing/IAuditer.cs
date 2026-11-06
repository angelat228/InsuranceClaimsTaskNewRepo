
namespace Claims.Auditing;
/// <summary>
/// Defines operations for recording claim and cover audit events.
/// </summary>
public interface IAuditer
{
    Task AuditClaimAsync(string id, string httpRequestType);

    Task AuditCoverAsync(string id, string httpRequestType);

}
