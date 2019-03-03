using Volo.Abp.EventBus;

namespace SharedModule
{
    /// <summary>
    /// Used to indicate that App2 has received a text message.
    /// </summary>
    [EventName("Test.App2TextReceived")] //Optional event name
    public class App2TextReceivedEventData
    {
        public string ReceivedText { get; set; }

        public App2TextReceivedEventData()
        {

        }

        public App2TextReceivedEventData(string receivedText)
        {
            ReceivedText = receivedText;
        }
    }
}