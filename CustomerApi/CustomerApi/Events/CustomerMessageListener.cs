using CustomerApi.Models.Mongo;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;
using System;
using System.Text;
using System.Threading;

namespace CustomerApi.Events
{
	public class CustomerMessageListener
	{
		private readonly CustomerMongoRepository _repository;

		public CustomerMessageListener(CustomerMongoRepository repository)
		{
			_repository = repository;
		}

		public void Start(string contentRootPath)
		{
			ConnectionFactory connectionFactory = new ConnectionFactory();

			var builder = new ConfigurationBuilder()
				.SetBasePath(contentRootPath)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
				.AddEnvironmentVariables();

			builder.Build().GetSection("amqp").Bind(connectionFactory);

			connectionFactory.AutomaticRecoveryEnabled = true;
			connectionFactory.NetworkRecoveryInterval = TimeSpan.FromSeconds(15);

			using (IConnection conn = connectionFactory.CreateConnection())
			{
				using (IModel channel = conn.CreateModel())
				{
					DeclareQueues(channel);

					var subscriptionCreated = new Subscription(channel, Constants.QUEUE_CUSTOMER_CREATED, false);
					var subscriptionUpdated = new Subscription(channel, Constants.QUEUE_CUSTOMER_UPDATED, false);
					var subscriptionDeleted = new Subscription(channel, Constants.QUEUE_CUSTOMER_DELETED, false);
					while (true)
					{
                        // Sleeps for 5 sec before trying again
						Thread.Sleep(5000);

						new Thread(() =>
						{
							ListerCreated(subscriptionCreated);
						}).Start();

						new Thread(() =>
						{
							ListenUpdated(subscriptionUpdated);
						}).Start();

						new Thread(() =>
						{
							ListenDeleted(subscriptionDeleted);
						}).Start();
					}
				}
			}
		}

		private void ListenDeleted(Subscription subscriptionDeleted)
		{
			BasicDeliverEventArgs eventArgsDeleted = subscriptionDeleted.Next();
			if (eventArgsDeleted != null)
			{
				string messageContent = Encoding.UTF8.GetString(eventArgsDeleted.Body);

				CustomerDeletedEvent _deleted = JsonConvert.DeserializeObject<CustomerDeletedEvent>(messageContent);

				_repository.Remove(_deleted.Id);

                subscriptionDeleted.Ack(eventArgsDeleted);
			}
		}

		private void ListenUpdated(Subscription subscriptionUpdated)
		{
			BasicDeliverEventArgs eventArgsUpdated = subscriptionUpdated.Next();
			if (eventArgsUpdated != null)
			{
				string messageContent = Encoding.UTF8.GetString(eventArgsUpdated.Body);

				CustomerUpdatedEvent _updated = JsonConvert.DeserializeObject<CustomerUpdatedEvent>(messageContent);

				_repository.Update(_updated.ToCustomerEntity(_repository.GetCustomer(_updated.Id)));

                subscriptionUpdated.Ack(eventArgsUpdated);
			}
		}

		private void ListerCreated(Subscription subscriptionCreated)
		{
			BasicDeliverEventArgs eventArgsCreated = subscriptionCreated.Next();
			if (eventArgsCreated != null)
			{
				string messageContent = Encoding.UTF8.GetString(eventArgsCreated.Body);

				CustomerCreatedEvent _created = JsonConvert.DeserializeObject<CustomerCreatedEvent>(messageContent);

				_repository.Create(_created.ToCustomerEntity());

				subscriptionCreated.Ack(eventArgsCreated);
			}
		}

		private static void DeclareQueues(IModel channel)
		{
			channel.QueueDeclare(
				queue: Constants.QUEUE_CUSTOMER_CREATED,
				durable: false,
				exclusive: false,
				autoDelete: false,
				arguments: null
			);

			channel.QueueDeclare(
				queue: Constants.QUEUE_CUSTOMER_UPDATED,
				durable: false,
				exclusive: false,
				autoDelete: false,
				arguments: null
			);

			channel.QueueDeclare(
				queue: Constants.QUEUE_CUSTOMER_DELETED,
				durable: false,
				exclusive: false,
				autoDelete: false,
				arguments: null
			);
		}
	}
}
