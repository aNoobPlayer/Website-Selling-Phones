using Microsoft.AspNetCore.Identity;

namespace Website_Selling_Phones.Models;

public class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}