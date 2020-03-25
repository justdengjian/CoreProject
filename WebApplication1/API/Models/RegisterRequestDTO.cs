using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.API.Models
{
    public class RegisterRequestDTO
    {

        [Required]
        [JsonProperty("userName")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [JsonProperty("email")]
        public string Email { get; set; }

        [Required]
        [StringLength(16, ErrorMessage = "{0}长度必须大于{2}位小于{1}位", MinimumLength = 6)]
        [JsonProperty("password")]
        public string Password { get; set; }

        [Required]
        [JsonProperty("confirm")]
        [Compare("Password", ErrorMessage = "两次密码不一致")]
        [DataType(DataType.Password)]
        public string Confirm { get; set; }
    }
}
