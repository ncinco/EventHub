using Azure.Messaging.EventHubs.Consumer;

namespace EventHub.Consumer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConsumeMessagesAsync().GetAwaiter().GetResult();
        }

        private static async Task ConsumeMessagesAsync()
        {
            var connectionString = "";
            var eventHubName = "tests-results";

            string consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;

            // It is recommended that you cache the Event Hubs clients for the lifetime of your
            // application, closing or disposing when application ends.  This example disposes
            // after the immediate scope for simplicity.

            await using (var consumer = new EventHubConsumerClient(consumerGroup, connectionString, eventHubName))
            {
                using var cancellationSource = new CancellationTokenSource();
                cancellationSource.CancelAfter(TimeSpan.FromSeconds(45));

                await foreach (PartitionEvent receivedEvent in consumer.ReadEventsAsync(cancellationSource.Token))
                {
                    // At this point, the loop will wait for events to be available in the Event Hub.  When an event
                    // is available, the loop will iterate with the event that was received.  Because we did not
                    // specify a maximum wait time, the loop will wait forever unless cancellation is requested using
                    // the cancellation token.

                    Console.WriteLine(receivedEvent);
                }
            }
        }
    }
}