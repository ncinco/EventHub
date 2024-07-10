using Azure.Messaging.EventHubs.Consumer;
using System.Text;

namespace EventHub.Core
{
    public class TestResultBuilder
    {
        private readonly List<TestResult> _testResults;

        public TestResultBuilder()
        {
            _testResults = [];
        }

        public void AppendPartitionEvent(PartitionEvent receivedEvent)
        {
            var testResult = _testResults.FirstOrDefault(x =>
                x.BuildId == (string)receivedEvent.Data.Properties["buildId"]);

            if (testResult == null)
            {
                _testResults.Add(new TestResult
                {
                    BuildId = (string)receivedEvent.Data.Properties["buildId"],
                    TeamProjectId = (string)receivedEvent.Data.Properties["teamProjectId"],
                    PartitionEvent = [receivedEvent]
                });
            }
            else
            {
                testResult.PartitionEvent.Add(receivedEvent);
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            foreach (var testResult in _testResults)
            {
                builder.AppendLine($"BuildId: {testResult.BuildId}");
                builder.AppendLine($"TeamProjectId: {testResult.TeamProjectId}");

                foreach (var partitionEvent in testResult.PartitionEvent)
                {
                    builder.AppendLine($"partitionEvent: {partitionEvent.Data.EventBody}");
                }

                builder.AppendLine(string.Empty);
            }

            return builder.ToString();
        }
    }
}