using CustomerApi.Models.Mongo;
using System.Collections.Generic;
using System.Linq;

namespace CustomerApi.Events
{
	public class CustomerUpdatedEvent : IEvent
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public int Age { get; set; }
		public List<PhoneCreatedEvent> Phones { get; set; }

		public CustomerEntity ToCustomerEntity(CustomerEntity entity)
		{
			return new CustomerEntity
			{
				Id = this.Id,
				Email = entity.Email,
				Name = entity.Name.Equals(this.Name) ? entity.Name : this.Name,
				Age = entity.Age.Equals(this.Age) ? entity.Age : this.Age,
				Phones = GetNewOnes(entity.Phones).Select(phone => new PhoneEntity { AreaCode = phone.AreaCode, Number = phone.Number }).ToList()
			};
		}

		private List<PhoneEntity> GetNewOnes(List<PhoneEntity> Phones)
		{
			return Phones.Where(a => !this.Phones.Any(x => x.Type == a.Type
				&& x.AreaCode == a.AreaCode
				&& x.Number == a.Number)).ToList<PhoneEntity>();
		}
	}
}
