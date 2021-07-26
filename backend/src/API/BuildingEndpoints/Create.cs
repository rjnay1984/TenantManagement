using Ardalis.ApiEndpoints;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace API.BuildingEndpoints
{
    [Authorize(Roles = "Admin, Landlord")]
    public class Create : BaseAsyncEndpoint
        .WithRequest<CreateBuildingCommand>
        .WithResponse<CreateBuildingResult>
    {
        private readonly IAsyncRepository<Building> _repository;
        private readonly IMapper _mapper;

        public Create(IAsyncRepository<Building> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost("api/buildings")]
        public override async Task<ActionResult<CreateBuildingResult>> HandleAsync([FromBody] CreateBuildingCommand request, CancellationToken cancellationToken = default)
        {
            var building = new Building();
            _mapper.Map(request, building);
            await _repository.AddAsync(building, cancellationToken);

            var result = _mapper.Map<CreateBuildingResult>(building);

            return Ok(result);
        }
    }
}
