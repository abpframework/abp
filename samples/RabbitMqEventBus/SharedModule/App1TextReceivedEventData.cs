using Volo.Abp.EventBus;

namespace SharedModule
{
    /// <summary>
    /// Used to indicate that App2 has received a text message.
    /// </summary>
    [EventName("Test.App1TextReceived")] //Optional event name
    public class App1TextReceivedEventData
    {
        public string ReceivedText { get; set; }

        public App1TextReceivedEventData()
        {
            
        }

        public App1TextReceivedEventData(string receivedText)
        {
            ReceivedText = receivedText;
        }
    }
}
