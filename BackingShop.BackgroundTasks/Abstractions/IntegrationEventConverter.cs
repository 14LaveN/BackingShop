using BackingShop.Application.Core.Abstractions.Messaging;
using BackingShop.Domain.Product.Events;
using BackingShop.RabbitMq.Messaging.Product.ProductCreated;
using BackingShop.RabbitMq.Messaging.User.Events.UserCreated;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BackingShop.BackgroundTasks.Abstractions;

public class IntegrationEventConverter : JsonConverter<IIntegrationEvent>
{
    public override IIntegrationEvent ReadJson(JsonReader reader, Type objectType, IIntegrationEvent existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JObject jsonObject = JObject.Load(reader);

        if (jsonObject.ToString() != "{}")
        {// You can determine the concrete type of the integration event based on a property in the JSON object
            var eventType = jsonObject.Value<string?>();

            // Create an instance of the concrete type based on the eventType
            IIntegrationEvent integrationEvent = eventType switch
            {
                "ProductCreatedIntegrationEvent" => jsonObject.ToObject<ProductCreatedIntegrationEvent>(),
                "UserCreatedIntegrationEvent" => jsonObject.ToObject<UserCreatedIntegrationEvent>(),
                // Add more cases for other concrete types if needed
                _ => throw new NotSupportedException($"Unsupported integration event type: {eventType}")
            };

            return integrationEvent;
        }

        return default;
    }

    public override void WriteJson(JsonWriter writer, IIntegrationEvent value, JsonSerializer serializer)
    {
        serializer.Serialize(writer, value);
    }
}