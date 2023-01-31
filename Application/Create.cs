using Application.Activities;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;
using Application.Core;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application
{
    public class Create
    {
        // Command does not return anything. We are passing IRequestor
        // This is the difference between a command and a query.
        // Queries return data, commands do not
        public class Command : IRequest<Result<Unit>> {
            // Pass an object of type Activity to this handler when we create it
            // Below is what we want to receive as a parameter from our API
            public Activity Activity { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command> {
            // Constructor for validator
            public CommandValidator()
            {
                RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
            }        
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            // Constructor injects DataContext so we can persist our changes 
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _context = context;
            }
                                          
            // Our request should hold an activity object
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x =>
                    x.UserName == _userAccessor.GetUsername());

                var attendee = new ActivityAttendee
                {
                    AppUser = user,
                    Activity = request.Activity,
                    IsHost = true
                };

                request.Activity.Attendees.Add(attendee);
                
                _context.Activities.Add(request.Activity);
                
                var result = await _context.SaveChangesAsync() > 0;

                if(!result) return Result<Unit>.Failure("Failed to create activity");
                
                return  Result<Unit>.Success(Unit.Value);
            }
        }
    }
}