using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class List
    {
        // Will derive from and use the interface from Mediator
        // Tell class what it will return - a List object of The Activity object
        // Can have parameters but don't need them as it just returns a list
        public class Query : IRequest<Result<List<ActivityDto>>> {} 

        // Handler class will use the IRequest interface from MediatR 
        // Pass it the query as 1st parameter and what we are returning, List of Activity, as the 2nd
        public class Handler : IRequestHandler<Query, Result<List<ActivityDto>>>
        {
            // Constructor to inject our task. 
            private readonly DataContext _context;
            // Bring in DataContext and call it context
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper) {
                _mapper = mapper;
                _context = context;    
            }

            // Returns a task of List of Activity
            // Because at some point it's returning a task it needs to be an async method
            // Params: Request passing in, thats our query. Ignore CancellationToken for now
            public async Task<Result<List<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {   
                var activities = await _context.Activities
                    .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken); 
                              
                return Result<List<ActivityDto>>.Success(activities);
            }
        }
    }
}