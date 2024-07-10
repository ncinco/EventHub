using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using System.Text;

namespace EventHub
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ProductMessagesAsync().GetAwaiter().GetResult();
        }

        private static async Task ProductMessagesAsync()
        {
            var connectionString = "";
            var eventHubName = "tests-results";

            int numOfEvents = 3;

            EventHubProducerClient producerClient = new EventHubProducerClient(connectionString, eventHubName);

            using EventDataBatch eventBatch = await producerClient.CreateBatchAsync();

            for (int i = 1; i <= numOfEvents; i++)
            {
                var eventData = new EventData(Encoding.UTF8.GetBytes($"Event {i}"));
                eventData.Properties.Add("buildId", "3000");
                eventData.Properties.Add("teamProjectId", "e58a3f8d-f5e0-4aca-b05c-475f664f76c0");

                if (!eventBatch.TryAdd(eventData))
                {
                    // if it is too large for the batch
                    throw new Exception($"Event {i} is too large for the batch and cannot be sent.");
                }
            }

            try
            {
                // Use the producer client to send the batch of events to the event hub
                await producerClient.SendAsync(eventBatch);
                Console.WriteLine($"A batch of {numOfEvents} events has been published.");
                Console.ReadLine();
            }
            finally
            {
                await producerClient.DisposeAsync();
            }
        }
    }
}