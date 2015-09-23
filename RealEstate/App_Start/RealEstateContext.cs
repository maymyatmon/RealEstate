using RealEstate.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;
using RealEstate.Models;

namespace RealEstate.App_Start
{
    public class RealEstateContext
    {
        public IMongoDatabase Database;

        public RealEstateContext()
        {
            var client = new MongoClient(Settings.Default.RealEstateConnectionString);
            Database = client.GetDatabase(Settings.Default.RealEstateDatabaseName);
        }

        public IMongoCollection<Rental> Rentals 
        {
            get
            {
                return Database.GetCollection<Rental>("rentals");
            }
        }
    }
}