using Azure.Messaging.EventHubs.Consumer;
using EventHub.Core;

namespace EventHub.Consumer
{
    internal class Program
    {
        private static TestResultBuilder testResultBuilder = new TestResultBuilder();

        static void Main(string[] args)
        {
            ConsumeMessagesAsync().GetAwaiter().GetResult();
        }

        private static async Task ConsumeMessagesAsync()
        {
            var connectionString = "";
            var eventHubName = "tests-results";

            string consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;

            await using (var consumer = new EventHubConsumerClient(consumerGroup, connectionString, eventHubName))
            {
                using var cancellationSource = new CancellationTokenSource();

                await foreach (PartitionEvent receivedEvent in consumer.ReadEventsAsync(cancellationSource.Token))
                {
                    testResultBuilder.AppendPartitionEvent(receivedEvent);

                    Console.WriteLine(testResultBuilder.ToString());
                }
            }
        }
    }
}