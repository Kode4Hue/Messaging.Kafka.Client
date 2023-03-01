using KafkaFlow;
using KafkaFlow.TypedHandler;

namespace Kode4Hue.Messaging.Kafka.Client
{
    public class KafkaConsumerBuilderConfig
    {
        public required KafkaTopicConfig TopicConfiguration { get; set; }
        public required string ConsumerGroupName { get; set; }
        public required int BufferSize { get; set; }
        public required int WorkersCount { get; set; }
        public required AutoOffsetReset OffsetReset { get; set; }
        public required List<IMessageHandler> MessageHandlers { get; set; }
    }
}
