using Microsoft.Extensions.Configuration;

namespace Kode4Hue.Messaging.Kafka.Client.Extensions
{
    public static class ConfigurationExtensions
    {
        public static KafkaConfig GenerateKafkaConfig(this IConfigurationRoot configurationRoot)
        {
            KafkaConfig kafkaConfig = new();
                
            configurationRoot.GetSection(nameof(KafkaConfig))
                .Bind(kafkaConfig);
            return kafkaConfig;
        }
    }
}
