namespace Kode4Hue.Messaging.Kafka.Client
{
    public class KafkaBrokerConfig
    {

        private readonly string _brokerUrl;
        private readonly int _port;

        public KafkaBrokerConfig(string brokerUrl, int port)
        {
            _brokerUrl = brokerUrl;
            _port = port;

        }

        public string GetBrokerUrl()
        {
            return string.Concat(_brokerUrl, ":", _port);
        }
    }
}
