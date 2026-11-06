
namespace Claims.Auditing;

/// <summary>
/// Represents an event that needs to be written to the audit database.
/// </summary>
public record AuditEvent(string EntityType, string EntityId, string HttpRequestType, DateTime Created);
