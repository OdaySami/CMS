using CMC.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMC.Data.Models
{
    public class Notifications
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public NotificationAction Action { get; set; }
        public string ActionId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime SendAt { get; set; }



    }
}
