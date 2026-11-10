using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.AspNetCore.SignalR.Auditing.TestApp;

public class AuditedNote : Entity<Guid>
{
    public string Text { get; set; }

    protected AuditedNote()
    {
    }

    public AuditedNote(Guid id, string text)
        : base(id)
    {
        Text = text;
    }
}
