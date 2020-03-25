using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.API.Models
{
    public class TokenStr
    {
        [Required]
        [JsonProperty("token")]
        public string tokenstr { get; set; }
    }
}
