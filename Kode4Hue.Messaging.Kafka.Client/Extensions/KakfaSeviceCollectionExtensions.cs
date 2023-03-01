using KafkaFlow;
using KafkaFlow.Serializer;
using KafkaFlow.TypedHandler;
using Microsoft.Extensions.DependencyInjection;

namespace Kode4Hue.Messaging.Kafka.Client.Extensions
{
    public static class KafkaServiceCollectionExtensions
    {
        public static IServiceCollection AddKafkaConfiguration(
            this IServiceCollection services,
            KafkaConfig config)
        {


            services.AddKafka(kafka =>
                kafka.AddCluster(cluster =>
                {
                    cluster
                        .WithBrokers(new[] { config.BrokerConfiguration.GetBrokerUrl() });

                    if (config.ProducerBuilderConfigurations is not null)
                    {
                        foreach (var producerBuilderConfiguration in config.ProducerBuilderConfigurations)
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
                    }


                    if (config.ConsumerBuilderConfigurations is not null)
                    {

                        foreach (var consumerBuilderConfiguration in config.ConsumerBuilderConfigurations)
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
                    }

                }));

            return services;
        }
    }
}
