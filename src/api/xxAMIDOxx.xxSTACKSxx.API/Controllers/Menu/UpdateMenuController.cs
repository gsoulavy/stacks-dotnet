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
    /// Menu related operations
    /// </summary>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiExplorerSettings(GroupName = "Menu")]
    [ApiController]
    public class UpdateMenuController : ApiControllerBase
    {
#if (ENABLE_CQRS)
        readonly ICommandHandler<UpdateMenu, bool> commandHandler;

        public UpdateMenuController(ICommandHandler<UpdateMenu, bool> commandHandler)
        {
            this.commandHandler = commandHandler;
        }
#else
        public UpdateMenuController()
        {

        }
#endif


        /// <summary>
        /// Update a menu
        /// </summary>
        /// <remarks>Update a menu with new information</remarks>
        /// <param name="id">menu id</param>
        /// <param name="body">Menu being updated</param>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Resource not found</response>
        [HttpPut("/v1/menu/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateMenu([FromRoute][Required] Guid id, [FromBody] UpdateMenuRequest body)
        {
            // NOTE: Please ensure the API returns the response codes annotated above
#if (ENABLE_CQRS)
            await commandHandler.HandleAsync(
                new UpdateMenu()
                {
                    MenuId = id,
                    Name = body.Name,
                    Description = body.Description,
                    Enabled = body.Enabled
                });
#endif

            return StatusCode(204);
        }
    }
}
