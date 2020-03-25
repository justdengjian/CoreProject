using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ViewModel
{
    public class LoginViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "密码")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "记住登录状态")]
        public bool RememberMe { get; set; }
    }
}
