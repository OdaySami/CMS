﻿using CMC.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMC.Infrastructure.Services
{
    public interface IDashboardService
    {
        Task<DashboardViewModel> GetData();
        Task<List<PieChartViewModel>> GetUserTypeChart();
        Task<List<PieChartViewModel>> GetContentTypeChart();
        Task<List<PieChartViewModel>> GetContentByMonthChart();
    }
}
