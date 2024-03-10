using AutoMapper;
using Skinet.Core.Entities;
using Skinet.WebAPI.Dtos;

namespace Skinet.WebAPI.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>();
        }
    }
}
