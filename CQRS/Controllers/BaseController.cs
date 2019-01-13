using CQRS.Business.Utils;
using Microsoft.AspNetCore.Mvc;

namespace CQRS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected IActionResult FromResult(Results result)
        {
            switch (result.Status)
            {
                case State.noContent:
                    return NoContent();

                case State.badRequest:
                    return BadRequest(result.ResponseMessage);

                case State.notFound:
                    return NotFound(result.ResponseMessage);

                case State.unProcessableEntity:
                    return UnprocessableEntity(result.ResponseMessage);

                case State.ok:
                    return Ok(result.ResponseMessage);

                default:
                    return null;
            }

            //if (result.IsSuccessful == true &&
            //    result.ResponseMessage == null)
            //    return NoContent();

            //return result.IsSuccessful ?
            //    Ok(result.ResponseMessage) :
            //    NotFound(result.ResponseMessage);
        }
    }
}