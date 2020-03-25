using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public interface IResultRepository
    {
        Task<Result> GetByIdAsync(int Id);
        Task<List<Result>> ListAsync();
        Task AddAsync(Result result);
        Task<bool> EditAsync(Result result);

        List<Result> PageList(int pageindex,int pagesize,out int pagecount);

        Task<bool> DeleteAsync(Result result);

    }
}
