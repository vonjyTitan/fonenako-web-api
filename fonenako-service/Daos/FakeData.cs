using System;
using fonenako.Models;

namespace fonenako_service.Daos
{
    public static class FakeData
    {
        public static LeaseOffer[] FakeDatas()
        {
            const int size = 100;
            var response = new LeaseOffer[size];
            var rand = new Random();
            for (int i = 1; i <= size; i++)
            {
                var surface = rand.Next(18, 68);
                response[i - 1] = new LeaseOffer()
                {
                    LeaseOfferID = i,
                    Title = $"lease Offer number {i}",
                    Rooms = rand.Next(1, 3),
                    Surface = rand.Next(18, 68),
                    MonthlyRent = 500 + surface * 10
                };
            }

            return response;
        }
    }
}
