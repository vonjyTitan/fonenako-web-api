using System;
using System.Collections.Generic;
using System.Linq;
using fonenako.DatabaseContexts;
using fonenako.Models;
using fonenako_service.Models;

namespace fonenako_service.Daos
{
    public static class FakeData
    {
        public static void InitFakeData(FonenakoDbContext dbContext)
        {

            if (dbContext.Localisations.Any()) return;

            var now = DateTime.Now;

            for (var i = 1; i <= 100; i++)
            {
                var city = new Localisation
                {
                    LocalisationId = i,
                    Name = $"City{i}",
                    Type = LocalisationType.CIT,
                };

                dbContext.Add(city);
                dbContext.SaveChanges();
                var area = new Localisation
                {
                    LocalisationId = i + 100,
                    Name = $"Area{i}",
                    Type = LocalisationType.ARE,
                    Hierarchy = city
                };
                dbContext.Add(area);
                dbContext.SaveChanges();
                var leaseOffer = new LeaseOffer
                {
                    LeaseOfferID = i,
                    Title = $"Offer numero {i}",
                    Rooms = i,
                    MonthlyRent = 1000 + 100 * i,
                    Surface = 10 * i,
                    CreationDate = now.AddDays(i),
                    Description = $"Description de l'offre numero {i}",
                    Localisation = area
                };

                dbContext.Add(leaseOffer);
                dbContext.SaveChanges();
            }
        }
    }
}
