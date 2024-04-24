using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Volo.Abp.Emailing;
[Serializable]
public class EmailExceptionEvent
{
    public string? Subject { get; set; }
    public MailAddress? From { get; set; }
    public MailAddressCollection? To { get; set; }
    public MailAddress? Sender { get; set; }
    public string? MailFailDescription { get; set; }
    public string? Body { get; set; }
    public DateTime Date { get; set; }
}
