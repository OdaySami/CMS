using CMC.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMC.Core.ViewModels
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int TimeInMinute { get; set; }  // وقت القراءة
        public CategoryViewModel Category { get; set; }
        public string CreatedAt { get; set; }
        public UserViewModel Author { get; set; }
        public string Status { get; set; }
        public List<PostAttachmentViewModel> Attachments { get; set; }
    }
}
