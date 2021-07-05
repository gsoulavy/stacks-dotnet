using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using xxAMIDOxx.xxSTACKSxx.API.Models.Requests;
using xxAMIDOxx.xxSTACKSxx.API.Models.Responses;
using xxAMIDOxx.xxSTACKSxx.CQRS.Commands;

namespace xxAMIDOxx.xxSTACKSxx.API.Controllers
{
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Category related operations
    /// </summary>
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "Category")]
    public class AddMenuCategoryController : ApiControllerBase
    {

#if (ENABLE_CQRS)
        readonly ICommandHandler<CreateCategory, Guid> commandHandler;

        public AddMenuCategoryController(ICommandHandler<CreateCategory, Guid> commandHandler)
        {
            this.commandHandler = commandHandler;
        }
#else
        public AddMenuCategoryController()
        {
        }
#endif

        /// <summary>
        /// Create a category in the menu
        /// </summary>
        /// <remarks>Adds a category to menu</remarks>
        /// <param name="id">menu id</param>
        /// <param name="body">Category being added</param>
        /// <response code="201">Resource created</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Resource not found</response>
        /// <response code="409">Conflict, an item already exists</response>
        [HttpPost("/v1/menu/{id}/category/")]
        [Authorize]
        [ProducesResponseType(typeof(ResourceCreatedResponse), StatusCodes.Status201Created)]
        public async Task<IActionResult> AddMenuCategory([FromRoute][Required] Guid id, [FromBody] CreateCategoryRequest body)
        {
            // NOTE: Please ensure the API returns the response codes annotated above

#if (ENABLE_CQRS)
		var categoryId = await commandHandler.HandleAsync(
				new CreateCategory(
					correlationId: GetCorrelationId(),
					menuId: id,
					name: body.Name,
					description: body.Description
				)
			);

			return StatusCode(StatusCodes.Status201Created, new ResourceCreatedResponse(categoryId));
#else
            return StatusCode(StatusCodes.Status201Created);
#endif
        }
    }
}
