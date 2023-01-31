using Application.Activities;
using AutoMapper;
using Domain;

// For common things like creating, editing etc
namespace Application.Core
{
    public class MappingProfiles : Profile // profile and Auto-Mapper class
    {
        public MappingProfiles()
        {
            // Auto Mapper will look at properties in class Activity and match them to the activity table in the db
            CreateMap<Activity, Activity>();
            CreateMap<Activity, ActivityDto>()
                .ForMember(d => d.HostUsername, o => o.MapFrom(s => s.Attendees
                    .FirstOrDefault(x => x.IsHost).AppUser.UserName)); 
            CreateMap<ActivityAttendee, Profiles.Profile>()
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.AppUser.DisplayName))    
                .ForMember(d => d.Username, o => o.MapFrom(s => s.AppUser.UserName))    
                .ForMember(d => d.Bio, o => o.MapFrom(s => s.AppUser.Bio));    
        }   
    }
}