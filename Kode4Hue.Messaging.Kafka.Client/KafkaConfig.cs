namespace Kode4Hue.Messaging.Kafka.Client
{
    public class KafkaConfig
    {
        //public KafkaConfig(KafkaBrokerConfig brokerConfiguration,
        //    List<KafkaProducerBuilderConfig>? kakfaProducerBuilderConfigurations = null,
        //    List<KafkaConsumerBuilderConfig>? kakfaConsumerBuilderConfigurations = null)
        //{
        //    BrokerConfiguration = brokerConfiguration;
        //    ProducerBuilderConfigurations = kakfaProducerBuilderConfigurations;
        //    ConsumerBuilderConfigurations = kakfaConsumerBuilderConfigurations;
        //}

        public KafkaBrokerConfig BrokerConfiguration { get; private set; }
        public List<KafkaProducerBuilderConfig>? ProducerBuilderConfigurations { get; private set; }
        public List<KafkaConsumerBuilderConfig>? ConsumerBuilderConfigurations { get; private set; }
    }
}
