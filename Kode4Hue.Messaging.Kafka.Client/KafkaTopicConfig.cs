namespace Kode4Hue.Messaging.Kafka.Client
{
    public class KafkaTopicConfig
    {
        public string TopicName { get; set; }
        public int NumberOfPartitions { get; set; }
        public short ReplicationFactor { get; set; }
    }
}
