using System;

namespace Volo.Abp.Emailing
{
    [Serializable]
    public class BackgroundEmailSendingJobArgs
    {
        public string From { get; set; }

        public string To { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        /// <summary>
        /// Default: true.
        /// </summary>
        public bool IsBodyHtml { get; set; } = true;

        //TODO: Add other properties and attachments
    }
}
