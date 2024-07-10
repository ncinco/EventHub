using Azure.Messaging.EventHubs.Consumer;

namespace EventHub.Core
{
    public class TestResult
    {
        public TestResult()
        {
            PartitionEvent = new List<PartitionEvent>();
        }

        public required string BuildId { get; set; }

        public required string TeamProjectId { get; set; }

        public required List<PartitionEvent> PartitionEvent { get; set; }
    }
}
