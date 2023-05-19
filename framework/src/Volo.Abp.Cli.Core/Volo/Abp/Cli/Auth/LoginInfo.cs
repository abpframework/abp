using System;

namespace Volo.Abp.Cli.Auth;

public class LoginInfo
{
    public Guid? Id { get; set; }
    
    public string Name { get; set; }

    public string Surname { get; set; }

    public string Username { get; set; }

    public string EmailAddress { get; set; }

    public string Organization { get; set; }

    public bool HasSourceCodeAccess { get; set; }
}
