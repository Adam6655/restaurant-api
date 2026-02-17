using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantBusiness;
using RestaurantData;
using RestaurantDTOs;
using System.Collections.Generic;
using static RestaurantBusiness.clsUserRole;

namespace RestaurantApi.Controllers
{
    [Route("api/Orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        [HttpPost(Name = "AddNewOrder")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsOrderDTO> AddNewOrder(clsProcessOrderDTO ProcessOrderDTO)
        {
            try
            {
                int? UserRole = clsUser.GetUserRole(ProcessOrderDTO.OrderDTO.UserID);

                if (UserRole != null)
                {
                    return BadRequest($"Employees Cant Order");
                }

                List<clsCheckoutCartItemsDTO> UpdatedCheckoutCartItemsDTO = clsOrder.SyncCartItemsWithDatabase(ProcessOrderDTO.CheckoutCartItemsDTO);
                if (!clsOrder.IsCartUpToDate(ProcessOrderDTO.CheckoutCartItemsDTO, UpdatedCheckoutCartItemsDTO))
                {
                    return Ok(UpdatedCheckoutCartItemsDTO);
                }
                
                decimal OrderTotalAmount = clsOrder.CalculateCartTotalAmount(ProcessOrderDTO.CheckoutCartItemsDTO);
                
                if ((ProcessOrderDTO.PaymentInfoDTO.PaymentMethod.Count == 1) && (ProcessOrderDTO.PaymentInfoDTO.PaymentMethod[0] == (byte)clsOrder.enPaymentMethod.Coins))
                {
                    int CoinsValue = clsUser.GetCoinsValue((int)ProcessOrderDTO.PaymentInfoDTO.Amount[0]);
                    if (OrderTotalAmount != CoinsValue)
                    {
                        return BadRequest("The Coins Arent Enough. Add Cash Or Debit Option With Coin");
                    }
                }

                clsOrder Order = new clsOrder();

                Order.ServiceType = ProcessOrderDTO.OrderDTO.ServiceType;
                Order.LocationID = ProcessOrderDTO.OrderDTO.LocationID;
                Order.UserID = ProcessOrderDTO.OrderDTO.UserID;
                Order.TotalAmount = OrderTotalAmount;

                if (Order.Save(ProcessOrderDTO.PaymentInfoDTO, ProcessOrderDTO.CheckoutCartItemsDTO))
                {
                    ProcessOrderDTO.OrderDTO.OrderID = Order.OrderID;
                    ProcessOrderDTO.OrderDTO.TotalAmount = Order.TotalAmount;
                    return CreatedAtRoute("GetOrderByID", new { OrderID = ProcessOrderDTO.OrderDTO.OrderID }, ProcessOrderDTO.OrderDTO);
                }
                else
                {
                    return BadRequest("The Order Could Not Be Placed");
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpPut("{orderID}/driver/{newDriverID}", Name = "UpdateOrderDriver")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsOrderDTO> UpdateOrderDriver(int orderID, int newDriverID)
        {
            try
            {
                clsOrder Order = clsOrder.Find(orderID);
                if (Order.UpdateOrderDriver(newDriverID))
                {
                    return Ok(Order.OrderDTO);
                }
                else
                {
                    return BadRequest($"The Order Could Not Be Updated");
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpPut("{orderID}/statuses/user/{userID}", Name = "UpdateOrderStatuses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsOrderDTO> UpdateOrderStatuses(int orderID, int userID)
        {
            try
            {
                clsOrder Order = clsOrder.Find(orderID);

                int? UserRole = clsUser.GetUserRole(userID);

                if (UserRole == null)
                {
                    return BadRequest($"You Have No Authorization To Update The Status Of The Order");
                }

                byte? OrderCurrentStatus = Order.GetOrderCurrentStatus();

                if (UserRole == (int)clsUserRole.enUserRole.Driver)
                {
                    if ((Order.ServiceType == (byte)clsOrder.enServiceType.Delivery) && !clsOrder.IsOrderAssignedToDriver(OrderID, UserID))
                    {
                        return BadRequest($"You are not assigned to the order in order to update it");
                    }
                    if (Order.ServiceType != (byte)clsOrder.enServiceType.Delivery)
                    {
                        return BadRequest($"You cant update the order since it isnt delivery");
                    }
                    if (OrderCurrentStatus < (byte)clsOrder.enOrderStatus.ReadyToDeliver)
                    {
                        return BadRequest($"it is the staff duty to update it");
                    }
                }

                if (UserRole != (int)clsUserRole.enUserRole.Driver)
                {
                    if (Order.ServiceType == (byte)clsOrder.enServiceType.Delivery && OrderCurrentStatus >= (byte)clsOrder.enOrderStatus.ReadyToDeliver)
                    {
                        return BadRequest($"it is the driver duty to update it");
                    }
                }

                if (Order.UpdateOrderStatus())
                {
                    return Ok(Order.OrderDTO);
                }
                else
                {
                    return BadRequest($"The Status Of the Order Could Not Be Updated");
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpGet("{OrderID}", Name = "GetOrderByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsOrderDTO> GetOrderByID(int OrderID)
        {
            try
            {
                clsOrder Order = clsOrder.Find(OrderID);

                if (Order == null)
                {
                    return NotFound("Could Not Find The Order");
                }
                else
                {
                    return Ok(Order.OrderDTO);
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }

        }
        [HttpGet("{orderID}/status", Name = "GetOrderStatuses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<IEnumerable<clsOrderStatusDTO>> GetOrderStatuses(int orderID)
        {
            try
            {
                List<clsOrderStatusDTO> OrderStatusesList = clsOrder.GetOrderStatuses(orderID);

                if (OrderStatusesList.Count == 0)
                {
                    return NotFound("There Is No Orders To Show");
                }
                return Ok(OrderStatusesList);
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpGet("all", Name = "GetAllOrders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<IEnumerable<clsOrderDTO>> GetAllOrders()
        {
            try
            {
                List<clsOrderDTO> OrdersList = clsOrder.GetAllOrders();

                if (OrdersList.Count == 0)
                {
                    return NotFound("There Is No Orders To Show");
                }
                return Ok(OrdersList);
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpGet("status/{orderStatus}", Name = "GetAllOrdersByOrderStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<IEnumerable<clsOrderStatus>> GetAllOrdersByOrderStatus(int orderStatus)
        {
            try
            {
                List<clsOrderDTO> OrdersList = clsOrder.GetAllOrders(orderStatus);

                if (OrdersList.Count == 0)
                {
                    return NotFound("There Is No Orders To Show");
                }
                return Ok(OrdersList);
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpGet("{orderID}/cart-items", Name = "GetCartItemsByOrderID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<IEnumerable<clsCartDTO>> GetCartItemsByOrderID(int orderID)
        {
            try
            {
                List<clsCartDTO> CartsList = clsOrder.GetCartItemsByOrderID(orderID);

                if (CartsList.Count == 0)
                {
                    return NotFound("The Order Does Not Have Any Items");
                }
                return Ok(CartsList);
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpPut("users/{userID}/orders/{orderID}/complete", Name = "MarkOrderAsCompleted")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<IEnumerable<clsAddOnDTO>> MarkOrderAsCompleted(int orderID, int userID)
        {
            try
            {
                int? UserRole = clsUser.GetUserRole(userID);

                if (UserRole == null)
                {
                    return BadRequest($"You Have No Authorization To Update The Status Of The Order");
                }

                clsOrder Order = clsOrder.Find(orderID);
                byte? OrderCurrentStatus = Order.GetOrderCurrentStatus();

                if (OrderCurrentStatus != (byte)clsOrder.enOrderStatus.ReadyToPick || OrderCurrentStatus != (byte)clsOrder.enOrderStatus.Arrived)
                {
                    return BadRequest($"The Order Cant Be Marked As Completed At This Current Status");
                }

                if (UserRole == (int)clsUserRole.enUserRole.Driver && OrderCurrentStatus != (byte)clsOrder.enOrderStatus.Arrived)
                {
                    return BadRequest($"The Driver Cant Update The Status Of The Order");
                }
                if (UserRole != (int)clsUserRole.enUserRole.Driver && OrderCurrentStatus == (byte)clsOrder.enOrderStatus.Arrived)
                {
                    return BadRequest($"Only The Driver can Mark The Order As Completed");
                }

                if (Order.MarkOrderAsCompleted())
                {
                    return Ok(Order.OrderDTO);
                }
                else
                {
                    return BadRequest("The Order Could Not Be Marked As Completed");
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpGet("sync-cart", Name = "SyncCartItemsWithDatabase")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<IEnumerable<clsCheckoutCartItemsDTO>> SyncCartItemsWithDatabase(List<clsCheckoutCartItemsDTO> CheckoutCartItems)
        {
            try
            {
                List<clsCheckoutCartItemsDTO> UpdatedCheckoutCartItemsDTO = clsOrder.SyncCartItemsWithDatabase(CheckoutCartItems);

                if (UpdatedCheckoutCartItemsDTO.Count == 0)
                {
                    return NotFound("The Updated Checkout Cart Does Not Have Any Items");
                }
                return Ok(UpdatedCheckoutCartItemsDTO);
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
    }
}
