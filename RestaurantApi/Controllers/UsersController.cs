using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantBusiness;
using RestaurantData;
using RestaurantDTOs;

namespace RestaurantApi.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpPost(Name = "AddNewUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsUserDTO> AddNewUser(clsUserDTO UserDTO)
        {
            try
            {
                clsUser User = new clsUser();

                User.UserName = UserDTO.UserName;
                User.DeviceToken = UserDTO.DeviceToken;
                User.Email = UserDTO.Email;
                User.PasswordHash = UserDTO.PasswordHash;
                User.Phone = UserDTO.Phone;

                if (User.Save())
                {
                    UserDTO.UserID = User.UserID;
                    return CreatedAtRoute("GetUserByID", new { UserID = UserDTO.UserID }, UserDTO);
                }
                else
                {
                    return BadRequest("Failed Creating An Account");
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpPut(Name = "UpdateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsUserDTO> UpdateUser(clsUserDTO UserDTO)
        {
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
        public ActionResult<clsUserDTO> GetUserByID(int id)
        {
            try
            {
                clsUser User = clsUser.Find(id);

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
        [HttpGet("username/{userName}", Name = "GetUserByUserName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsUserDTO> GetUserByUserName(string userName)
        {
            try
            {
                clsUser User = clsUser.Find(userName);

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
        [HttpGet(Name = "GetUserByUserNameAndPassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsUserDTO> GetUserByUserNameAndPassword(clsLoginRequestDTO loginRequestDTO)
        {
            try
            {
                clsUser User = clsUser.Find(loginRequestDTO.UserName, loginRequestDTO.PasswordHash);

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
        [HttpGet("{userID}/coins/transactions", Name = "GetUserCoinsTransactionsHistory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<IEnumerable<clsCoinTransactionDTO>> GetUserCoinsTransactionsHistory(int userID)
        {
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
        [HttpGet("{userID}/orders", Name = "GetAllUserOrders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<IEnumerable<clsOrderDTO>> GetAllUserOrders(int userID)
        {
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
        [HttpGet("{userID}/savings/coins", Name = "GetDriverDeliveryOrders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsCoinSavingsSummaryDTO> GetUserCoinSavingsSummary(int userID)
        {
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
