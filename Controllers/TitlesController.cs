using Microsoft.AspNetCore.Mvc;
using movies_api.Contracts.Results;
using movies_api.Contracts.DTOs;
using movies_api.Contracts.ServiceInterfaces;

namespace movies_api.Controllers
{
    [Route("api/title")]
    [ApiController]
    // Single responsibility: provide CRUD endpoints for the Title entity.
    public class TitleController : ControllerBase
    {   
        private readonly IRepository<TitleDto> _titleRepository;
        public TitleController(
            IRepository<TitleDto> titleRepository)
        {
            _titleRepository = titleRepository;
        }

        [Route("list")]
        [HttpGet]
        public async Task<ActionResult<GetTitleListResult>> GetTitleList([FromQuery] string? cursor = null, [FromQuery] int pageSize = 10, [FromQuery] string? filter = null)
        {
            try
            {
                List<TitleDto> titles = await _titleRepository.GetList(cursor, pageSize, filter);
                // Set the cursor to the id of the last result in titles. Set it to null if last page was recieved from the database.
                string? nextCursor = titles.Count > pageSize ? titles.Last().Id : null;
                // Remove the last result from titles if there are more pages left.
                if (nextCursor != null)
                {
                    titles.RemoveAt(titles.Count - 1);
                }

                return Ok(new GetTitleListResult(titles, nextCursor));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500);
            }
        }
    }
}