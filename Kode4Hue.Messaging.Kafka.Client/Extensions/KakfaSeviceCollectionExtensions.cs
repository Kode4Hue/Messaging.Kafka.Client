using KafkaFlow;
using KafkaFlow.Serializer;
using KafkaFlow.TypedHandler;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Kode4Hue.Messaging.Kafka.Client.Extensions
{
    public static class KafkaServiceCollectionExtensions
    {
        public static IServiceCollection AddKafkaConfiguration(
            this IServiceCollection services,
            IConfigurationRoot configurationRoot)
        {
            var kafkaConfig = configurationRoot.GenerateKafkaConfig();

            if (kafkaConfig.ProducerBuilderConfigurations?.Count > 0)
            {
                services.AddKafka(kafka =>
                kafka.AddCluster(cluster =>
                {
                    cluster
                        .WithBrokers(new[] { kafkaConfig.BrokerConfiguration.GetBrokerUrl() });


                    foreach (var producerBuilderConfiguration in kafkaConfig.ProducerBuilderConfigurations)
                    {
                        var topicConfiguration = producerBuilderConfiguration.TopicConfiguration;
                        cluster.CreateTopicIfNotExists(
                                            topicConfiguration.TopicName,
                                            numberOfPartitions: topicConfiguration.NumberOfPartitions,
                                            replicationFactor: topicConfiguration.ReplicationFactor);
                        cluster.AddProducer(
                                            producerBuilderConfiguration.ProducerName,
                                            producer: producer => producer
                                                .DefaultTopic(topicConfiguration.TopicName)
                                                .AddMiddlewares(middlewares => middlewares
                                                    .AddSerializer<ProtobufNetSerializer>()));
                    }
                }));

            }

            if (kafkaConfig.ConsumerBuilderConfigurations?.Count > 0)
            {

                services.AddKafkaFlowHostedService(
                kafka => kafka
                    .UseMicrosoftLog()
                    .AddCluster(cluster =>
                    {
                        cluster
                            .WithBrokers(new[] { kafkaConfig.BrokerConfiguration.GetBrokerUrl() });


                        foreach (var consumerBuilderConfiguration in kafkaConfig.ConsumerBuilderConfigurations)
                        {
                            var topicConfiguration = consumerBuilderConfiguration.TopicConfiguration;

                            cluster.AddConsumer(consumer => consumer
                                .Topic(topicConfiguration.TopicName)
                                .WithGroupId(consumerBuilderConfiguration.ConsumerGroupName)
                                .WithBufferSize(consumerBuilderConfiguration.BufferSize)
                                .WithWorkersCount(consumerBuilderConfiguration.WorkersCount)
                                .WithAutoOffsetReset(consumerBuilderConfiguration.OffsetReset)
                                .AddMiddlewares(middlewares => middlewares
                                    .AddSerializer<ProtobufNetSerializer>()
                                    .AddTypedHandlers(handlers =>
                                        handlers.AddHandlers(consumerBuilderConfiguration.MessageHandlers.Select(x => x.GetType())))
                                )
                            );

                        }
                    }));

            }

            return services;
        }
    }
}
