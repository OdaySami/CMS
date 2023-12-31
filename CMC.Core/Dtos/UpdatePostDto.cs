﻿using CMC.Core.Enums;
using CMC.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMC.Core.Dtos
{
    public class UpdatePostDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [Display(Name = "عنوان الخبر ")]
        public string Title { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [Display(Name = "تفاصيل الخبر ")]
        public string Body { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [Display(Name = "وقت القراءة ")]
        public int TimeInMinute { get; set; }  // وقت القراءة
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [Display(Name = "التصنيف")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [Display(Name = "المحرر")]
        public string AuthorId { get; set; }
        [Display(Name = " المرفقات ")]
        public List<IFormFile> Attachments { get; set; }
        public List<PostAttachmentViewModel> PostAttachments { get; set; }
    }
}
