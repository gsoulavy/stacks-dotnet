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
    /// <summary>
    /// Menu related operations
    /// </summary>
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "Menu")]
    [ApiController]
    public class CreateMenuController : ApiControllerBase
    {
#if (ENABLE_CQRS)
        readonly ICommandHandler<CreateMenu, Guid> commandHandler;

        public CreateMenuController(ICommandHandler<CreateMenu, Guid> commandHandler)
        {
            this.commandHandler = commandHandler;
        }
#else
        public CreateMenuController()
        {

        }
#endif

        /// <summary>
        /// Create a menu
        /// </summary>
        /// <remarks>Adds a menu</remarks>
        /// <param name="body">Menu being created</param>
        /// <response code="201">Resource created</response>
        /// <response code="400">Bad Request</response>
        /// <response code="409">Conflict, an item already exists</response>
        [HttpPost("/v1/menu/")]
        [Authorize]
        [ProducesResponseType(typeof(ResourceCreatedResponse), 201)]
        public async Task<IActionResult> CreateMenu([Required][FromBody] CreateMenuRequest body)
        {
            // NOTE: Please ensure the API returns the response codes annotated above
#if (ENABLE_CQRS)
            var id = await commandHandler.HandleAsync(
                new CreateMenu(
                        correlationId: GetCorrelationId(),
                        tenantId: body.TenantId, //Should check if user logged-in owns it
                        name: body.Name,
                        description: body.Description,
                        enabled: body.Enabled
                    )
                );
#else
            var id = Guid.NewGuid();
#endif
            return new CreatedAtActionResult(
                    "GetMenu", "GetMenuById", new
                    {
                        id = id
                    }, new ResourceCreatedResponse(id)
            );
        }
    }
}
