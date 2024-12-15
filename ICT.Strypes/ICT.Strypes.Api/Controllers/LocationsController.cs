using ICT.Strypes.Business.Interfaces;
using ICT.Strypes.Business.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ICT.Strypes.Api.Controllers
{
    [Route("api/locations")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationsController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LocationModel>> PostLocationAsync([BindRequired, FromBody] LocationRequestModel request)
        {
            var location = await _locationService.PostLocationAsync(request).ConfigureAwait(false);

            return Created(location.LocationId, location);
        }

        [HttpGet, Route("{locationId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LocationModel>> GetLocationAsync([BindRequired, FromRoute] string locationId)
        {
            return Ok(await _locationService.GetLocationByIdAsync(locationId).ConfigureAwait(false));
        }

        [HttpPatch, Route("{locationId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LocationModel>> PatchLocationAsync([BindRequired, FromRoute] string locationId,
            [BindRequired, FromBody] PatchLocationRequestModel request)
        {
            return Ok(await _locationService.PatchLocationAsync(locationId, request).ConfigureAwait(false));
        }

        [HttpPut, Route("{locationId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LocationModel>> UpsertChargePointsAsync([BindRequired, FromRoute] string locationId,
            [BindRequired, FromBody] ChargePointRequestModel request)
        {
            return Ok(await _locationService.UpsertChargePointsAsync(locationId, request).ConfigureAwait(false));
        }
    }
}
