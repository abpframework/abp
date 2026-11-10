using System;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace Volo.Abp.AspNetCore.SignalR.Auditing.TestApp;

[Audited]
public class AuditedNoteService : ITransientDependency
{
    private readonly IRepository<AuditedNote, Guid> _noteRepository;
    private readonly AuditedWorkSignal _signal;

    public AuditedNoteService(IRepository<AuditedNote, Guid> noteRepository, AuditedWorkSignal signal)
    {
        _noteRepository = noteRepository;
        _signal = signal;
    }

    [UnitOfWork]
    public virtual async Task UpdateTextAsync(Guid id, string text)
    {
        await _signal.Task;

        var note = await _noteRepository.GetAsync(id);
        note.Text = text;
        await _noteRepository.UpdateAsync(note, autoSave: true);
    }

    [UnitOfWork]
    public virtual async Task UpdateTextWithoutSavingAsync(Guid id, string text)
    {
        await _signal.Task;

        // The change is saved when the unit of work completes.
        var note = await _noteRepository.GetAsync(id);
        note.Text = text;
    }
}

public class AuditedWorkSignal : ISingletonDependency
{
    private readonly TaskCompletionSource _completionSource = new(TaskCreationOptions.RunContinuationsAsynchronously);

    public Task Task => _completionSource.Task;

    public void Complete() => _completionSource.TrySetResult();
}
