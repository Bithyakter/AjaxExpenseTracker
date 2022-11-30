using Domain;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

namespace API.Controllers
{
   /// <summary>
   /// ExCategory controller.
   /// </summary>
   [Route(RouteConstants.BaseRoute)]
   [ApiController]
   public class CategoryController : ControllerBase
   {
      private readonly IUnitOfWork context;
      private readonly ILogger<CategoryController> logger;

      /// <summary>
      /// Default constructor.
      /// </summary>
      /// <param name="context">Instance of the UnitOfWork.</param>
      public CategoryController(IUnitOfWork context, ILogger<CategoryController> logger)
      {
         this.context = context;
         this.logger = logger;
      }

      /// <summary>
      /// URL: expenses-api/category
      /// </summary>
      /// <param name="category">Category object.</param>
      /// <returns>Http status code: CreatedAt.</returns>
      [HttpPost]
      [Route(RouteConstants.CreateCategory)]
      public async Task<IActionResult> CreateCategory(Category category)
      {
         try
         {

            if (await IsCategoryDuplicate(category) == true)
               return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

            category.DateModified = DateTime.Now;
            category.IsDeleted = false;
            category.IsSynced = false;

            context.CategoryRepository.Add(category);
            await context.SaveChangesAsync();

            return CreatedAtAction("ReadCategoryByKey", new { key = category.OID }, category);
         }
         catch (Exception ex)
         {
            logger.LogError(ex.Message, "Creating Expense Category");
            return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
         }
      }

      /// <summary>
      /// URL: expenses-api/categories
      /// </summary>
      /// <returns>Http status code: Ok.</returns>
      [HttpGet]
      [Route(RouteConstants.ReadCategories)]
      public async Task<IActionResult> ReadCategories()
      {
         try
         {
            var category = await context.CategoryRepository.GetCategories();

            return Ok(category);
         }
         catch (Exception ex)
         {
            logger.LogError(ex.Message, "ReadCategories");
            return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
         }
      }

      /// <summary>
      /// URL: expenses-api/category/key/{key}
      /// </summary>
      /// <param name="key">Primary key of the table category.</param>
      /// <returns>Http status code: Ok.</returns>
      [HttpGet]
      [Route(RouteConstants.ReadCategoryByKey)]
      public async Task<IActionResult> ReadCategoryByKey(int key)
      {
         try
         {
            if (key <= 0)
               return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

            var category = await context.CategoryRepository.GetByIdAsync(key);

            if (category == null)
               return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

            return Ok(category);
         }
         catch (Exception ex)
         {
            logger.LogError(ex.Message, "ReadCategoryByKey");
            return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
         }
      }

      /// <summary>
      /// URL: expenses-api/category/{key}
      /// </summary>
      /// <param name="key">Primary key of the table Districts.</param>
      /// <param name="district">District to be updated.</param>
      /// <returns>Http Status Code: NoContent.</returns>
      [HttpPut]
      [Route(RouteConstants.UpdateCategory)]
      public async Task<IActionResult> UpdateCategory(int key, Category category)
      {
         try
         {
            if (key != category.OID)
               return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

            if (await IsCategoryDuplicate(category) == true)
               return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

            category.DateModified = DateTime.Now;
            category.IsSynced = false;

            context.CategoryRepository.Update(category);
            await context.SaveChangesAsync();

            return StatusCode(StatusCodes.Status204NoContent);
         }
         catch (Exception ex)
         {
            logger.LogError(ex.Message, "UpdateCategory");
            return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
         }
      }

      /// <summary>
      /// URL: expenses-api/category/{key}
      /// </summary>
      /// <param name="key">Primary key of the table Categories.</param>
      /// <returns>Http status code: Ok.</returns>
      [HttpDelete]
      [Route(RouteConstants.DeleteCategory)]
      public async Task<IActionResult> DeleteCategory(int key)
      {
         try
         {

            if (key <= 0)
               return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

            var categoryInDb = await context.CategoryRepository.GetCategoryByKey(key);

            if (categoryInDb == null)
               return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

            categoryInDb.DateModified = DateTime.Now;
            categoryInDb.IsDeleted = true;
            categoryInDb.IsSynced = false;

            context.CategoryRepository.Update(categoryInDb);
            await context.SaveChangesAsync();

            return Ok(categoryInDb);
         }
         catch (Exception ex)
         {
            logger.LogError(ex.Message, "DeleteCategory");
            return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
         }
      }

      /// <summary>
      /// Checks whether the category name is duplicate or not.
      /// </summary>
      /// <param name="district">Category object.</param>
      /// <returns>Boolean</returns>
      private async Task<bool> IsCategoryDuplicate(Category category)
      {
         try
         {

            var catagoryInDb = await context.CategoryRepository.GetCategoryByName(category.Name);
            if (catagoryInDb != null)
               if (catagoryInDb.OID != category.OID)
                  return true;
            return false;

         }
         catch (Exception ex)
         {
            logger.LogError(ex.Message, "IsCategoryDuplicate");
            throw;
         }
      }
   }
}