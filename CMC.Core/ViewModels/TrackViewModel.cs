using CMC.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMC.Core.ViewModels
{
    public class TrackViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public int TimeInMinute { get; set; }
        [Required]
        public string TrackUrl { get; set; }
        [Required]
        public string OwnerName { get; set; }
        public CategoryViewModel Category { get; set; }
        public UserViewModel PublishedBy { get; set; }
        public string Status { get; set; }
        public string CreateAt { get; set; }


    }
}
