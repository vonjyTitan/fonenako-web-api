using System;
using System.Linq;
using AutoMapper;
using fonenako.Models;
using fonenako_service.Dtos;

namespace fonenako_service
{
    public class LeaseOfferProfile : Profile
    {
        public LeaseOfferProfile(string photoUrlBase)
        {
            if(string.IsNullOrEmpty(photoUrlBase))
            {
                throw new ArgumentException($"Cannot be null or empty", nameof(photoUrlBase));
            }

            CreateMap<LeaseOffer, LeaseOfferDto>()
                .ForMember(dto => dto.PhotoUris, act => act.MapFrom(leaseOffer =>
                leaseOffer.ConcatenedPhotos.Split(";", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(s => $"{photoUrlBase}/{s}").ToArray()));
        }
    }
}
