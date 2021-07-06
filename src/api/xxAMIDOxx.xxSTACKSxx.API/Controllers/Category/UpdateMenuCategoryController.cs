using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using xxAMIDOxx.xxSTACKSxx.API.Models.Requests;
#if (ENABLE_CQRS)
using Amido.Stacks.Application.CQRS.Commands;
using xxAMIDOxx.xxSTACKSxx.CQRS.Commands;
#endif

namespace xxAMIDOxx.xxSTACKSxx.API.Controllers
{
    /// <summary>
    /// Category related operations
    /// </summary>
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "Category")]
    public class UpdateMenuCategoryController : ApiControllerBase
    {

#if (ENABLE_CQRS)
    	readonly ICommandHandler<UpdateCategory, bool> commandHandler;

        public UpdateMenuCategoryController(ICommandHandler<UpdateCategory, bool> commandHandler)
        {
            this.commandHandler = commandHandler;
        }
#else
        public UpdateMenuCategoryController()
        {
        }
#endif


        /// <summary>
        /// Update a category in the menu
        /// </summary>
        /// <remarks>Update a category to menu</remarks>
        /// <param name="id">menu id</param>
        /// <param name="categoryId">Id for Category being removed</param>
        /// <param name="body">Category being added</param>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Resource not found</response>
        /// <response code="409">Conflict, an item already exists</response>
        [HttpPut("/v1/menu/{id}/category/{categoryId}")]
        [Authorize]
        public async Task<IActionResult> UpdateMenuCategory([FromRoute][Required] Guid id, [FromRoute][Required] Guid categoryId, [FromBody] UpdateCategoryRequest body)
        {
            // NOTE: Please ensure the API returns the response codes annotated above
#if (ENABLE_CQRS)
            await commandHandler.HandleAsync(
                new UpdateCategory(
                    correlationId: GetCorrelationId(),
                    menuId: id,
                    categoryId: categoryId,
                    name: body.Name,
                    description: body.Description
                )
            );
#endif

            return StatusCode(204);
        }
    }
}
