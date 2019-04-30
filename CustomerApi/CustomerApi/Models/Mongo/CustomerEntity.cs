using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace CustomerApi.Models.Mongo
{
	public class CustomerEntity
    {
		[BsonElement("Id")]
		public long Id { get; set; }
		[BsonElement("Email")]
		public string Email { get; set; }
		[BsonElement("Name")]
		public string Name { get; set; }
		[BsonElement("Age")]
		public int Age { get; set; }
		[BsonElement("Phones")]
		public List<PhoneEntity> Phones;
	}
}
