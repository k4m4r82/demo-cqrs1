using System.Collections.Generic;

namespace CustomerApi.Models.SQLite
{
	public class CustomerRecord
    {
		public long Id { get; set; }
		public string Email { get; set; }
		public string Name { get; set; }
		public int Age { get; set; }
		public List<PhoneRecord> Phones { get; set; }
	}
}
