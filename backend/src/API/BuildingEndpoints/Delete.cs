using Ardalis.ApiEndpoints;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace API.BuildingEndpoints
{
    [Authorize(Roles = "Admin, Landlord")]
    public class Delete : BaseAsyncEndpoint
        .WithRequest<DeleteBuildingRequest>
        .WithoutResponse
    {
        private readonly IAsyncRepository<Building> _repository;

        public Delete(IAsyncRepository<Building> repository)
        {
            _repository = repository;
        }

        [HttpDelete("api/buildings/{id}")]
        public override async Task<ActionResult> HandleAsync([FromRoute]  DeleteBuildingRequest request, CancellationToken cancellationToken = default)
        {
            var building = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (building == null) return NotFound(request.Id);

            await _repository.DeleteAsync(building, cancellationToken);

            return NoContent();
        }
    }
}
