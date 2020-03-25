using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class ResultTypeRepository : IResultTypeRepository
    {
        private ResultContext resultContext;
        public ResultTypeRepository(ResultContext _resultContext)
        {
            resultContext = _resultContext;
        }
        public Task AddAsync(ResultType result)
        {
            resultContext.ResultTypes.AddAsync(result);
            return resultContext.SaveChangesAsync();
        }

        public async Task<bool> EditAsync(ResultType result)
        {
            resultContext.ResultTypes.Update(result);
            return await resultContext.SaveChangesAsync() > 0;
        }

        public Task<ResultType> GetByIdAsync(int Id)
        {
            return resultContext.ResultTypes.FirstOrDefaultAsync(r => r.Id == Id);
        }

        public Task<List<ResultType>> ListAsync()
        {
            return resultContext.ResultTypes.ToListAsync();
        }
    }
}
