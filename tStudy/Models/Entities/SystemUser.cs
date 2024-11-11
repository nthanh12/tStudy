using Microsoft.AspNetCore.Identity;

namespace tStudy.Models.Entities
{
    public class SystemUser : IdentityUser
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
