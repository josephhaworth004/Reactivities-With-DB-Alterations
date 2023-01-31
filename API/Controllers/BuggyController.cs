using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseAPIController
    {
        [Microsoft.AspNetCore.Mvc.HttpGet("not-found")]
        public ActionResult GetNotFound()
        {
            return NotFound();
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("bad-request")]
        public ActionResult GetBadRequest()
        {
            return BadRequest("This is a bad request");
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("server-error")]
        public ActionResult GetServerError()
        {
            throw new Exception("This is a server error");
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("unauthorised")]
        public ActionResult GetUnauthorised()
        {
            return Unauthorized();
        }
    }
}
