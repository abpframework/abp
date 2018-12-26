using Volo.Abp.EventBus;

namespace SharedModule
{
    /// <summary>
    /// Used to send a text message from App1 to App2.
    /// </summary>
    [EventName("Test.App1ToApp2Text")] //Optional event name
    public class App1ToApp2TextEventData
    {
        public string TextMessage { get; set; }

        public App1ToApp2TextEventData()
        {

        }

        public App1ToApp2TextEventData(string textMessage)
        {
            TextMessage = textMessage;
        }
    }
}
