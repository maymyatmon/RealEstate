using MongoDB.Bson;
using MongoDB.Driver;
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

        public async Task<ActionResult> AdjustPrice(string id)
        {
            var rental = await GetRental(id);
            return View(rental);
        }

        [HttpPost]
        public async Task<ActionResult> AdjustPrice(string id, AdjustPrice adjustPrice)
        {
            var rental = await GetRental(id);
            /*
            rental.AdjustPrice(adjustPrice);

            await Context.Rentals.ReplaceOneAsync<Rental>(r => r.Id == id, rental);
            */

            //****************Update document partially ***********************************/
            var adjustment = new PriceAdjustment(adjustPrice, rental.Price);
            
            var update = Builders<Rental>.Update.Set(r => r.Price, adjustPrice.NewPrice)
                                                .Push(r => r.Adjustments, adjustment);
            await Context.Rentals.UpdateOneAsync<Rental>(r => r.Id == id, update);
            /******************************************************************************/

            return RedirectToAction("index");
        }

        private async Task<Rental> GetRental(string id)
        {
            var rental = await Context.Rentals.Find(r => r.Id == id).FirstOrDefaultAsync();
            return rental;
        }
    }
}