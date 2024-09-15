using Application.Activities;
using AutoMapper;
using Domain;

namespace Application.Core;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Activity, Activity>();
        CreateMap<Activity, ActivityDto>()
            .ForMember(d => d.HostUsername,
                o =>
                    o.MapFrom(a => a.Attendees.FirstOrDefault(aa => aa.IsHost).AppUser.UserName))
            ;
        CreateMap<ActivityAttendee, AttendeeDto>()
            .ForMember(d => d.DisplayName, o => o.MapFrom(
                s => s.AppUser.DisplayName))
            .ForMember(d => d.Username, o => o.MapFrom(
                s => s.AppUser.UserName))
            .ForMember(d => d.Bio, o => o.MapFrom(
                s => s.AppUser.Bio))
            .ForMember(d => d.Image,
                opt =>
                    opt.MapFrom(src => src.AppUser.Photos.FirstOrDefault(x => x.IsMain).Url))
            ;

        //Map user Main photo to profile photo
        CreateMap<AppUser, Profiles.Profile>()
            .ForMember(d => d.Image,
                opt =>
                    opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
            ;
    }
}