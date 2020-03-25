using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.ViewModel;

namespace WebApplication1.Models
{
    public class ResultContext : IdentityDbContext
    {
        public ResultContext(DbContextOptions<ResultContext> options):base(options)
        {

        }
        public DbSet<Result> Results { get; set; }
        public DbSet<ResultType> ResultTypes { get; set; }
        public DbSet<WebApplication1.ViewModel.ResultModel> ResultModel { get; set; }

        public DbSet<ResultUser> ResultUsers { get; set; }

        public DbSet<WebApplication1.ViewModel.LoginViewModel> LoginViewModel { get; set; }

        public DbSet<WebApplication1.ViewModel.RegisterViewModel> RegisterViewModel { get; set; }

    }
}
