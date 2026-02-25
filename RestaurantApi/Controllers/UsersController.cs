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
    [Route("api/Users")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        [HttpPut(Name = "UpdateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsUserDTO> UpdateUser(clsUserDTO UserDTO)
        {
            var ID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userRole = User.FindFirstValue(ClaimTypes.Role);

            int authenticatedUserID = int.Parse(ID);

            bool IsAdmin = userRole == "Admin";

            if (!IsAdmin && authenticatedUserID != UserDTO.UserID)
            {
                return Forbid();
            }
            try
            {
                clsUser User = clsUser.Find(UserDTO.UserID);

                if (User == null)
                {
                    return NotFound("Could Not Find The User");
                }

                User.UserName = UserDTO.UserName;
                User.DeviceToken = UserDTO.DeviceToken;
                User.Email = UserDTO.Email;
                User.PasswordHash = UserDTO.PasswordHash;
                User.Phone = UserDTO.Phone;
                User.RefreshTokenHash = UserDTO.RefreshTokenHash;

                if (User.Save())
                {
                    return Ok(User.UserDTO);
                }
                else
                {
                    return BadRequest($"The User Info Could Not Be Updated");
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpGet("id/{id}", Name = "GetUserByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsUserDTO> GetUserByID(int userId)
        {
            var ID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userRole = User.FindFirstValue(ClaimTypes.Role);

            int authenticatedUserID = int.Parse(ID);

            bool IsAdmin = userRole == "Admin";

            if (!IsAdmin && authenticatedUserID != userId)
            {
                return Forbid();
            }
            try
            {
                clsUser User = clsUser.Find(userId);

                if (User == null)
                {
                    return NotFound("Could Not Find The User");
                }
                else
                {
                    return Ok(User.UserDTO);
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [Authorize(Roles = "Customer")]
        [HttpGet("{userID}/coins/transactions", Name = "GetUserCoinsTransactionsHistory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<IEnumerable<clsCoinTransactionDTO>> GetUserCoinsTransactionsHistory(int userID)
        {
            var ID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userID != int.Parse(ID))
            {
                return Forbid();
            }
            try
            {
                List<clsCoinTransactionDTO> UserCoinsTransactionsHistoryList = clsUser.GetUserCoinsTransactionsHistory(userID);

                if (UserCoinsTransactionsHistoryList.Count == 0)
                {
                    return NotFound("There Is No Transactions To Show");
                }
                return Ok(UserCoinsTransactionsHistoryList);
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [Authorize(Roles = "Customer")]
        [HttpGet("{userID}/orders", Name = "GetAllUserOrders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<IEnumerable<clsOrderDTO>> GetAllUserOrders(int userID)
        {
            var ID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userID != int.Parse(ID))
            {
                return Forbid();
            }
            try
            {
                List<clsOrderDTO> UserOrdersList = clsUser.GetAllUserOrders(userID);

                if (UserOrdersList.Count == 0)
                {
                    return NotFound("There Is No Orders To Show");
                }
                return Ok(UserOrdersList);
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [Authorize(Roles = "Admin,Driver")]
        [HttpGet("{driverID}/delivery-orders", Name = "GetDriverDeliveryOrders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<IEnumerable<clsOrderDTO>> GetDriverDeliveryOrders(int driverID)
        {
            try
            {
                clsUser User = clsUser.Find(driverID);

                if (User == null)
                {
                    return NotFound("Could Not Find The User");
                }

                if (User.GetUserRole() != (int)clsUserRole.enUserRole.Driver)
                {
                    return BadRequest($"Only Drivers Can Access This Page");
                }

                List<clsOrderDTO> DriverOrdersList = clsUser.GetDriverDeliveryOrders(driverID);

                if (DriverOrdersList.Count == 0)
                {
                    return NotFound("There Is No Orders To Show");
                }
                return Ok(DriverOrdersList);
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [Authorize(Roles = "Customer")]
        [HttpGet("{userID}/savings/coins", Name = "GetUserCoinSavingsSummary")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsCoinSavingsSummaryDTO> GetUserCoinSavingsSummary(int userID)
        {
            var ID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userID != int.Parse(ID))
            {
                return Forbid();
            }
            try
            {
                clsUser User = clsUser.Find(userID);

                if (User == null)
                {
                    return NotFound("Could Not Find The User");
                }

                return Ok(User.GetUserCoinSavingsSummary());
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
    }
}
