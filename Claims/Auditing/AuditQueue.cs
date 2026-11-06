
using System.Threading.Channels;

namespace Claims.Auditing;

/// <summary>
/// Stores audit events in an in-memory queue for background processing.
/// </summary>
public class AuditQueue
{
    private readonly Channel<AuditEvent> _queue = Channel.CreateUnbounded<AuditEvent>();

    /// <summary>
    /// Adds an audit event to the queue.
    /// </summary>
    public async ValueTask EnqueueAsync(AuditEvent auditEvent)
    {
        await _queue.Writer.WriteAsync(auditEvent);
    }

    /// <summary>
    /// Reads queued audit events as they become available.
    /// </summary>
    public IAsyncEnumerable<AuditEvent> ReadAllAsync(CancellationToken cancellationToken)
    {
        return _queue.Reader.ReadAllAsync(cancellationToken);
    }
}
