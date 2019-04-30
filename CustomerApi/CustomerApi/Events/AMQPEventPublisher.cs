using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace CustomerApi.Events
{
	public class AMQPEventPublisher
    {
		private readonly ConnectionFactory connectionFactory;

		public AMQPEventPublisher(IHostingEnvironment env)
		{
			connectionFactory = new ConnectionFactory();

			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
				.AddEnvironmentVariables();
			
			builder.Build().GetSection("amqp").Bind(connectionFactory);
		}

		public void PublishEvent<T>(T @event) where T : IEvent
		{
			using (IConnection conn = connectionFactory.CreateConnection())
			{
				using (IModel channel = conn.CreateModel())
				{
					var queue = @event is CustomerCreatedEvent ? 
						Constants.QUEUE_CUSTOMER_CREATED : @event is CustomerUpdatedEvent ? 
							Constants.QUEUE_CUSTOMER_UPDATED : Constants.QUEUE_CUSTOMER_DELETED;

					channel.QueueDeclare(
						queue: queue,
						durable: false,
						exclusive: false,
						autoDelete: false,
						arguments: null
					);

					var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event));

					channel.BasicPublish(
						exchange: "",
						routingKey: queue,
						basicProperties: null,
						body: body
					);
				}
			}
		}
	}
}
