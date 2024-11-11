using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using tStudy.Models.Entities;

namespace tStudy.Models.Data
{
    public class tStudyDbContext : IdentityDbContext<SystemUser, IdentityRole, string,
            IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>,
            IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public tStudyDbContext(DbContextOptions<tStudyDbContext> dbOptions) : base(dbOptions) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
