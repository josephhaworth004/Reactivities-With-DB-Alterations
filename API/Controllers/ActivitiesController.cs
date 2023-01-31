using Domain;
using Microsoft.AspNetCore.Mvc;
using Application.Activities;
using Application;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    public class ActivitiesController : BaseAPIController
    {
        // Define end-points
        // Utilize Dependency Injection for putting datacontext in api controller class 
        [Microsoft.AspNetCore.Mvc.HttpGet] // Will use: api//activities. 
       
        //Returns list of activities.
        // In the form <ActionResult>
        public async Task<IActionResult> GetActivities()
        {
            // Use send to send the query (<List<Activity>>> GetActivities()) to handlers
            return HandleResult(await Mediator.Send(new List.Query()));
        }

        //[Authorize]
        [Microsoft.AspNetCore.Mvc.HttpGet("{id}")] // Route parameter: api/activities/GUID      
        // Register Mediator as a service inside our program class
        public async Task <IActionResult> GetActivity(Guid id)
        {
            return HandleResult(await Mediator.Send(new Details.Query{Id = id}));   
        }
        
       [Microsoft.AspNetCore.Mvc.HttpPost] 
        public async Task<IActionResult> CreateActivity(Activity activity) {
            // Something needs to be returned
            return HandleResult(await Mediator.Send(new Create.Command {Activity = activity}));
        }

        [Authorize(Policy ="IsActivityHost")]
        [Microsoft.AspNetCore.Mvc.HttpPut("{id}")]
        //[HttpPut("{id}")]
        public async Task<IActionResult> EditActivity(Guid id, Activity activity)
        {
            activity.Id = id;
            return HandleResult(await Mediator.Send(new Edit.Command{Activity = activity}));    
        }

        [Authorize(Policy ="IsActivityHost")]
        [Microsoft.AspNetCore.Mvc.HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(Guid id)
        {
            return HandleResult(await Mediator.Send(new Delete.Command{Id = id}));
        }

        [HttpPost("{id}/attend")]
        public async Task<IActionResult> Attend(Guid id)
        {
            return HandleResult(await Mediator.Send(new UpdateAttendance.Command { Id = id }));
        }

    }
}