using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LPFW.WebApplication.Models;

namespace LPFW.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        public Guid Pid;

        public IActionResult Index()
        {
            var publicLinkCollection = new List<PublicLinkItemVM>()
            {
                new PublicLinkItemVM(){ Title="音乐管理", Description="修改音乐管理修改用户资源。", Url="/Music/Music" },
                new PublicLinkItemVM(){ Title="堡垒音乐UI", Description="播放音乐，MV播放。", Url="/MusicUI/Home" },
                //new PublicLinkItemVM(){ Title="注册", Description="点开进来K歌吧。", Url="/MusicUI/Account/Register" },
                //new PublicLinkItemVM(){ Title="登录注册", Description="包括常规的课程学习内容管理子系统等。", Url="/Student" },
                new PublicLinkItemVM(){ Title="音乐数据管理", Description="音乐数据的维护管理", Url="/News" },
                //new PublicLinkItemVM(){ Title="框架模型演示", Description="包含最基本的应用模板的常规处理维护", Url="/XDemo" },
            };

            ViewData["PublicLinkCollection"]= publicLinkCollection;

            var sprintItemCollection = new List<SprintItem>()
            {
                new SprintItem()
                {
                    Counter =1,
                    Version ="0.30.00",
                    Description ="包含系统用户和角色、组织机构和人员、供应商和采购商组织机构、商品基础信息、订单模型、订单层级合成与分解等系统中最基本的数据应用模型。这一期应用以桌面浏览器应用提供。",
                    StartDate ="2019-11-02",
                    FinishedDate ="2019-12-20"
                },
                new SprintItem()
                {
                    Counter =2,
                    Version ="0.40.00",
                    Description ="实现采购商形成初步的线上采购作业，提交采购订单。实现供应商初步的商品供应管理，可响应采购订单。",
                    StartDate ="2019-12-23",
                    FinishedDate ="2020-01-20"
                },
                new SprintItem()
                {
                    Counter =3,
                    Version ="0.60.00 [预览版本] ",
                    Description ="实现采购订单线上交易撮合服务，构成商品交易数据链。实现采购订单线上交易撮合服务，构成商品交易数据链。首个预览发布版本，包括桌面的和移动 APP的。",
                    StartDate ="2020-02-03",
                    FinishedDate ="2020-03-23"
                },
                new SprintItem()
                {
                    Counter =4,
                    Version ="0.70.00 ",
                    Description ="持续优化桌面的和移动 APP的应用，增加物流服务、金融服务、即时消息、通知消息服务。",
                    StartDate ="2020-03-26",
                    FinishedDate ="2020-04-22"
                },
                new SprintItem()
                {
                    Counter =5,
                    Version ="0.80.00 ",
                    Description ="持续优化桌面的和移动 APP的应用，增加社区电商服务平台，进一步优化营运单位内部管理功能。",
                    StartDate ="2020-04-27",
                    FinishedDate ="2020-05-11"
                },
                new SprintItem()
                {
                    Counter =6,
                    Version ="1.00.00 ",
                    Description ="持续优化桌面的和移动 APP的应用，进一步优化营运单位内部管理功能。",
                    StartDate ="2020-05-15",
                    FinishedDate ="2020-06-30"
                },
            };
            ViewData["SprintItemCollection"] = sprintItemCollection;
            return View();
        }

        public IActionResult Privacy()
        {
            var sprintItemCollection = new List<SprintItem>()
            {
                new SprintItem()
                {
                    Counter =1,
                    Version ="0.30.00",
                    Description ="包含系统用户和角色、组织机构和人员、供应商和采购商组织机构、商品基础信息、订单模型、订单层级合成与分解等系统中最基本的数据应用模型。这一期应用以桌面浏览器应用提供。",
                    StartDate ="2019-11-02",
                    FinishedDate ="2019-12-20"
                },
                new SprintItem()
                {
                    Counter =2,
                    Version ="0.40.00",
                    Description ="实现采购商形成初步的线上采购作业，提交采购订单。实现供应商初步的商品供应管理，可响应采购订单。",
                    StartDate ="2019-12-23",
                    FinishedDate ="2020-01-20"
                },
                new SprintItem()
                {
                    Counter =3,
                    Version ="0.60.00 [预览版本] ",
                    Description ="实现采购订单线上交易撮合服务，构成商品交易数据链。实现采购订单线上交易撮合服务，构成商品交易数据链。首个预览发布版本，包括桌面的和移动 APP的。",
                    StartDate ="2020-02-03",
                    FinishedDate ="2020-03-23"
                },
                new SprintItem()
                {
                    Counter =4,
                    Version ="0.70.00 ",
                    Description ="持续优化桌面的和移动 APP的应用，增加物流服务、金融服务、即时消息、通知消息服务。",
                    StartDate ="2020-03-26",
                    FinishedDate ="2020-04-22"
                },
                new SprintItem()
                {
                    Counter =5,
                    Version ="0.80.00 ",
                    Description ="持续优化桌面的和移动 APP的应用，增加社区电商服务平台，进一步优化营运单位内部管理功能。",
                    StartDate ="2020-04-27",
                    FinishedDate ="2020-05-11"
                },
                new SprintItem()
                {
                    Counter =6,
                    Version ="1.00.00 ",
                    Description ="持续优化桌面的和移动 APP的应用，进一步优化营运单位内部管理功能。",
                    StartDate ="2020-05-15",
                    FinishedDate ="2020-06-30"
                },
            };
            ViewData["SprintItemCollection"] = sprintItemCollection;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
