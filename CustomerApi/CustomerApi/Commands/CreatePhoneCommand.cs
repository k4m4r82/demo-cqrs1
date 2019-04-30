using CustomerApi.Models;

namespace CustomerApi.Commands
{
	public class CreatePhoneCommand : Command
	{
		public PhoneType Type { get; set; }
		public int AreaCode { get; set; }
		public int Number { get; set; }
	}
}
