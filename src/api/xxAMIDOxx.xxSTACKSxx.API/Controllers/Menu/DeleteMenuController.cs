using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
#if (ENABLE_CQRS)
using Amido.Stacks.Application.CQRS.Commands;
using xxAMIDOxx.xxSTACKSxx.CQRS.Commands;
#endif

namespace xxAMIDOxx.xxSTACKSxx.API.Controllers
{
    /// <summary>
    /// Menu related operations
    /// </summary>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiExplorerSettings(GroupName = "Menu")]
    [ApiController]
    public class DeleteMenuController : ApiControllerBase
    {
#if (ENABLE_CQRS)
        readonly ICommandHandler<DeleteMenu, bool> commandHandler;

        public DeleteMenuController(ICommandHandler<DeleteMenu, bool> commandHandler)
        {
            this.commandHandler = commandHandler;
        }
#else
        public DeleteMenuController()
        {

        }
#endif

        /// <summary>
        /// Removes a menu with all its categories and items
        /// </summary>
        /// <remarks>Remove a menu from a restaurant</remarks>
        /// <param name="id">menu id</param>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Resource not found</response>
        [HttpDelete("/v1/menu/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteMenu([FromRoute][Required] Guid id)
        {
            // NOTE: Please ensure the API returns the response codes annotated above
#if (ENABLE_CQRS)
            await commandHandler.HandleAsync(new DeleteMenu(id));
#endif
            return StatusCode(204);
        }
    }
}
