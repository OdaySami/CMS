using CMC.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMC.Infrastructure.Services.Notifications
{
    public interface INotificationService
    {
        Task<bool> SendByFCM(string token, NotificationDto dto);
    }
}
