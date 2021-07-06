using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using xxAMIDOxx.xxSTACKSxx.API.Models.Responses;
#if (ENABLE_CQRS)
using Amido.Stacks.Application.CQRS.Queries;
using xxAMIDOxx.xxSTACKSxx.CQRS.Queries.SearchMenu;
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
    public class SearchMenuController : ApiControllerBase
    {
#if (ENABLE_CQRS)
        readonly IQueryHandler<SearchMenu, SearchMenuResult> queryHandler;

        public SearchMenuController(IQueryHandler<SearchMenu, SearchMenuResult> queryHandler)
        {
            this.queryHandler = queryHandler;
        }
#else
        public SearchMenuController()
        {

        }
#endif

        /// <summary>
        /// Get or search a list of menus
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can search for available menus in the system </remarks>
        /// <param name="searchTerm">pass an optional search string for looking up menus</param>
        /// <param name="RestaurantId">id of restaurant to look up for menu's</param>
        /// <param name="pageNumber">page number</param>
        /// <param name="pageSize">maximum number of records to return per page</param>
        /// <response code="200">search results matching criteria</response>
        /// <response code="400">bad request</response>
        [HttpGet("/v1/menu/")]
        [Authorize]
        [ProducesResponseType(typeof(SearchMenuResponse), 200)]
        public async Task<IActionResult> SearchMenu(
            [FromQuery] string searchTerm,
            [FromQuery] Guid? RestaurantId,
            [FromQuery][Range(0, 50)] int? pageSize = 20,
            [FromQuery] int? pageNumber = 1)
        {
            // NOTE: Please ensure the API returns the response codes annotated above
#if (ENABLE_CQRS)
 			var criteria = new SearchMenu(
                correlationId: GetCorrelationId(),
                searchText: searchTerm,
                restaurantId: RestaurantId,
                pageSize: pageSize,
                pageNumber: pageNumber
            );

			var results = await queryHandler.ExecuteAsync(criteria);
			return new ObjectResult(results);
#else
            var response = new SearchMenuResponse()
            {
                Offset = 0,
                Size = 0,
                Results = new List<SearchMenuResponseItem>()
                // Offset = (results?.PageNumber ?? 0) * (results?.PageSize ?? 0),
                // Size = (results?.PageSize ?? 0),
                // Results = results.Results.Select(i => new SearchMenuResponseItem()
                // {
                //     Id = i.Id ?? Guid.Empty,
                //     Name = i.Name,
                //     Description = i.Description,
                //     Enabled = i.Enabled ?? false
                // }).ToList()
            };

            return new ObjectResult(response);
#endif
        }
    }
}
