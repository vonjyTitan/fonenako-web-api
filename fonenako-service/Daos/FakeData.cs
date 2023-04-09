using System;
using System.Collections.Generic;
using fonenako.DatabaseContexts;
using fonenako.Models;
using fonenako_service.Models;

namespace fonenako_service.Daos
{
    public static class FakeData
    {
        public static void InitFakeData(FonenakoDbContext dbContext)
        {
            var now = DateTime.Now;

            for (var i = 1; i <= 100; i++)
            {
                var city = new City
                {
                    CityId = i,
                    Name = $"City{i}",
                    Areas = new List<Area>
                    {
                        new()
                        {
                            AreaId = i,
                            Name = $"Area{i}",
                            LeaseOffers = new List<LeaseOffer>()
                            {
                                new LeaseOffer
                                {
                                    LeaseOfferID = i,
                                    Title = $"Offer number {i}",
                                    Rooms = i,
                                    MonthlyRent = 1000 + 100 * i,
                                    Surface = 10 * i,
                                    CreationDate = now.AddDays(i)
                                }
                            }
                        }
                    }
                };

                dbContext.Add(city);
            }

            dbContext.SaveChanges();
        }
    }
}
