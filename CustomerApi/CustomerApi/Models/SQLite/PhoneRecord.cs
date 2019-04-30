namespace CustomerApi.Models.SQLite
{
	public class PhoneRecord
    {
		public long Id { get; set; }
		public long CustomerId { get; set; }
		public CustomerRecord Customer { get; set; }
		public PhoneType Type { get; set; }
		public int AreaCode { get; set; }
		public int Number { get; set; }
	}
}
