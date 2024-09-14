﻿using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace Domain;

public sealed class AppUser : IdentityUser
{
    public string DisplayName { get; set; } 
    public string? Bio { get; set; }
    public ICollection<ActivityAttendee> Activities { get; set; }
    
    public ICollection<Photo> Photos { get; set; }
}