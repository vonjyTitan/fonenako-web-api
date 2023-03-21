using System;
using fonenako.Models;

namespace fonenako_service.Daos
{
    public static class FakeData
    {
        public static LeaseOffer[] FakeDatas()
        {
            var response = new LeaseOffer[16];
            var rand = new Random();
            for (int i = 1; i <= 16; i++)
            {
                response[i - 1] = new LeaseOffer()
                {
                    LeaseOfferID = i,
                    Title = $"lease Offer number {i}",
                    Rooms = rand.Next(1, 3),
                    Surface = rand.Next(18, 68)
                };
            }

            return response;
        }
    }
}
