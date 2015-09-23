using MongoDB.Bson;
using RealEstate.App_Start;
using RealEstate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RealEstate.Controllers
{
    public class RentalsController : Controller
    {
        public readonly RealEstateContext Context = new RealEstateContext();

        public async Task<ActionResult> Index()
        {
            List<Rental> rentals = new List<Rental>();
            var filter = new BsonDocument();
            //var task = Context.Rentals.FindAsync<Rental>(filter);
            //task.Wait();
            using (var cursor = await Context.Rentals.FindAsync<Rental>(filter))
            {
                
                while (await cursor.MoveNextAsync())
                {
                    var batch = cursor.Current;
                    foreach (var document in batch)
                    {
                        rentals.Add(document);
                    }
                }
            }
            return View(rentals);
        }

        public ActionResult Post()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Post(PostRental postRental)
        {
            var rental = new Rental(postRental);
            Context.Rentals.InsertOneAsync(rental);
            return RedirectToAction("index");
        }
    }
}