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
    public class Get : BaseAsyncEndpoint
        .WithRequest<int>
        .WithResponse<BuildingResult>
    {
        private readonly IAsyncRepository<Building> _repository;
        private readonly IMapper _mapper;

        public Get(IAsyncRepository<Building> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("api/buildings/{id}")]
        public override async Task<ActionResult<BuildingResult>> HandleAsync(int id, CancellationToken cancellationToken = default)
        {
            var building = await _repository.GetByIdAsync(id, cancellationToken);

            if (building == null) return NotFound();

            var result = _mapper.Map<BuildingResult>(building);

            return Ok(result);
        }
    }
}
