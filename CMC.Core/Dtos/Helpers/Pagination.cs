using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMC.Core.Dtos
{
   public class Pagination
    {
        public int PerPage { get; set; } // عدد الصفوف في الصفحة
        public int Page { get; set; }   // رقم الصفحة الحالية
        public int Pages { get; set; }  // عدد الصفحات
        public int Total { get; set; }  // عدد User
    }
}
