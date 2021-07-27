using API.BuildingEndpoints;
using AutoMapper;
using Core.Entities;

namespace API
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<CreateBuildingCommand, Building>();
            CreateMap<UpdateBuildingCommand, Building>();

            CreateMap<Building, BuildingListResult>();
            CreateMap<Building, BuildingResult>();
            CreateMap<Building, CreateBuildingResult>();
            CreateMap<Building, UpdateBuildingResult>();
        }
    }
}
