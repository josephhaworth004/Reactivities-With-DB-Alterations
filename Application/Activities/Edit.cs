using Application.Core;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Edit
    {
         // Command not a query
        public class Command : IRequest<Result<Unit>>
        {
            public Activity Activity { get; set; }
        } 

        public class CommandValidator : AbstractValidator<Command> {
            // Constructor for validator
            public CommandValidator()
            {
                RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
            }        
        }

        public class Handler : IRequestHandler<Command, Result<Unit>> {
            // constructor
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                // Get activity from database to edit fields
                var activity = await _context.Activities.FindAsync(request.Activity.Id);

                if(activity == null) return null;

                // Mapper takes properties from class Activity and update the properties of the db
                _mapper.Map(request.Activity, activity); // Mapper injected in constructor
                // Save changes to DB
                var result = await _context.SaveChangesAsync() > 0; // Updates db
               
                if(!result) return Result<Unit>.Failure("Failed to edit activity");

                return Result<Unit>.Success(Unit.Value);
                
              
            }
        }
    }
}