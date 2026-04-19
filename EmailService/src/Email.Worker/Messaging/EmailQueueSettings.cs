
namespace Email.Worker.Messaging
{
    public sealed class EmailQueueSettings
    {
        public const string SectionName = "EmailQueue";
        public string ConnectionString { get; init; } = string.Empty;
        public string QueueName { get; init; } = string.Empty;
    }
}
