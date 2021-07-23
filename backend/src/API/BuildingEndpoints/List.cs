using Ardalis.ApiEndpoints;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.BuildingEndpoints
{
    [Authorize(Roles = "Admin, Landlord")]
    public class List : BaseAsyncEndpoint
        .WithRequest<BuildingListRequest>
        .WithResponse<IList<BuildingListResult>>
    {
        private readonly IAsyncRepository<Building> _repository;
        private readonly IMapper _mapper;

        public List(IAsyncRepository<Building> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("api/buildings")]
        public override async Task<ActionResult<IList<BuildingListResult>>> HandleAsync([FromQuery] BuildingListRequest request, CancellationToken cancellationToken = default)
        {
            if (request.PerPage == 0) request.PerPage = 10;
            if (request.Page == 0) request.Page = 1;

            var result = (await _repository.ListAllAsync(request.PerPage, request.Page, cancellationToken))
                    .Select(i => _mapper.Map<BuildingListResult>(i));

            return Ok(result);
        }
    }
}
