using System.Collections.Generic;

namespace Volo.Abp.Domain.Values;

public class EmailAddress : ValueObject
{
    public string Email { get; }

    private EmailAddress()
    {
    }

    public EmailAddress(string email)
    {
        Email = email;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        if (Email != null)
        {
            yield return Email;
        }
    }
}
