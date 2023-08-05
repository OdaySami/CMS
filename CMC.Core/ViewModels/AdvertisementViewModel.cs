using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMC.Core.ViewModels
{
    public class AdvertisementViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public string WebsiteUrl { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public float Price { get; set; }
        public UserViewModel Owner { get; set; }
    }
}
