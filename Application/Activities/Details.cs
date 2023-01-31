using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class Details
    {
        // Query needs "using MediatR" , IRequest needs "Using Domain"
        // Below is going to be fetching data not receiving
        // Going to return a single activity
        public class Query : IRequest<Result<ActivityDto>>
        {
            // Will have access to below inside our handler (ActivitiesController.cs)
            public Guid Id { get; set;} 
        }

        // Handler Class. Inherits the IRequest Handler
        // Takes Query as a parameter and returns a single Activity
        public class Handler : IRequestHandler<Query, Result<ActivityDto>>
        {
            // Constructor to bring in DataContext needs using persistance
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            // Constructor
            public Handler (DataContext context, IMapper mapper) {
                _mapper = mapper;
                _context = context;
            }
            public async Task<Result<ActivityDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                // Id here is got from the query request injected just above
                var activity = await _context.Activities
                    .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(x => x.Id == request.Id);
                
                return Result<ActivityDto>.Success(activity);

            }
        }
    }
}