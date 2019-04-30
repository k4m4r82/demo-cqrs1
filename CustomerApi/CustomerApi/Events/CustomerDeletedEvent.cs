namespace CustomerApi.Events
{
	public class CustomerDeletedEvent : IEvent
	{
		public long Id { get; set; }
	}
}
