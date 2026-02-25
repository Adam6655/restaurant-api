using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using RestaurantBusiness;
using RestaurantData;
using RestaurantDTOs;
using Microsoft.AspNetCore.Authorization;
namespace RestaurantApi.Controllers
{
    [ApiController]
    [Route("api/AddOns")]
    [Authorize]
    public class AddOnsController : ControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpPost(Name = "AddNewAddOn")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsAddOnDTO> AddNewAddOn(clsAddOnDTO AddOnDTO)
        {
            try
            {
                clsAddOn AddOn = new clsAddOn();

                AddOn.AddOnName = AddOnDTO.AddOnName;
                AddOn.Price = AddOnDTO.Price;

                if (AddOn.Save())
                {
                    AddOnDTO.AddOnID = AddOn.AddOnID;
                    return CreatedAtRoute("GetAddOnByID", new { AddOnID = AddOnDTO.AddOnID }, AddOnDTO);
                }
                else
                {
                    return BadRequest("The AddOn Could Not Be Added");
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }

        }
        [Authorize(Roles = "Admin")]
        [HttpPut(Name = "UpdateAddOn")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsAddOnDTO> UpdateAddOn(clsAddOnDTO AddOnDTO)
        {
            try
            {
                clsAddOn AddOn = clsAddOn.Find(AddOnDTO.AddOnID);

                if (AddOn == null)
                {
                    return NotFound("Could Not Find The AddOn");
                }

                AddOn.AddOnName = AddOnDTO.AddOnName;
                AddOn.Price = AddOnDTO.Price;

                if (AddOn.Save())
                {
                    return Ok(AddOn.AddOnDTO);
                }
                else
                {
                    return BadRequest($"The {AddOnDTO.AddOnName} Could Not Be Updated");
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{addOnID}", Name = "DeleteAddOn")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult DeleteAddOn(int addOnID)
        {
            try
            {
                clsAddOn AddOn = clsAddOn.Find(addOnID);

                if (AddOn == null)
                {
                    return NotFound("Could Not Find The AddOn");
                }

                if (clsAddOn.DeleteAddOn(addOnID))
                {
                    return Ok("AddOn Deleted Successfully");
                }
                else
                {
                    return BadRequest("AddOn Could Not Be Deleted");
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [AllowAnonymous]
        [HttpGet("{addOnID}", Name = "GetAddOnByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsAddOnDTO> GetAddOnByID(int addOnID)
        {
            try
            {
                clsAddOn AddOn = clsAddOn.Find(addOnID);

                if (AddOn == null)
                {
                    return NotFound("Could Not Find The AddOn");
                }
                else
                {
                    return Ok(AddOn.AddOnDTO);
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [AllowAnonymous]
        [HttpGet("all", Name = "GetAllAddOns")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<IEnumerable<clsAddOnDTO>> GetAllAddOns()
        {
            try
            {
                List<clsAddOnDTO> AddOnsList = clsAddOn.GetAllAddOns();

                if (AddOnsList.Count == 0)
                {
                    return NotFound("There Is No AddOns To Show");
                }
                return Ok(AddOnsList);
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [AllowAnonymous]
        [HttpGet("{productID}/addons", Name = "GetProductAddOns")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<IEnumerable<clsAddOnDTO>> GetProductAddOns(int productID)
        {
            try
            {
                List<clsAddOnDTO> ProductAddOnsList = clsAddOn.GetProductAddOns(productID);

                if (ProductAddOnsList.Count == 0)
                {
                    return NotFound("The Product Does Not Have Any AddOns");
                }
                return Ok(ProductAddOnsList);
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [AllowAnonymous]
        [HttpGet("{productID}/addons/available", Name = "GetAvailableAddOnsForProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<IEnumerable<clsAddOnDTO>> GetAvailableAddOnsForProduct(int productID)
        {
            try
            {
                List<clsAddOnDTO> AvailableAddOnsList = clsAddOn.GetAvailableAddOnsForProduct(productID);

                if (AvailableAddOnsList.Count == 0)
                {
                    return NotFound("The Product Does Not Have Any Available AddOns");
                }
                return Ok(AvailableAddOnsList);
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [AllowAnonymous]
        [HttpGet("addOnsByIDs", Name = "GetAddOnsByIDs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<IEnumerable<clsAddOnDTO>> GetAddOnsByIDs([FromQuery] List<int> AddOnIDs)
        {
            try
            {
                List<clsAddOnDTO> AddOnsList = clsAddOn.GetAddOnsByIDs(AddOnIDs);

                if (AddOnsList.Count == 0)
                {
                    return NotFound("There Is No AddOns To Show");
                }
                return Ok(AddOnsList);
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [AllowAnonymous]
        [HttpGet("{addOnID}/products/selections", Name = "GetAddOnProductSelections")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<IEnumerable<clsAddOnProductSelectionDTO>> GetAddOnProductSelections(int addOnID)
        {
            try
            {
                clsAddOn AddOn = clsAddOn.Find(addOnID);

                if (AddOn == null)
                {
                    return NotFound("Could Not Find The AddOn");
                }

                List<clsAddOnProductSelectionDTO> AddOnsList = AddOn.GetAddOnProductSelections();

                if (AddOnsList.Count == 0)
                {
                    return NotFound("There Is No Products To Show");
                }
                return Ok(AddOnsList);
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{addOnID}/products/selections", Name = "SaveAddOnProductSelections")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult SaveAddOnProductSelections(int addOnID, List<int> ProductIDs)
        {
            try
            {
                clsAddOn AddOn = clsAddOn.Find(addOnID);

                if (AddOn == null)
                {
                    return NotFound("Could Not Find The AddOn");
                }

                if (AddOn.SaveAddOnProductSelections(ProductIDs))
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("The Products For The AddOn Could Not Be Saved");
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
    }
}
