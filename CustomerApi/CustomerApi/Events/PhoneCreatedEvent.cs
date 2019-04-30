using CustomerApi.Models;

namespace CustomerApi.Events
{
	public class PhoneCreatedEvent : IEvent
	{
		public PhoneType Type { get; set; }
		public int AreaCode { get; set; }
		public int Number { get; set; }
	}
}
