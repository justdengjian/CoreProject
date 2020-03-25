using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public interface IResultTypeRepository
    {
        Task<ResultType> GetByIdAsync(int Id);
        Task<List<ResultType>> ListAsync();
        Task AddAsync(ResultType result);
        Task<bool> EditAsync(ResultType result);

    }
}
