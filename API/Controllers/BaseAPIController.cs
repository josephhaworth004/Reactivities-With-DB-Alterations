using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    //Controllers decorated with this attribute are configured with features and behavior targeted at // improving the developer experience for building APIs.
    [ApiController] // ApiController attribute works with data annotation - see commented ut  Activity.cs 
    [Route("api/[controller]")]
    public class BaseAPIController : ControllerBase // All my controllers derive from ControllerBase
    {
        private IMediator _mediator;
    
        //Can be used in any classes derived of this one
        // => as we are going to return one of two options 
        // ??= if 1st arg is null, assign to the right of ??= or if it already exists, return it.
        protected IMediator Mediator => _mediator ??=
            HttpContext.RequestServices.GetService<IMediator>();

        protected ActionResult HandleResult<T>(Application.Core.Result<T> result) {
            if(result == null) return NotFound();
            if(result.IsSuccess && result.Value != null)
                return Ok(result.Value); 
            if(result.IsSuccess && result.Value == null)
                return NotFound();
            return BadRequest(result.Error);    
        }
    }
}