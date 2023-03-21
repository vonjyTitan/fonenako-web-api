using AutoMapper;
using fonenako.Models;
using fonenako_service.Dtos;

namespace fonenako_service
{
    public class LeaseOfferProfile : Profile
    {
        public LeaseOfferProfile()
        {
            CreateMap<LeaseOffer, LeaseOfferDto>();
        }
    }
}
