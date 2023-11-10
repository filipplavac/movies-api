using Microsoft.AspNetCore.Mvc;
using movies_api.Contracts.Results;
using movies_api.Contracts.DTOs;
using movies_api.Contracts.ServiceInterfaces;

namespace movies_api.Controllers
{
    [Route("api/titles")]
    [ApiController]
    public class TitlesController : ControllerBase
    {   
        private IRepository<TitleDto> _titleRepository;
        public TitlesController(
            IRepository<TitleDto> titleRepository)
        {
            _titleRepository = titleRepository;
        }

        [Route("list")]
        [HttpGet]
        public async Task<ActionResult<TitleListResult>> GetTitleList([FromQuery] string? cursor = null, [FromQuery] int pageSize = 10)
        {
            try
            {
                List<TitleDto> titles = await _titleRepository.GetList(cursor, pageSize);
                // Set the cursor to the id of the last result in titles. Set it to null if last page was recieved from the database.
                string? nextCursor = titles.Count > pageSize ? titles.Last().Id : null;
                // Remove the last result from titles if there are more pages left.
                if (nextCursor != null)
                {
                    titles.RemoveAt(titles.Count - 1);
                }

                return Ok(new TitleListResult(titles, nextCursor));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500);
            }
        }
    }
}