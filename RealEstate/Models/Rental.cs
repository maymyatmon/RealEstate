using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.Models
{
    public class Rental
    {
        //Need default constructor so that Mongo can deserialize Rental model
        public Rental()
        {

        }
        public Rental(PostRental postRental)
        {
            Description = postRental.Description;
            NumberOfRooms = postRental.NumberOfRooms;
            Price = postRental.Price;
            Address = (postRental.Address ?? string.Empty).Split('\n').ToList();
        }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Description { get; set; }
        public int NumberOfRooms { get; set; }
        public List<string> Address = new List<string>();

        //.NET decimal type is mapped to string in Bson. So, need to define attribute to represent as Bson Double
        [BsonRepresentation(BsonType.Double)]
        public decimal Price { get; set; }
    }
}