using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Task_Management_System.Models;
namespace Task_Management_System.Data

{
    public class ApplicationDBContext : IdentityDbContext 
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
            
        }
        public DbSet<TaskInfos> TaskInfos{ get; set; }

    }
}

