using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class ResultRepository : IResultRepository
    {
        private ResultContext resultContext;
        public ResultRepository(ResultContext _resultContext)
        {
            resultContext = _resultContext;
        }
        public Task AddAsync(Result result)
        {
            resultContext.Results.AddAsync(result);
            return resultContext.SaveChangesAsync();
        }

        public async Task<bool> EditAsync(Result result)
        {
            resultContext.Results.Update(result);
            resultContext.Entry(result).Property(p => p.Create).IsModified = false;            
            return await resultContext.SaveChangesAsync() > 0;
        }

        public Task<Result> GetByIdAsync(int Id)
        {
            return resultContext.Results.FirstOrDefaultAsync(r => r.Id == Id);
        }

        public Task<List<Result>> ListAsync()
        {
            return resultContext.Results.Include<Result,ResultType>(r=>r.Type).ToListAsync();
        }

        public List<Result> PageList(int pageindex, int pagesize, out int pagecount)
        {
            var query = resultContext.Results.Include<Result, ResultType>(r => r.Type).AsQueryable();
            var count = query.Count();
            pagecount = count % pagesize == 0 ? count / pagesize : count / pagesize + 1;
            var results = query.OrderByDescending(r => r.Create).Skip((pageindex - 1) * pagesize).Take(pageindex * pagesize).ToList();

            return results;
        }

        public async Task<bool> DeleteAsync(Result result)
        {
            resultContext.Entry<Result>(result).State = EntityState.Deleted;
            return await resultContext.SaveChangesAsync() > 0;
        }
    }
}
