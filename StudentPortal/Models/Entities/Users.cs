using Microsoft.AspNetCore.Identity;

namespace StudentPortal.Models.Entities
{
    public class Users : IdentityUser
    {
        public string FullName { get; set; }
    }
}
