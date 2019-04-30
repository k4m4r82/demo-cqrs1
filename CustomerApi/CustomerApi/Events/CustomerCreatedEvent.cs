using CustomerApi.Models.Mongo;
using System.Collections.Generic;
using System.Linq;

namespace CustomerApi.Events
{
	public class CustomerCreatedEvent : IEvent
	{
		public long Id { get; set; }
		public string Email { get; set; }
		public string Name { get; set; }
		public int Age { get; set; }
		public List<PhoneCreatedEvent> Phones { get; set; }

		public CustomerEntity ToCustomerEntity()
		{
			return new CustomerEntity
			{
				Id = this.Id,
				Email = this.Email,
				Name = this.Name,
				Age = this.Age,
				Phones = this.Phones.Select(phone => new PhoneEntity {
					Type = phone.Type,
					AreaCode = phone.AreaCode,
					Number = phone.Number
				}).ToList()
			};
		}
	}
}
