using CMC.Core.ViewModels;
using CMC.Infrastructure.Services;
using CMC.Infrastructure.Services.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CMC.Web.Controllers
{ 
    public class HomeController : BaseController
    {
        
            private readonly IDashboardService _dashboardService;

        public HomeController(IDashboardService dashboardService, IUserServices userServices) : base(userServices)
        {
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            if (userType != "Administrator")
            {
                return Redirect("/Category");
            }
            var data = await _dashboardService.GetData();
            return View(data);
        }


        public async Task<IActionResult> GetUserTypeChartData()
        {
            var data = await _dashboardService.GetUserTypeChart();
            return Ok(data);
        }

        public async Task<IActionResult> GetContentTypeChartData()
        {
            var data = await _dashboardService.GetContentTypeChart();
            return Ok(data);
        }

        public async Task<IActionResult> GetContentByMonthChartData()
        {
            var data = await _dashboardService.GetContentByMonthChart();
            return Ok(data);
        }
    }
    
}
