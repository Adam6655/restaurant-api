using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantBusiness;
using RestaurantData;
using RestaurantDTOs;

namespace RestaurantApi.Controllers
{
    [ApiController]
    [Route("api/Categories")]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpPost(Name = "AddNewCategory")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsCategoryDTO> AddNewCategory(clsCategoryDTO CategoryDTO)
        {
            try
            {
                clsCategory Category = new clsCategory();

                Category.CategoryName = CategoryDTO.CategoryName;
                Category.CategoryImageUrl = CategoryDTO.CategoryImageUrl;

                if (Category.Save())
                {
                    CategoryDTO.CategoryID = Category.CategoryID;
                    return CreatedAtRoute("GetCategoryByID", new { CategoryID = CategoryDTO.CategoryID }, CategoryDTO);
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
        [HttpPut(Name = "UpdateCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsCategoryDTO> UpdateCategory(clsCategoryDTO CategoryDTO)
        {
            try
            {
                clsCategory Category = clsCategory.Find(CategoryDTO.CategoryID);

                if (Category == null)
                {
                    return NotFound("Could Not Find The Category");
                }

                Category.CategoryName = CategoryDTO.CategoryName;
                Category.CategoryImageUrl = CategoryDTO.CategoryImageUrl;

                if (Category.Save())
                {
                    return Ok(Category.CategoryDTO);
                }
                else
                {
                    return BadRequest($"The {CategoryDTO.CategoryName} Could Not Be Updated");
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}", Name = "DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult DeleteCategory(int id)
        {
            try
            {
                clsCategory Category = clsCategory.Find(id);

                if (Category == null)
                {
                    return NotFound("Could Not Find The Category");
                }

                if (clsCategory.DeleteCategory(id))
                {
                    return Ok("Cayegory Deleted Successfully");
                }
                else
                {
                    return BadRequest("Category Could Not Be Deleted");
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [AllowAnonymous]
        [HttpGet("{id}", Name = "GetCategoryByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsCategoryDTO> GetCategoryByID(int id)
        {
            try
            {
                clsCategory Category = clsCategory.Find(id);

                if (Category == null)
                {
                    return NotFound("Could Not Find The Category");
                }
                else
                {
                    return Ok(Category.CategoryDTO);
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [AllowAnonymous]
        [HttpGet("all", Name = "GetAllCategories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<IEnumerable<clsCategoryDTO>> GetAllCategories()
        {
            try
            {
                List<clsCategoryDTO> CategoriesList = clsCategory.GetAllCategories();

                if (CategoriesList.Count == 0)
                {
                    return NotFound("There Is No Categories To Show");
                }
                return Ok(CategoriesList);
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [AllowAnonymous]
        [HttpGet("{categoryID}/products", Name = "GetAllCategoryProducts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<IEnumerable<clsProductDTO>> GetAllCategoryProducts(int categoryID)
        {
            try
            {
                clsCategory Category = clsCategory.Find(categoryID);
                List<clsProductDTO> CategoryProductsList = Category.GetAllProducts();

                if (CategoryProductsList.Count == 0)
                {
                    return NotFound("The Category Have No Products To Show");
                }
                return Ok(CategoryProductsList);
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
    }
}
