using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using xxAMIDOxx.xxSTACKSxx.API.Models.Responses;
#if (ENABLE_CQRS)
using Amido.Stacks.Application.CQRS.Queries;
using Query = xxAMIDOxx.xxSTACKSxx.CQRS.Queries.GetMenuById;
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
    public class GetMenuByIdController : ApiControllerBase
    {
#if (ENABLE_CQRS)
        readonly IQueryHandler<Query.GetMenuById, Query.Menu> queryHandler;

        public GetMenuByIdController(IQueryHandler<Query.GetMenuById, Query.Menu> queryHandler)
        {
            this.queryHandler = queryHandler;
        }
#else
        public GetMenuByIdController()
        {

        }
#endif

        /// <summary>
        /// Get a menu
        /// </summary>
        /// <remarks>By passing the menu id, you can get access to available categories and items in the menu </remarks>
        /// <param name="id">menu id</param>
        /// <response code="200">Menu</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Resource not found</response>
        [HttpGet("/v1/menu/{id}")]
        [Authorize]
        [ProducesResponseType(typeof(Menu), 200)]
        public async Task<IActionResult> GetMenu([FromRoute][Required] Guid id)
        {
            // NOTE: Please ensure the API returns the response codes annotated above
#if (ENABLE_CQRS)
            var result = await queryHandler.ExecuteAsync(new Query.GetMenuById() { Id = id });

            if (result == null)
                return NotFound();
#else
            var result = new Menu()
            {
                Id = id,
                Description = "Menu description",
                Categories = new List<Category>()
                {
                    new Category() {
                        Id = Guid.NewGuid(),
                        Description = "Category Description",
                         Name = "Category name",
                         Items = new List<Item>()
                         {
                             new Item()
                             {
                                 Id = Guid.NewGuid(),
                                 Name = "Item name 1",
                                 Description = "Item description 1",
                                 Available = true,
                                 Price = 10
                             },
                             new Item()
                             {
                                 Id = Guid.NewGuid(),
                                 Name = "Item name 2",
                                 Description = "Item description 2",
                                 Available = true,
                                 Price = 10
                             }
                         }
                    }
                },
                Enabled = true,
                Name = "Menu name"
            };
#endif

            return new ObjectResult(result);
        }
    }
}
