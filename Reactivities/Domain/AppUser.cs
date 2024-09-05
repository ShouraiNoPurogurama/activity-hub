using Microsoft.AspNetCore.Identity;

namespace Domain;

public class AppUser : IdentityUser
{
    public string DisplayName { get; set; }
    public string Bio { get; set; }
    
    public string Email { get; set; }

    public AppUser(string displayName, string bio, string email)
    {
        DisplayName = displayName;
        Bio = bio;
        Email = email;
    }
}