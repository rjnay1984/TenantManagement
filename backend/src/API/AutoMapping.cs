using API.BuildingEndpoints;
using AutoMapper;
using Core.Entities;

namespace API
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Building, BuildingListResult>();
        }
    }
}
