using CleanArchitecture.SharedLibrary.Messaging.Constants;
using CleanArchitecture.SharedLibrary.Messaging.Services;
using Confluent.Kafka;
using KafkaFlow;
using KafkaFlow.Producers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Kode4Hue.Messaging.Kafka.Client.Services
{
    public class KafkaMessagingService<T> : IMessagingService<T> where T : class
    {
        private readonly ILogger<KafkaMessagingService<T>> _logger;
        private readonly IMessageProducer _producer;

        public KafkaMessagingService(string producerName, IProducerAccessor producerAccessor, ILogger<KafkaMessagingService<T>> logger)
        {
            _producer = producerAccessor.GetProducer(producerName);
            _logger = logger;
        }

        public async Task<string> PublishMessage(string messageKey, T messageContent, CancellationToken cancellationToken)
        {
            var jsonMessageContent = JsonConvert.SerializeObject(messageContent);
            string status = PublisherMessageStatuses.Unknown;
            Message<string, string> message = new()
            {
                Value = jsonMessageContent
            };
            _logger.LogInformation($"Publishing message to key/topic {messageKey}");
            var result = await _producer.ProduceAsync(messageKey, message, cancellationToken);

            if (result.Status.Equals(PersistenceStatus.Persisted))
            {
                _logger.LogInformation($"Successfully published message to key/topic {messageKey}");
                status = PublisherMessageStatuses.Published;
            }

            if (result.Status.Equals(PersistenceStatus.NotPersisted))
            {
                _logger.LogError($"Error: message was published to key/topic {messageKey}, but not persisted");
                status = PublisherMessageStatuses.NotPublished;

            }

            if (result.Status.Equals(PersistenceStatus.PossiblyPersisted))
            {
                _logger.LogWarning($"Message was published to key/topic {messageKey}, however we are unsure if the message was persisted or not");
                status = PublisherMessageStatuses.PossiblyPublished;
            }

            return status;
        }
    }
}
