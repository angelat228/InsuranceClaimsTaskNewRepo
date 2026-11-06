
using Claims.Data;

namespace Claims.Auditing;

/// <summary>
/// Processes queued audit events and saves them to the audit database.
/// </summary>
public class AuditBackgroundService : BackgroundService
{
    private readonly AuditQueue _auditQueue;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<AuditBackgroundService> _logger;

    public AuditBackgroundService(AuditQueue auditQueue, IServiceScopeFactory scopeFactory, ILogger<AuditBackgroundService> logger)
    {
        _auditQueue = auditQueue;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Audit background service started.");

        await foreach (var auditEvent in _auditQueue.ReadAllAsync(stoppingToken))
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();

                var auditContext = scope.ServiceProvider
                    .GetRequiredService<AuditContext>();

                if (auditEvent.EntityType == "Claim")
                {
                    auditContext.Add(new ClaimAudit
                    {
                        Created = auditEvent.Created,
                        HttpRequestType = auditEvent.HttpRequestType,
                        ClaimId = auditEvent.EntityId
                    });
                }
                else if (auditEvent.EntityType == "Cover")
                {
                    auditContext.Add(new CoverAudit
                    {
                        Created = auditEvent.Created,
                        HttpRequestType = auditEvent.HttpRequestType,
                        CoverId = auditEvent.EntityId
                    });
                }
                else
                {
                    _logger.LogWarning("Unknown audit entity type: {EntityType}", auditEvent.EntityType);

                    continue;
                }

                await auditContext.SaveChangesAsync(stoppingToken);

                _logger.LogInformation("Successfully saved audit event for {EntityType} {EntityId} with request type {RequestType}", auditEvent.EntityType, auditEvent.EntityId, auditEvent.HttpRequestType);
            }
            catch (OperationCanceledException)
                when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save audit event for {EntityType} {EntityId}", auditEvent.EntityType, auditEvent.EntityId);
            }
        }
    }
}
