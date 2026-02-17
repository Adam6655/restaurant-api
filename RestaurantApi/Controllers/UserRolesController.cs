using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantBusiness;
using RestaurantData;
using RestaurantDTOs;

namespace RestaurantApi.Controllers
{
    [Route("api/UserRoles")]
    [ApiController]
    public class UserRolesController : ControllerBase
    {
        [HttpPost(Name = "AddNewUserRole")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsUserRoleDTO> AddNewUserRole(clsUserRoleDTO UserRoleDTO)
        {
            try
            {
                clsUserRole UserRole = new clsUserRole();

                UserRole.UserRole = UserRoleDTO.UserRole;
                UserRole.UserID = UserRoleDTO.UserID;

                if (UserRole.Save())
                {
                    UserRoleDTO.UserRoleID = UserRole.UserRoleID;
                    return CreatedAtRoute("GetUserRoleByID", new { UserRoleID = UserRoleDTO.UserID }, UserRoleDTO);
                }
                else
                {
                    return BadRequest("The Role For The User Could Not Be Added");
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpPut(Name = "UpdateUserRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsUserRoleDTO> UpdateUserRole(clsUserRoleDTO UserRoleDTO)
        {
            try
            {
                clsUserRole UserRole = clsUserRole.Find(UserRoleDTO.UserRoleID);

                if (UserRole == null)
                {
                    return NotFound("Could Not Find The User Role");
                }

                UserRole.UserRole = UserRoleDTO.UserRole;
                UserRole.UserID = UserRoleDTO.UserID;

                if (UserRole.Save())
                {
                    return Ok(UserRole.UserRoleDTO);
                }
                else
                {
                    return BadRequest("The Role Of The User Could Not Be Updated");
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpDelete("{id}", Name = "DeleteUserRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult DeleteUserRole(int id)
        {
            try
            {
                clsUserRole UserRole = clsUserRole.Find(id);

                if (UserRole == null)
                {
                    return NotFound("Could Not Find The User Role");
                }

                if (UserRole.UserRole == (byte)clsUserRole.enUserRole.Admin)
                {
                    return BadRequest("Admin Could Not Be Deleted");
                }

                if (UserRole.UserRole == (byte)clsUserRole.enUserRole.Driver)
                {
                    List<clsUserRoleWithNameDTO> NumOfDrivers = clsUserRole.GetAllDrivers();
                    List<clsOrderDTO> DriverOrders = clsUser.GetDriverDeliveryOrders(UserRole.UserID);
                    if (NumOfDrivers.Count == 1 && DriverOrders.Count != 0)
                    {
                        return BadRequest("Cant delete the only driver while him having Orders to Deliver");
                    }

                    if (DriverOrders.Count != 0 && NumOfDrivers.Count > 1)
                    {
                        int DistributedOrders = clsUserRole.DistributeDriverOrders(UserRole.UserID);
                        if (DistributedOrders != DriverOrders.Count)
                        {
                            return BadRequest($"Only {DistributedOrders} Orders Where Distrubuted Out Of {DriverOrders.Count}");
                        }
                    }
                }

                if (clsUserRole.DeleteUserRole(id))
                {
                    return Ok("Deleted The User Role Successfully");
                }
                else
                {
                    return BadRequest("The User Could Not Be Deleted");
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpGet("{id}", Name = "GetUserRoleByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsUserRoleDTO> GetUserRoleByID(int id)
        {
            try
            {
                clsUserRole UserRole = clsUserRole.Find(id);

                if (UserRole == null)
                {
                    return NotFound("Could Not Find The User Role");
                }
                else
                {
                    return Ok(UserRole.UserRoleDTO);
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpGet("all", Name = "GetAllUserRoles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<IEnumerable<clsUserRoleWithNameDTO>> GetAllUserRoles()
        {
            try
            {
                List<clsUserRoleWithNameDTO> UserRolesList = clsUserRole.GetAllUserRoles();

                return Ok(UserRolesList);
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpGet("alldrivers", Name = "GetAllDrivers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<IEnumerable<clsUserRoleWithNameDTO>> GetAllDrivers()
        {
            try
            {
                List<clsUserRoleWithNameDTO> DriversList = clsUserRole.GetAllDrivers();

                if (DriversList.Count == 0)
                {
                    return NotFound("There Is No Shifts To Show");
                }
                return Ok(DriversList);
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpGet("{userID}/coin-savings-summary", Name = "CoinSavingsSummary")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<IEnumerable<clsCoinSavingsSummaryDTO>> CoinSavingsSummary(int userID)
        {
            try
            {
                clsUser User = clsUser.Find(userID);

                if (User == null)
                {
                    return NotFound("Could Not Find The User");
                }

                return Ok(User.CoinSavingsSummaryDTO);
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
    }
}
