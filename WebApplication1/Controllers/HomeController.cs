using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebApplication1.Configs;
using WebApplication1.DITest;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IConfiguration _configuration;  
        //private IOptions<Database> _database;  //单例模式，配置文件发生改变，配置信息不会发生改变
        private IOptionsSnapshot<Database> _database;   //属于scope服务，针对每次请求都会重新加载配置文件
        //private IOptionsMonitor<Database> _database;  //自动监控配置文件,_database.CurrentValue,配置文件发生改变则读取
        private readonly IFoo _foo;
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, IOptionsSnapshot<Database> database, IFoo foo)
        {
            _logger = logger;
            _configuration = configuration;
            _database = database;
            _foo = foo;
        }

        public IActionResult Index()
        {
            //ViewBag.Name1=configuration["Name1"]   //读取配置文件节点信息
            //ViewBag.Name2=configuration["Name1:Name2"]   //读取配置文件节点的子节点信息，用冒号表示子节点
            //ViewBag.Name3 = configuration["Students:0:Name3"]   //读取配置文件节点的子节点信息，用冒号表示子节点。数组的话，用序号表示第几个节点
            Database db = _database.Value;
            ViewBag.database = $"Server:{db.Server},Name:{db.Name},UId:{db.UId},Password:{db.Password},";

            ViewBag.foo = _foo.GetInputString("Your application description page.");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
