using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.Models;
using WebApplication1.Repository;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class ResultController : Controller
    {
        private IResultRepository resultRepository;
        private IResultTypeRepository resultTypeRepository;
        public ResultController(IResultRepository _resultRepository, IResultTypeRepository _resultTypeRepository)
        {
            resultRepository = _resultRepository;
            resultTypeRepository = _resultTypeRepository;
        }
        //public async Task<IActionResult> Index()
        //{
        //    var result =await resultRepository.ListAsync();
        //    return View(result);
        //}

        public IActionResult Index(int pageindex=1,int pagesize=5)
        {
            var result = resultRepository.PageList(pageindex, pagesize,out int pagecount);
            ViewBag.PageCount = pagecount;
            ViewBag.PageIndex = pageindex;
            return View(result);
        }

        public async Task<IActionResult> Add()
        {
            var types = await resultTypeRepository.ListAsync();
            ViewBag.types = types.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id.ToString()
            });
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(ResultModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await resultRepository.AddAsync(new Models.Result
            {
                StuName = model.StuName,
                Title = model.Title,
                Description = model.Description,
                Create = DateTime.Now,
                TypeId=model.TypeId
            }); ;
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var types = await resultTypeRepository.ListAsync();
            ViewBag.types = types.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id.ToString()
            });
            Result result = await resultRepository.GetByIdAsync(id);
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Result model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await resultRepository.EditAsync(model); 
            return RedirectToAction("Index");
        }
        //public async Task<IActionResult> Edit(ResultModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    await resultRepository.EditAsync(new Models.Result
        //    {
        //        Id=model.Id,
        //        StuName = model.StuName,
        //        Title = model.Title,
        //        Description = model.Description,
        //        TypeId = model.TypeId
        //    }); ;
        //    return RedirectToAction("Index");
        //}

        public async Task<IActionResult> Delete(int id)
        {
            Result result = await resultRepository.GetByIdAsync(id);
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id,string name)
        {
            Result result = await resultRepository.GetByIdAsync(id);
            await resultRepository.DeleteAsync(result);
            return RedirectToAction("Index");
        }
    }
}