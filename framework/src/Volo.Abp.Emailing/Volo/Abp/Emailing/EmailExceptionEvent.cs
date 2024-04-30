using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Volo.Abp.Emailing;
[Serializable]
public class EmailExceptionEvent
{
    public string? Subject { get; set; }
    public string? From { get; set; }
    public string? Sender { get; set; }
    public List<string>? To { get; set; }
    public string? MailFailDescription { get; set; }
    public string? Body { get; set; }
    public DateTime Date { get; set; }
}
