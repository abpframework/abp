using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.Users;

namespace Volo.Abp.Identity;

public class IdentitySessionManager : DomainService
{
    protected IIdentitySessionRepository IdentitySessionRepository { get; }
    protected ICurrentUser CurrentUser { get; }

    public IdentitySessionManager(IIdentitySessionRepository identitySessionRepository, ICurrentUser currentUser)
    {
        IdentitySessionRepository = identitySessionRepository;
        CurrentUser = currentUser;
    }

    public virtual async Task<IdentitySession> CreateAsync(
        string sessionId,
        string device,
        string deviceInfo,
        Guid userId,
        Guid? tenantId,
        string clientId,
        string ipAddresses)
    {
        Check.NotNullOrWhiteSpace(sessionId, nameof(sessionId));
        Check.NotNullOrWhiteSpace(device, nameof(device));

        var session = new IdentitySession(
            GuidGenerator.Create(),
            sessionId,
            device,
            deviceInfo,
            userId,
            tenantId,
            clientId,
            ipAddresses,
            Clock.Now
        );

        return await IdentitySessionRepository.InsertAsync(session);
    }

    public virtual async Task UpdateAsync(IdentitySession session)
    {
        await IdentitySessionRepository.UpdateAsync(session);
    }

    public virtual async Task<List<IdentitySession>> GetListAsync(Guid userId)
    {
        return await IdentitySessionRepository.GetListAsync(userId);
    }

    public virtual async Task<IdentitySession> GetAsync(Guid id)
    {
        return await IdentitySessionRepository.GetAsync(id);
    }

    public virtual async Task<IdentitySession> FindAsync(Guid id)
    {
        return await IdentitySessionRepository.FindAsync(id);
    }

    public virtual async Task<IdentitySession> GetAsync(string sessionId)
    {
        return await IdentitySessionRepository.GetAsync(sessionId);
    }

    public virtual async Task<IdentitySession> FindAsync(string sessionId)
    {
        return await IdentitySessionRepository.FindAsync(sessionId);
    }

    public virtual async Task RevokeAsync(Guid id)
    {
        var session = await IdentitySessionRepository.GetAsync(id);
        await IdentitySessionRepository.DeleteAsync(session);
    }

    public virtual async Task RevokeAsync(string sessionId)
    {
        var session = await IdentitySessionRepository.GetAsync(sessionId);
        await IdentitySessionRepository.DeleteAsync(session);
    }

    public virtual async Task RevokeAllAsync(Guid userId, Guid? exceptSessionId = null)
    {
        await IdentitySessionRepository.DeleteAllAsync(userId, exceptSessionId);
    }

    public virtual async Task RevokeAllAsync(Guid userId, string device, Guid? exceptSessionId = null)
    {
        await IdentitySessionRepository.DeleteAllAsync(userId, device, exceptSessionId);
    }
}
