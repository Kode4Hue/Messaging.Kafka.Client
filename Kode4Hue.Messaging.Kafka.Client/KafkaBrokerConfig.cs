namespace Kode4Hue.Messaging.Kafka.Client
{
    public class KafkaBrokerConfig
    {

        public string Server { get; set; }
        public int Port { get; set; }

        public string GetBrokerUrl()
        {
            return string.Concat(Server, ":", Port);
        }
    }
}
