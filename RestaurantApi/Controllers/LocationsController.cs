using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantBusiness;
using RestaurantData;
using RestaurantDTOs;
using System.Security.Claims;

namespace RestaurantApi.Controllers
{
    [ApiController]
    [Route("api/Locations")]
    [Authorize]
    public class LocationsController : ControllerBase
    {
        [Authorize(Roles = "Customer")]
        [HttpPost(Name = "AddNewLocation")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsLocationDTO> AddNewLocation(clsLocationDTO LocationDTO)
        {
            var ID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (LocationDTO.UserID != int.Parse(ID))
            {
                return Forbid();
            }
            try
            {
                clsLocation Location = new clsLocation();

                Location.LocationName = LocationDTO.LocationName;
                Location.LocationAddress = LocationDTO.LocationAddress;
                Location.Latitude = LocationDTO.Latitude;
                Location.Longitude = LocationDTO.Longitude;
                Location.UserID = LocationDTO.UserID;

                if (Location.Save())
                {
                    LocationDTO.LocationID = Location.LocationID;
                    return CreatedAtRoute("GetLocationByID", new { LocationID = LocationDTO.LocationID }, LocationDTO);
                }
                else
                {
                    return BadRequest("The Location Could Not Be Added");
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [Authorize(Roles = "Customer")]
        [HttpPut(Name = "UpdateLocation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsLocationDTO> UpdateLocation(clsLocationDTO LocationDTO)
        {
            var ID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (LocationDTO.UserID != int.Parse(ID))
            {
                return Forbid();
            }
            try
            {
                clsLocation Location = clsLocation.Find(LocationDTO.LocationID);

                if (Location == null)
                {
                    return NotFound("Could Not Find The Location");
                }

                Location.LocationName = LocationDTO.LocationName;
                Location.LocationAddress = LocationDTO.LocationAddress;
                Location.Latitude = LocationDTO.Latitude;
                Location.Longitude = LocationDTO.Longitude;

                if (Location.Save())
                {
                    return Ok(Location.LocationDTO);
                }
                else
                {
                    return BadRequest("The Location Could Not Be Updated");
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [Authorize(Roles = "Customer")]
        [HttpDelete("{id}", Name = "DeleteLocation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult DeleteLocation(int id)
        {
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (id != int.Parse(userID))
            {
                return Forbid();
            }
            try
            {
                clsLocation Location = clsLocation.Find(id);

                if (Location == null)
                {
                    return NotFound("Could Not Find The Location");
                }

                if (clsLocation.DeleteLocation(id))
                {
                    return Ok("Location Deleted Successfully");
                }
                else
                {
                    return BadRequest("Location Could Not Be Deleted");
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpGet("{id}", Name = "GetLocationByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsLocationDTO> GetLocationByID(int id)
        {
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (id != int.Parse(userID))
            {
                return Forbid();
            }
            try
            {
                clsLocation Location = clsLocation.Find(id);

                if (Location == null)
                {
                    return NotFound("Could Not Find The Location");
                }
                else
                {
                    return Ok(Location.LocationDTO);
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [Authorize(Roles = "Customer")]
        [HttpGet("{userID}/locations", Name = "GetAllUserLocations")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<IEnumerable<clsLocationDTO>> GetAllUserLocations(int id)
        {
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (id != int.Parse(userID))
            {
                return Forbid();
            }
            try
            {
                List<clsLocationDTO> AllUserLocationsList = clsLocation.GetAllLocations(id);

                if (AllUserLocationsList.Count == 0)
                {
                    return NotFound("There Is No Locations To Show");
                }
                return Ok(AllUserLocationsList);
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
    }
}
