using Volo.Abp.EventBus;

namespace SharedModule
{
    /// <summary>
    /// Used to send a text message from App2 to App1.
    /// </summary>
    [EventName("Test.App2ToApp1Text")] //Optional event name
    public class App2ToApp1TextEventData
    {
        public string TextMessage { get; set; }

        public App2ToApp1TextEventData()
        {

        }

        public App2ToApp1TextEventData(string textMessage)
        {
            TextMessage = textMessage;
        }
    }
}