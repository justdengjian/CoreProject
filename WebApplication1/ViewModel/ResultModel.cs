using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ViewModel
{
    public class ResultModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(10)]
        [Display(Name = "姓名")]
        public string StuName { get; set; }
        [Required]
        [MaxLength(100)]
        [Display(Name = "标题")]
        public string Title { get; set; }
        [Required]
        [Display(Name = "成果概述")]
        public string Description { get; set; }

        [Display(Name = "成果类型")]
        public int TypeId { get; set; }

    }
}
