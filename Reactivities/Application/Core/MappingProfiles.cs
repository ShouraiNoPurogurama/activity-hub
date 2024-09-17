using Application.Activities;
using Application.Comments;
using AutoMapper;
using Domain;

namespace Application.Core;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        string? currentUsername = null;
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
            .ForMember(d => d.FollowersCount, opt => opt.MapFrom(src => src.AppUser.Followers.Count))
            .ForMember(d => d.FollowingCount, opt => opt.MapFrom(src => src.AppUser.Followings.Count))
            .ForMember(d => d.Following,
                opt => opt.MapFrom(src => src.AppUser.Followers.Any(f => f.Observer.UserName == currentUsername)))
            ;
            ;

        //Map user Main photo to profile photo
        CreateMap<AppUser, Profiles.Profile>()
            .ForMember(d => d.Image,
                opt =>
                    opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
            .ForMember(d => d.FollowersCount, opt => opt.MapFrom(src => src.Followers.Count))
            .ForMember(d => d.FollowingCount, opt => opt.MapFrom(src => src.Followings.Count))
            .ForMember(d => d.Following,
                opt => opt.MapFrom(src => src.Followers.Any(f => f.Observer.UserName == currentUsername)))
            ;
        CreateMap<Comment, CommentDto>()
            .ForMember(d => d.DisplayName, o => o.MapFrom(
                s => s.Author.DisplayName))
            .ForMember(d => d.Username, o => o.MapFrom(
                s => s.Author.UserName))
            .ForMember(d => d.Image,
                opt =>
                    opt.MapFrom(src => src.Author.Photos.FirstOrDefault(x => x.IsMain).Url));
    }
}