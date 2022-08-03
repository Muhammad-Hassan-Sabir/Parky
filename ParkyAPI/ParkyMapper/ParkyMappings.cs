using AutoMapper;
using ParkyAPI.Models;
using ParkyAPI.Models.Dtos;

namespace ParkyAPI.ParkyMapper
{
    public class ParkyMappings:Profile
    {

        public ParkyMappings()
        {
            //National Park Mapping
            CreateMap<NationalPark, NationalParkDto>().ReverseMap();


            //Trail Mapping
            CreateMap<Trail, TrailInsertsDto>().ReverseMap();
            CreateMap<Trail, TrailUpdateDto>().ReverseMap();
            CreateMap<Trail, TrailDto>().ReverseMap();
        }
    }
}
