using CustomerApi.Events;

namespace CustomerApi.Commands
{
	public class DeleteCustomerCommand : Command
	{
		internal CustomerDeletedEvent ToCustomerEvent()
		{
			return new CustomerDeletedEvent
			{
				Id = this.Id
			};
		}
	}
}
