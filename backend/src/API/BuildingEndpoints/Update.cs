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
    public class Update : BaseAsyncEndpoint
        .WithRequest<UpdateBuildingCommand>
        .WithResponse<UpdateBuildingResult>
    {
        private readonly IAsyncRepository<Building> _repository;
        private readonly IMapper _mapper;

        public Update(IAsyncRepository<Building> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPut("api/buildings")]
        public override async Task<ActionResult<UpdateBuildingResult>> HandleAsync([FromBody]UpdateBuildingCommand request, CancellationToken cancellationToken = default)
        {
            var building = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (building == null) return NotFound();
            _mapper.Map(request, building);
            await _repository.UpdateAsync(building, cancellationToken);

            var result = _mapper.Map<UpdateBuildingResult>(building);
            return Ok(result);
        }
    }
}
