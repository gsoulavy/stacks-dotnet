using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using xxAMIDOxx.xxSTACKSxx.API.Models.Requests;
using xxAMIDOxx.xxSTACKSxx.API.Models.Responses;
#if (ENABLE_CQRS)
using Amido.Stacks.Application.CQRS.Commands;
using xxAMIDOxx.xxSTACKSxx.CQRS.Commands;
#endif

namespace xxAMIDOxx.xxSTACKSxx.API.Controllers
{
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Item related operations
    /// </summary>
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "Item")]
    public class AddMenuItemController : ApiControllerBase
    {
#if (ENABLE_CQRS)
    	readonly ICommandHandler<CreateMenuItem, Guid> commandHandler;

        public AddMenuItemController(ICommandHandler<CreateMenuItem, Guid> commandHandler)
        {
            this.commandHandler = commandHandler;
        }
#else
        public AddMenuItemController()
        {
        }
#endif


        /// <summary>
        /// Create an item to a category in the menu
        /// </summary>
        /// <remarks>Adds a new item to a category in the menu</remarks>
        /// <param name="id">menu id</param>
        /// <param name="categoryId">Id for Category being removed</param>
        /// <param name="body">Category being added</param>
        /// <response code="201">Resource created</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Resource not found</response>
        /// <response code="409">Conflict, an item already exists</response>
        [HttpPost("/v1/menu/{id}/category/{categoryId}/items/")]
        [Authorize]
        [ProducesResponseType(typeof(ResourceCreatedResponse), 201)]
        public async Task<IActionResult> AddMenuItem([FromRoute][Required] Guid id, [FromRoute][Required] Guid categoryId, [FromBody] CreateItemRequest body)
        {
            // NOTE: Please ensure the API returns the response codes annotated above
#if (ENABLE_CQRS)
            var menuItemId = await commandHandler.HandleAsync(
                new CreateMenuItem(
                    correlationId: GetCorrelationId(),
                    menuId: id,
                    categoryId: categoryId,
                    name: body.Name,
                    description: body.Description,
                    price: body.Price,
                    available: body.Available
                )
            );
#else
            var menuItemId = Guid.NewGuid();
#endif
            return StatusCode(StatusCodes.Status201Created, new ResourceCreatedResponse(menuItemId));
        }
    }
}
